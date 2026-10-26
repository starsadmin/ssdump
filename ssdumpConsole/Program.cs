using Mono.Options;
using System;
using System.Collections.Generic;

namespace ssdump
{
    class Program
    {
        /// <summary>
        /// Program starts here.
        /// </summary>
        /// <param name="args">Command-line options</param>
        static void Main(string[] args)
        {
            try
            {
                // Variables for holding command-line option results, with defaults.
                bool help = false;
                string outputPath = null;
                int maxFileSizeMegabytes = 1536; // 1.5 GB
                DumpProcessor processor = new();

                // Option set, maps command-line options to variables above.
                OptionSet options = new()
                {
                    { "create-db", "Include CREATE DATABASE statement", value => processor.IncludeCreateDatabase = value != null },
                    { "data-dir=", "With --create-db, put data files (.mdf/.ndf) in this folder on the target server", value => processor.DataDirectory = value },
                    { "encrypt","Use encrypted connection", value => processor.UseEncryption = value != null },
                    { "help", "Show help message", value => help = value != null },
                    { "h|host=", "Connect to host", value => processor.Host = value },
                    { "include-users", "Include CREATE USER statements", value => processor.IncludeUsers = value != null },
                    { "d|no-data", "No row information", value => processor.NoData = value != null },
                    { "no-prefetch", "Don't prefetch table details in bulk; SMO loads them table by table instead (for troubleshooting)", value => processor.NoPrefetch = value != null },
                    { "include-data-schemas=", "Only include data from these schemas (comma-separated)", value => processor.IncludedDataSchemas = value },
                    { "include-data-tables=", "Only include data from these tables (comma-separated, table or schema.table)", value => processor.IncludedDataTables = value },
                    { "log-dir=", "With --create-db, put log files (.ldf) in this folder on the target server (default: --data-dir)", value => processor.LogDirectory = value },
                    { "max-allowed-packet=", "Maximum packet length", (int value) => processor.MaxPacket = value },
                    { "max-file-size=", "With --output, split into files of at most this many MB, named name-001.sql, name-002.sql, ... (default 1536; 0 = one file)", (int value) => maxFileSizeMegabytes = value },
                    { "o|output=", "Write to this file instead of the console", value => outputPath = value },
                    { "p|password=", "Password to use when connecting to server", value => processor.Password = value },
                    { "timeout=", "Connection timeout (seconds)", (int value) => processor.Timeout = value },
                    { "u|user=", "User to use if not current user (SQL Server authentication)", value => processor.Username = value },
                    { "verbose", "Log each SQL statement sent to the server to stderr, with timestamps", value => processor.Verbose = value != null }
                };

                // Parse command-line options.
                List<string> extras;
                try
                {
                    extras = options.Parse(args);
                }
                catch (OptionException exception)
                {
                    Console.Error.WriteLine("Error parsing command-line options:");
                    Console.Error.WriteLine(exception.Message);
                    return;
                }

                // Check command-line parameters and make sure that everything is in order.
                if (!help && extras.Count == 0)
                {
                    Console.Error.WriteLine("Name of the database to dump is required.");
                    help = true;
                }

                if (!help && (processor.Password == null && processor.Username != null) || (processor.Password != null && processor.Username == null))
                {
                    Console.Error.WriteLine("To use SQL Server authentication, both user and password must be provided.");
                    help = true;
                }

                if (!help && (processor.DataDirectory != null || processor.LogDirectory != null) && !processor.IncludeCreateDatabase)
                {
                    Console.Error.WriteLine("--data-dir and --log-dir only apply with --create-db.");
                    help = true;
                }

                if (processor.Password != null && processor.Username != null)
                {
                    processor.UseWindowsAuthentication = false;
                }

                if (outputPath != null)
                {
                    processor.WriteToConsole = false;
                    processor.FilePath = outputPath;
                    if (maxFileSizeMegabytes > 0)
                        processor.MaxFileSize = maxFileSizeMegabytes * 1024L * 1024L;
                } else
                {
                    processor.WriteToConsole = true;
                    System.IO.TextWriter consoleOutput = Console.Out;
                }

                // Do what was requested.
                if (help)
                {
                    ShowHelp(options);
                }
                else
                {
                    processor.DatabaseName = WhichDatabases(extras)[0];
                    processor.Tables = WhichTables(extras);
                    processor.Execute();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine();
                Console.Error.WriteLine("Exception: " + exception.Message);
                if (exception.InnerException != null)
                    Console.Error.WriteLine("Inner Exception: " + exception.InnerException.Message);
                Console.WriteLine();
                Console.Error.WriteLine(exception.StackTrace);
            }
        }

        /// <summary>
        /// Figure out which databases to dump.
        /// </summary>
        /// <param name="extras">Unparsed command-line options</param>
        /// <returns>List of databases to dump</returns>
        private static List<string> WhichDatabases(List<string> extras)
        {
            List<string> databases = [];

            // First extra is database to dump.
            databases.Add(extras[0]);

            return databases;
        }

        /// <summary>
        /// Figure out which tables to dump.
        /// </summary>
        /// <param name="extras">Unparsed command-line options</param>
        /// <returns>List of tables to dump</returns>
        private static List<string> WhichTables(List<string> extras)
        {
            List<string> tables = [];

            // It will be an empty list (meaning all tables) unless...
            if (extras.Count > 1)
            {
                // Everything except first value is a table to dump.
                for (int index = 1; index < extras.Count; index++)
                {
                    tables.Add(extras[index]);
                }
            }

            return tables;
        }

        /// <summary>
        /// Show help message, which includes command-line option descriptions.
        /// </summary>
        /// <param name="options"></param>
        private static void ShowHelp(OptionSet options)
        {
            Console.WriteLine();
            Console.WriteLine(DumpProcessor.ProgramName + " v" + DumpProcessor.ProgramVersion);
            Console.WriteLine("Dumping structure and contents of Microsoft SQL Server databases and tables.");
            Console.WriteLine("Usage: ssdump [OPTIONS] database [table1 table2 ...]");
            Console.WriteLine();
            options.WriteOptionDescriptions(Console.Out);
        }
    }
}