using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public string FilePath { get; set; }

        public int Timeout { get; set; }
        public int? MaxPacket { get; set; }

        public bool UseWindowsAuthentication { get; set; }
        public bool UseEncryption { get; set; }
        public bool NoData { get; set; }
        public bool IncludeCreateDatabase { get; set; }
        public bool IncludeUsers { get; set; }

        public bool WriteToConsole { get; set; }

        public List<string> Tables { get; set; }
        public List<string> ExtraStatements { get; set; }

        public DumpProcessor()
        {
            // Set default values.
            Host = "localhost";
            IncludeCreateDatabase = false;
            IncludeUsers = false;
            NoData = false;
            MaxPacket = null;
            Password = null;
            Timeout = 10;
            Username = null;
            UseEncryption = false;
            UseWindowsAuthentication = true; WriteToConsole = false;
        }

        /// <summary>
        /// Process the request and save the result.
        /// </summary>
        public void Execute()
        {
            // Set up server object and connection options.
            Server server = new Server(Host);
            server.ConnectionContext.ApplicationName = ProgramName + " v" + ProgramVersion;
            server.ConnectionContext.ConnectTimeout = Timeout;
            server.ConnectionContext.LoginSecure = UseWindowsAuthentication;
            server.ConnectionContext.EncryptConnection = UseEncryption;

            if (Username != null && Username.Length > 0)
            {
                server.ConnectionContext.Login = Username;
                server.ConnectionContext.Password = Password;
            }

            if (MaxPacket != null && MaxPacket >= 0)
            {
                server.ConnectionContext.PacketSize = (int) MaxPacket;
            }

            // Connect.
            server.ConnectionContext.Connect();

            if (server.ConnectionContext.IsOpen)
            {
                // Set up scripter object and script options.
                Scripter scripter = new Scripter(server);

                // Extra SQL statements to be written out.
                ExtraStatements = new List<string>();

                // Set script options.
                scripter.Options.DriAll = true;
                scripter.Options.ScriptData = !NoData;
                scripter.Options.ScriptOwner = false;

                Database database = server.Databases[DatabaseName];

                // List of objects to dump.
                List<Urn> urns = new List<Urn>();

                // If no specific tables were requested, write out database info.
                if (Tables.Count == 0)
                {
                    if (IncludeCreateDatabase)
                    {
                        urns.Add(database.Urn);

                        foreach (ExtendedProperty extendedProperty in database.ExtendedProperties)
                        {
                            urns.Add(extendedProperty.Urn);
                        }
                    }

                    if (IncludeUsers)
                    {
                        // Also output users.
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
                    }

                    // Also output schemas.
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
                }

                // Loop through tables.
                foreach (Table table in database.Tables)
                {
                    // Output table if it was requested, or if no specific tables were requested.
                    if (Tables.Count == 0 || Tables.Contains(table.Name))
                    {
                        urns.Add(table.Urn);

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
                    }

                }

                // Loop through views.
                foreach (Microsoft.SqlServer.Management.Smo.View view in database.Views)
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

                // Don't forget stored procedures.
                if (Tables.Count == 0)
                {
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
                }

                // Dump everything.
                if (urns.Count > 0)
                {
                    WriteScripts(scripter, urns);
                }
            }
            else
            {
                throw new Exception("Not connected to server.");
            }

        }

        /// <summary>
        /// Write scripts to console.
        /// </summary>
        /// <param name="scripter">Scripter object with options to write scripts</param>
        /// <param name="scripter">List of objects to write out</param>
        private void WriteScripts(Scripter scripter, List<Urn> urns)
        {
            // https://connect.microsoft.com/SQLServer/feedback/details/363076/smo-create-procedure-generated-script-is-unusable
            // You must write to file in order to get "GO" statements to appear.
            string tempFilename = Path.GetTempFileName();
            scripter.Options.ToFileOnly = true;
            scripter.Options.FileName = tempFilename;

            scripter.EnumScriptWithList(urns.ToArray()); // Will write to file.
            string script = File.ReadAllText(tempFilename);
            File.Delete(tempFilename);

            TextWriter TextWriterStream;
            if (WriteToConsole)
            {
                TextWriterStream = Console.Out;
            }
            else
            {
                TextWriterStream = new StreamWriter(FilePath);
            }

            TextWriterStream.Write(script);

            if (ExtraStatements.Count > 0)
            {
                foreach (string extra in ExtraStatements)
                {
                    TextWriterStream.WriteLine(extra);
                    TextWriterStream.WriteLine("GO");
                }
            }
            TextWriterStream.Close();
        }
    }
}

