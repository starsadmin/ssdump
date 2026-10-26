using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text.RegularExpressions;

namespace ssdump
{
    public class DumpProcessor
    {
        /// <summary>
        /// Program name.
        /// </summary>
        public static readonly string ProgramName = "ssdump";

        /// <summary>
        /// Program version.
        /// </summary>
        public static readonly string ProgramVersion = "0.9.5";

        public string Host { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string DatabaseName { get; set; }
        public string IncludedDataSchemas { get; set; }
        public string IncludedDataTables { get; set; }
        public string FilePath { get; set; }

        public int Timeout { get; set; }
        public int? MaxPacket { get; set; }

        /// <summary>
        /// Maximum size of each output file in bytes. If set, output is split into FilePath-001.sql, FilePath-002.sql, etc.
        /// </summary>
        public long? MaxFileSize { get; set; }

        /// <summary>
        /// Number of INSERT statements per batch, so large tables can be split across files.
        /// 100 matches what SMO writes when it scripts data straight to a file.
        /// </summary>
        private const int RowsPerBatch = 100;

        public bool UseWindowsAuthentication { get; set; }
        public bool UseEncryption { get; set; }
        public bool NoData { get; set; }
        public bool IncludeCreateDatabase { get; set; }
        public bool IncludeUsers { get; set; }

        /// <summary>
        /// With IncludeCreateDatabase, folder for data files (.mdf/.ndf) in the CREATE DATABASE statement, instead of the source database's.
        /// </summary>
        public string DataDirectory { get; set; }

        /// <summary>
        /// With IncludeCreateDatabase, folder for log files (.ldf) in the CREATE DATABASE statement. Defaults to DataDirectory.
        /// </summary>
        public string LogDirectory { get; set; }

        /// <summary>
        /// Original database file path to new path, for rewriting the CREATE DATABASE statement (see RewriteDatabaseFilePaths).
        /// </summary>
        private Dictionary<string, string> databaseFilePaths = [];

        /// <summary>
        /// Skip prefetching table details, both up front and inside the scripter, so SMO loads them object by object instead.
        /// For troubleshooting slow or hanging prefetches.
        /// </summary>
        public bool NoPrefetch { get; set; }

        /// <summary>
        /// Log every SQL statement SMO sends to the server to stderr, with timestamps.
        /// </summary>
        public bool Verbose { get; set; }

        public bool WriteToConsole { get; set; }

        public List<string> Tables { get; set; }
        public List<string> ExtraStatements { get; set; }

        public DumpProcessor()
        {
            // Set default values.
            Host = "localhost";
            IncludeCreateDatabase = false;
            IncludeUsers = false;
            DataDirectory = null;
            LogDirectory = null;
            NoData = false;
            NoPrefetch = false;
            IncludedDataTables = null;
            IncludedDataSchemas = null;
            MaxPacket = null;
            MaxFileSize = null;
            Password = null;
            Timeout = 10;
            Username = null;
            UseEncryption = false;
            UseWindowsAuthentication = true;
            Verbose = false;
            WriteToConsole = false;
        }

        /// <summary>
        /// Process the request and save the result.
        /// </summary>
        public void Execute()
        {
            // Set up server object and connection options.
            Server server = new(Host);
            server.ConnectionContext.ApplicationName = ProgramName + " v" + ProgramVersion;
            server.ConnectionContext.ConnectTimeout = Timeout;
            server.ConnectionContext.LoginSecure = UseWindowsAuthentication;
            server.ConnectionContext.EncryptConnection = UseEncryption;

            // Connect to the database itself, not master, so it stays open for the whole dump. SMO opens a new connection
            // for each table's data; with AUTO_CLOSE on (the default on Express/LocalDB) the database otherwise closes and
            // reopens between tables, which made scripting data about 10x slower in testing.
            server.ConnectionContext.DatabaseName = DatabaseName;

            if (Username != null && Username.Length > 0)
            {
                server.ConnectionContext.Login = Username;
                server.ConnectionContext.Password = Password;
            }

            if (MaxPacket != null && MaxPacket >= 0)
            {
                server.ConnectionContext.PacketSize = (int) MaxPacket;
            }

            // Fetch IsSystemObject when listing these objects. Otherwise SMO queries the server once per object
            // when it's checked below, which is slow: a database can have over a thousand system procedures and views.
            server.SetDefaultInitFields(typeof(StoredProcedure), "IsSystemObject");
            server.SetDefaultInitFields(typeof(View), "IsSystemObject");
            server.SetDefaultInitFields(typeof(UserDefinedFunction), "IsSystemObject");
            server.SetDefaultInitFields(typeof(Schema), "IsSystemObject");
            server.SetDefaultInitFields(typeof(User), "IsSystemObject");

            if (Verbose)
            {
                server.ConnectionContext.StatementExecuted += LogStatement;
            }

            // Connect.
            server.ConnectionContext.Connect();

            Console.WriteLine($"Opened connection to [{Host}].[{DatabaseName}]");
            if (server.ConnectionContext.IsOpen)
            {
                // Without VIEW DEFINITION, the server hides object definitions (defaults, views, procedures, etc.) and SMO
                // either fails partway through or hangs. Checking up front gives a clear error instead.
                object hasViewDefinition = server.ConnectionContext.ExecuteScalar("SELECT HAS_PERMS_BY_NAME(DB_NAME(), 'DATABASE', 'VIEW DEFINITION')");
                if (!(hasViewDefinition is int permission && permission == 1))
                {
                    throw new Exception($"Your login needs the VIEW DEFINITION permission on [{DatabaseName}] to script it. Ask a DBA to run: GRANT VIEW DEFINITION TO [<your user>];");
                }

                // Set up scripter object and script options.
                Scripter scripter = new(server);

                // Extra SQL statements to be written out.
                ExtraStatements = [];

                // Set script options.
                // Data is scripted in a separate data-only pass, so it can be limited to specific tables (see WriteDataScripts).
                // Foreign keys are also scripted separately, after the data, so INSERT order can't violate them (see WriteForeignKeyScripts).
                // These flags are DriAll minus DriForeignKeys; DriAll can't be used, as it overrides DriForeignKeys = false.
                scripter.Options.DriPrimaryKey = true;
                scripter.Options.DriUniqueKeys = true;
                scripter.Options.DriChecks = true;
                scripter.Options.DriDefaults = true;
                scripter.Options.DriIndexes = true;
                scripter.Options.DriClustered = true;
                scripter.Options.DriNonClustered = true;
                scripter.Options.ScriptData = false;
                scripter.Options.ScriptOwner = false;

                Database database = server.Databases[DatabaseName];

                // List of objects to dump.
                List<Urn> urns = [];

                // Tables whose data should be dumped. With no schema/table filter, that is every dumped table.
                List<Urn> dataUrns = [];
                HashSet<string> includedDataSchemas = ParseList(IncludedDataSchemas);
                HashSet<string> includedDataTables = ParseList(IncludedDataTables);

                // Foreign keys of dumped tables, written last.
                List<Urn> foreignKeyUrns = [];

                // If no specific tables were requested, write out database info.
                if (Tables.Count == 0)
                {
                    if (IncludeCreateDatabase)
                    {
                        urns.Add(database.Urn);

                        // SMO has no scripting option for file locations; it copies the source database's paths.
                        // So note each file's new path here, and WriteScripts swaps them into the script.
                        databaseFilePaths = [];
                        string logDirectory = LogDirectory ?? DataDirectory;
                        if (DataDirectory != null)
                        {
                            foreach (FileGroup fileGroup in database.FileGroups)
                            {
                                foreach (DataFile dataFile in fileGroup.Files)
                                {
                                    databaseFilePaths[dataFile.FileName] = Path.Combine(DataDirectory, Path.GetFileName(dataFile.FileName));
                                }
                            }
                        }
                        if (logDirectory != null)
                        {
                            foreach (LogFile logFile in database.LogFiles)
                            {
                                databaseFilePaths[logFile.FileName] = Path.Combine(logDirectory, Path.GetFileName(logFile.FileName));
                            }
                        }

                        Console.Write("Adding extended properties...");
                        foreach (ExtendedProperty extendedProperty in database.ExtendedProperties)
                        {
                            urns.Add(extendedProperty.Urn);
                        }
                        Console.WriteLine(" done");
                    }

                    if (IncludeUsers)
                    {
                        // Also output users.
                        Console.Write("Adding database users...");
                        foreach (User user in database.Users)
                        {
                            if (!user.IsSystemObject)
                            {
                                urns.Add(user.Urn);

                                foreach (ExtendedProperty extendedProperty in user.ExtendedProperties)
                                {
                                    urns.Add(extendedProperty.Urn);
                                }

                                StringCollection userRoles = user.EnumRoles();
                                foreach (string userRole in userRoles)
                                {
                                    // Can't find a way to make the code do this...
                                    ExtraStatements.Add(string.Format("ALTER ROLE [{0}] ADD MEMBER [{1}]", userRole, user.Name));
                                }
                            }
                        }
                        Console.WriteLine(" done");
                    }

                    // Also output schemas.
                    Console.Write("Adding schemas...");
                    foreach (Schema schema in database.Schemas)
                    {
                        if (!schema.IsSystemObject)
                        {
                            urns.Add(schema.Urn);

                            foreach (ExtendedProperty extendedProperty in schema.ExtendedProperties)
                            {
                                urns.Add(extendedProperty.Urn);
                            }
                        }
                    }
                    Console.WriteLine(" done");
                }

                // When dumping every table, load their columns, indexes, keys, triggers etc. in a few queries,
                // instead of SMO querying the server separately for each table.
                if (Tables.Count == 0 && !NoPrefetch)
                {
                    Console.Write("Prefetching every table's colunms, indices, keys, triggers, etc...");
                    database.PrefetchObjects(typeof(Table), scripter.Options);
                    Console.WriteLine(" done");
                }

                // Loop through tables.
                Console.WriteLine("Adding prefetched tables, columns, indices, keys, trigggers, and extended properties...");
                foreach (Table table in database.Tables)
                {
                    // Output table if it was requested, or if no specific tables were requested.
                    if (Tables.Count == 0 || Tables.Contains(table.Name))
                    {
                        Console.WriteLine($"\t[{table.Schema}].[{table.Name}]");
                        urns.Add(table.Urn);

                        // If the "no-data" flag is set, don't include data contained in tables.
                        if (!NoData)
                        {
                            // If "include-data-tables" or "include-data-schemas" is set, only include data for specific tables.
                            if ((includedDataSchemas.Count == 0 && includedDataTables.Count == 0)
                            || includedDataSchemas.Contains(table.Schema)
                            || includedDataTables.Contains(table.Name)
                            || includedDataTables.Contains(table.Schema + "." + table.Name)) // Tables may be named either "table" or "schema.table".
                            {
                                dataUrns.Add(table.Urn);
                            }
                        }

                        foreach (ExtendedProperty extendedProperty in table.ExtendedProperties)
                        {
                            urns.Add(extendedProperty.Urn);
                        }

                        foreach (Microsoft.SqlServer.Management.Smo.Index index in table.Indexes)
                        {
                            urns.Add(index.Urn);

                            foreach (ExtendedProperty extendedProperty in index.ExtendedProperties)
                            {
                                urns.Add(extendedProperty.Urn);
                            }
                        }

                        foreach (Trigger trigger in table.Triggers)
                        {
                            urns.Add(trigger.Urn);

                            foreach (ExtendedProperty extendedProperty in trigger.ExtendedProperties)
                            {
                                urns.Add(extendedProperty.Urn);
                            }
                        }

                        foreach (ForeignKey foreignKey in table.ForeignKeys)
                        {
                            foreignKeyUrns.Add(foreignKey.Urn);

                            foreach (ExtendedProperty extendedProperty in foreignKey.ExtendedProperties)
                            {
                                foreignKeyUrns.Add(extendedProperty.Urn);
                            }
                        }
                    }
                }

                // Loop through views.
                Console.Write("Adding database views...");
                foreach (View view in database.Views)
                {
                    // Output view if it was requested, or if no specific views were requested.
                    if (Tables.Count == 0 || Tables.Contains(view.Name))
                    {
                        if (!view.IsSystemObject)
                        {
                            urns.Add(view.Urn);

                            foreach (ExtendedProperty extendedProperty in view.ExtendedProperties)
                            {
                                urns.Add(extendedProperty.Urn);
                            }
                        }
                    }
                }
                Console.WriteLine(" done");

                // Don't forget stored procedures.
                if (Tables.Count == 0)
                {
                    Console.Write("Adding stored procedures...");
                    foreach (StoredProcedure storedProcedure in database.StoredProcedures)
                    {
                        if (!storedProcedure.IsSystemObject)
                        {
                            urns.Add(storedProcedure.Urn);

                            foreach (ExtendedProperty extendedProperty in storedProcedure.ExtendedProperties)
                            {
                                urns.Add(extendedProperty.Urn);
                            }
                        }
                    }
                    Console.WriteLine(" done");

                    // Views can call functions, so they must be scripted too.
                    Console.Write("Adding user-defined functions...");
                    foreach (UserDefinedFunction userDefinedFunction in database.UserDefinedFunctions)
                    {
                        if (!userDefinedFunction.IsSystemObject)
                        {
                            urns.Add(userDefinedFunction.Urn);

                            foreach (ExtendedProperty extendedProperty in userDefinedFunction.ExtendedProperties)
                            {
                                urns.Add(extendedProperty.Urn);
                            }
                        }
                    }
                    Console.WriteLine(" done");
                }

                // Dump everything.
                if (urns.Count > 0)
                {
                    Console.Write($"Writing scripts to {FilePath}    ...");
                    WriteScripts(scripter, urns, dataUrns, foreignKeyUrns);
                    Console.WriteLine(" done");
                }
            }
            else
            {
                throw new Exception("Not connected to server.");
            }

        }

        /// <summary>
        /// Write scripts to the console or to file(s).
        /// </summary>
        /// <param name="scripter">Scripter object with options to write scripts</param>
        /// <param name="urns">List of objects to write out</param>
        /// <param name="dataUrns">Tables to write data for</param>
        /// <param name="foreignKeyUrns">Foreign keys to write after the data</param>
        private void WriteScripts(Scripter scripter, List<Urn> urns, List<Urn> dataUrns, List<Urn> foreignKeyUrns)
        {
            using ScriptFileWriter writer = WriteToConsole ? new(Console.Out) : new(FilePath, MaxFileSize);

            // https://connect.microsoft.com/SQLServer/feedback/details/363076/smo-create-procedure-generated-script-is-unusable
            // You must write to file in order to get "GO" statements to appear.
            string tempFilename = Path.GetTempFileName();
            try
            {
                scripter.Options.ToFileOnly = true;
                scripter.Options.FileName = tempFilename;

                // The scripter also prefetches child objects itself while scripting, separately from database.PrefetchObjects.
                // Only turned off for this schema pass: with it off, SMO's data scripting queries tables from the wrong database.
                scripter.PrefetchObjects = !NoPrefetch;
                scripter.EnumScriptWithList(urns.ToArray()); // Will write database/schemas/tables to file.
                scripter.PrefetchObjects = true;
                RewriteDatabaseFilePaths(tempFilename);
                writer.CopyFrom(tempFilename);

                if (!NoData && dataUrns.Count > 0)
                {
                    WriteDataScripts(scripter, dataUrns, writer); // Writes INSERT statements, if options allow.
                }

                if (foreignKeyUrns.Count > 0)
                {
                    WriteForeignKeyScripts(scripter, foreignKeyUrns); // Writes foreign keys after data, to avoid foreign key constraint errors.
                    writer.CopyFrom(tempFilename);
                }
            }
            finally
            {
                File.Delete(tempFilename);
            }

            foreach (string extra in ExtraStatements)
            {
                writer.WriteLine(extra);
                writer.EndBatch();
            }
        }

        /// <summary>
        /// Swap the database file paths in a scripted CREATE DATABASE statement for the ones in databaseFilePaths.
        /// </summary>
        /// <param name="scriptFilePath">Schema script written by SMO (no data, so small enough to read whole)</param>
        private void RewriteDatabaseFilePaths(string scriptFilePath)
        {
            if (databaseFilePaths.Count == 0)
                return;

            string script;
            System.Text.Encoding encoding;
            using (StreamReader reader = new(scriptFilePath, detectEncodingFromByteOrderMarks: true))
            {
                script = reader.ReadToEnd();
                encoding = reader.CurrentEncoding; // Write it back the way SMO wrote it.
            }

            foreach (KeyValuePair<string, string> path in databaseFilePaths)
            {
                // Match the whole N'...' literal, with quotes escaped as SQL does, so only exact paths are replaced.
                script = script.Replace("N'" + path.Key.Replace("'", "''") + "'", "N'" + path.Value.Replace("'", "''") + "'", StringComparison.OrdinalIgnoreCase);
            }

            File.WriteAllText(scriptFilePath, script, encoding);
        }

        /// <summary>
        /// Write INSERT statements for the given tables, parent tables first, in batches of RowsPerBatch rows.
        /// </summary>
        /// <param name="scripter">Scripter object with options to write scripts</param>
        /// <param name="dataUrns">Tables to write data for</param>
        /// <param name="writer">Where to write the statements</param>
        private static void WriteDataScripts(Scripter scripter, List<Urn> dataUrns, ScriptFileWriter writer)
        {
            Dictionary<string, Urn> selected = new(StringComparer.OrdinalIgnoreCase);
            foreach (Urn urn in dataUrns)
            {
                selected[TableKey(urn)] = urn;
            }

            // The walk returns the selected tables plus every table they reference, ordered so that
            // referenced (parent) tables come before the tables that reference them.
            DependencyTree tree = scripter.DiscoverDependencies(dataUrns.ToArray(), DependencyType.Parents);
            DependencyCollection dependencies = scripter.WalkDependencies(tree);

            List<Urn> orderedUrns = [];
            foreach (DependencyCollectionNode node in dependencies)
            {
                if (node.Urn.Type != "Table")
                    continue;

                if (selected.TryGetValue(TableKey(node.Urn), out Urn urn))
                {
                    orderedUrns.Add(urn);
                }
                else
                {
                    // Rows in the selected tables may point at rows in this table, which won't be in the dump.
                    Console.Error.WriteLine(string.Format("Warning: data for {0} is not included, but is referenced by included tables.", TableKey(node.Urn)));
                }
            }

            // Second pass: data only. SMO returns one statement at a time when not writing to file,
            // so we can add "GO" every RowsPerBatch rows, giving places to split large tables across files.
            scripter.Options.ScriptSchema = false;
            scripter.Options.ScriptData = true;
            scripter.Options.ScriptForCreateOrAlter = false; // SMO throws "no collection of Data objects" when scripting data with this on.
            scripter.Options.ToFileOnly = false;

            // One table at a time, so the output follows the dependency order above exactly.
            foreach (Urn urn in orderedUrns)
            {
                int rowsInBatch = 0;
                foreach (string statement in scripter.EnumScript(new Urn[] { urn }))
                {
                    string trimmed = statement.TrimEnd();
                    if (trimmed.StartsWith("SET IDENTITY_INSERT ", StringComparison.OrdinalIgnoreCase) && trimmed.EndsWith(" ON", StringComparison.OrdinalIgnoreCase))
                    {
                        // IDENTITY_INSERT only lasts for one connection, so if this table continues in a new file,
                        // the writer turns it off at the end of the old file and back on at the top of the new one.
                        // It's written as its own batch first, so a new file never turns it on twice.
                        writer.WriteLine(statement);
                        writer.EndBatch();
                        writer.StartSession(trimmed, trimmed[..^"ON".Length] + "OFF");
                        continue;
                    }

                    writer.WriteLine(statement);

                    if (trimmed.StartsWith("INSERT ", StringComparison.OrdinalIgnoreCase) && ++rowsInBatch == RowsPerBatch)
                    {
                        writer.EndBatch();
                        rowsInBatch = 0;
                    }
                }

                writer.EndBatch();
                writer.EndSession();
            }
        }

        /// <summary>
        /// Write ALTER TABLE ... ADD FOREIGN KEY statements to the scripter's file, to go after all tables and data.
        /// </summary>
        /// <param name="scripter">Scripter object already set up with a file name</param>
        /// <param name="foreignKeyUrns">Foreign keys (and their extended properties) to write</param>
        private static void WriteForeignKeyScripts(Scripter scripter, List<Urn> foreignKeyUrns)
        {
            // Last pass: schema only, overwriting the scripter's file (its previous contents were already copied out).
            scripter.Options.ScriptSchema = true;
            scripter.Options.ScriptData = false;
            scripter.Options.DriForeignKeys = true;
            scripter.Options.ToFileOnly = true;
            scripter.Options.AppendToFile = false;
            scripter.EnumScriptWithList(foreignKeyUrns.ToArray()); // Will write to file.
        }

        /// <summary>
        /// Time of the previous statement logged by LogStatement.
        /// </summary>
        private DateTime? lastStatementTime;

        /// <summary>
        /// Write one line to stderr for each statement SMO sends, for --verbose.
        /// </summary>
        private void LogStatement(object sender, StatementEventArgs e)
        {
            // SMO raises this as each statement is sent, so a long gap before a line means the line above it was slow.
            string gap = lastStatementTime == null ? "" : string.Format(" +{0:0.000}s", (e.TimeStamp - lastStatementTime.Value).TotalSeconds);
            lastStatementTime = e.TimeStamp;

            // One line per statement, so the log stays readable.
            string statement = Regex.Replace(e.SqlStatement, @"\s+", " ").Trim();
            if (statement.Length > 200)
            {
                statement = statement[..200] + "...";
            }

            Console.Error.WriteLine(string.Format("[{0:HH:mm:ss.fff}{1}] {2}", e.TimeStamp, gap, statement));
        }

        /// <summary>
        /// Split a comma-separated command-line value into a case-insensitive set.
        /// </summary>
        private static HashSet<string> ParseList(string value)
        {
            HashSet<string> result = new(StringComparer.OrdinalIgnoreCase);
            if (value != null)
            {
                foreach (string item in value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                {
                    result.Add(item);
                }
            }
            return result;
        }

        /// <summary>
        /// "schema.table" key for a table URN, used to match URNs that come from different SMO calls.
        /// </summary>
        private static string TableKey(Urn urn)
        {
            return urn.GetAttribute("Schema") + "." + urn.GetAttribute("Name");
        }
    }
}

