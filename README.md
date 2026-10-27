# ssdump

## What is it?

ssdump is a command line tool for dumping a Microsoft SQL Server database.  It can dump the schema only (tables, views, stored procedures, etc.), the schema with the data included, or you can pick and choose which table schemas and/or which table's data is included.  The output is a SQL script that can be imported into another SQL Server instance.  It is intended to be similar to the tool "mysqldump" which provides similar functionality for MySQL and MariaDB databases.  mysqldump was used as a starting point when coming up with the command line parameters for ssdump.

It is possible to generate SQL script dumps like this using the GUI tool Microsoft SQL Server Management Studio (SSMS).  In SSMS, you can right-click on a database and select "Tasks &rarr; Generate Scripts" to get a GUI wizard that walks you through creating such a dump.  The intention of ssdump is to allow for this process to be automated or scripted.  In fact, ssdump links with the SQL Server Management Studio libraries in order to generate the scripts.  Most of the options available in the SSMS wizard should be available via the libraries as well, but I have no exposed all of them in this program.

I created ssdump with the intention of using it to dump the database schema only and then load that into a version control system, in order to track changes to the schema, stored procedure code, and so forth, because I was unable to find an existing tool that allowed this.  After I got it working, I added a few options that seemed useful and then tossed it up here on GitHub.

## Getting started

Run the application from the command line with no parameters to get some help text explaining how to use it and what the available parameters are.  Parameter format is similar to mysqldump.

A simple example of a schema dump into a single file could look like:

```
ssdump MyDatabase --host=sqlserver.company.com --user=ssdump_user --password=BEgETDp4FnHu8xzzRMwr --no-data > output.sql
```

If no username or password is provided on the command line, ssdump will use Windows authentication.  Providing the uername and password uses SQL Server authentication.

Here's a more complex example, to show off some of the other options:
- Only includes the DimAccount and DimCurrency table schemas
- Windows Authentication (happens when there is no --user). Uses credentials of the user who is running the program.
- Includes CREATE DATABASE statement
- Specifies the directory where the new database should store its data
- Specifies the directory where the new database should store its logs
- Only includes data (INSERT statements) for DimAccount. Foreign keys are created last, so it's safe to only include data from tables with foreign keys to blank tables.
- Outputs file(s) to a path on the local machine. If this option is present, the console will show progress instead of printing the script to stdout.
- Splits output files into 1GB chunks (1024MB), to open them more easily in SQL Server Management Studio (the next best thing if you're not familiar with sqlcommand on the command line)
```
ssdump AdventureWorks [ DimAccount,DimCurrency ] --host sqlserver.company.com --create-db --data-dir "D:\MSSQL\Data" --log-dir "D:\MSSQL\Log" --include-data-tables "DimAccount" --output "C:\Users\Me\SQL Backups" --max-file-size 1024
```

ssdump uses the Microsoft.SqlServer.SqlManagementObjects NuGet package to do a lot of the work, but attempts to clean up the output a little bit and add extra features.
- Options to only include data from specific tables, without breaking foreign keys (foreign keys are added to the script after INSERT statements).
- Simplifies CREATE DATABASE statement, so it doesn't demand an enourmous amount of storage space if it's a big database.
- You may optionally specify data directory and log directory, as part of the CREATE DATABASE statement.
- Attempts to reorder procedures and functions, in order of dependency.

ssdump writes the SQL script to the console and it can be redirected to a file using a batch script or any other standard scripting tool.  Like SSMS, currently, ssdump can only dump one database at a time.

## GUI

A GUI tool, ssdumpGUI, is available as well.  A window pops up with information to fill in, but the process is the same as the command-line tool.

## Development

If you clone this repository, what you will get is a Visual Studio 2026 solution that should have everything that you need to build the program yourself.  Free versions of Visual Studio (Express or Community) should work fine.  The only other third-party dependency has been copied in for now, it is the file `ssdumpConsole/Mono.Options/Options.cs` which is a command line parameter parsing library from the [Mono project](https://github.com/mono/mono/tree/master/mcs/class/Mono.Options).

[This tutorial from MSSQLTips](https://www.mssqltips.com/sqlservertip/1833/generate-scripts-for-database-objects-with-smo-for-sql-server/) served as the starting point for this project.

## Support

If you find this tool useful and you have a feature that you would like to see added, submit an issue and I will take a look.  Or, implement it yourself and submit a pull request.

## License / Legal

You are free to fork or extend ssdump to suit your needs.  You may not bundle ssdump or any derivative as part of a commercial product.
