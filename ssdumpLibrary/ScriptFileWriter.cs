using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ssdump
{
    /// <summary>
    /// Writes a SQL script one batch at a time, optionally splitting it into numbered files (name-001.sql, name-002.sql, ...).
    /// Files are only split between batches (at a "GO"), so each file can be run on its own, in order.
    /// </summary>
    public class ScriptFileWriter : IDisposable
    {
        private readonly string filePath;
        private readonly long? maxFileSize;
        private readonly bool ownsWriter;
        private readonly StringBuilder batch = new();

        private TextWriter writer;
        private long currentFileSize;
        private int fileNumber;

        // Session setting in effect for the batches being written (see StartSession).
        private string sessionStartStatement;
        private string sessionEndStatement;

        // sessionEndStatement as it was when the last batch was written to the current file.
        private string sessionEndInCurrentFile;

        /// <summary>
        /// Statement written (with "GO") at the top of every numbered file after the first, e.g. "USE [db]".
        /// Each file is run on its own connection, so connection-wide context has to be set again in each one.
        /// </summary>
        public string FileStartStatement { get; set; }

        /// <summary>
        /// Write to an existing writer (e.g. the console), without splitting.
        /// </summary>
        public ScriptFileWriter(TextWriter writer)
        {
            this.writer = writer;
            ownsWriter = false;
        }

        /// <summary>
        /// Write to a file. If maxFileSize is set, write numbered files of at most that many bytes instead.
        /// </summary>
        public ScriptFileWriter(string filePath, long? maxFileSize)
        {
            this.filePath = filePath;
            this.maxFileSize = maxFileSize;
            ownsWriter = true;

            if (maxFileSize == null)
                OpenFile(filePath);
            else
                StartNextFile();
        }

        /// <summary>
        /// Add text to the current batch exactly as given; the caller includes any line endings.
        /// </summary>
        public void Write(string text)
        {
            batch.Append(text);
        }

        /// <summary>
        /// Add a line to the current batch.
        /// </summary>
        public void WriteLine(string text)
        {
            batch.Append(text).Append(Environment.NewLine);
        }

        /// <summary>
        /// End the current batch with "GO" and write it out, starting a new file first if it wouldn't fit.
        /// </summary>
        public void EndBatch()
        {
            WriteLine("GO");
            FlushBatch();
        }

        /// <summary>
        /// Mark a setting, e.g. "SET IDENTITY_INSERT [dbo].[T] ON", as needed by the batches that follow.
        /// Such settings only last for one connection, so if a new file is started, the old file ends with
        /// endStatement and the new file starts with startStatement. The caller writes startStatement itself.
        /// </summary>
        public void StartSession(string startStatement, string endStatement)
        {
            FlushBatch();
            sessionStartStatement = startStatement;
            sessionEndStatement = endStatement;

            // The caller has already written startStatement to the current file.
            sessionEndInCurrentFile = endStatement;
        }

        /// <summary>
        /// The setting from StartSession is no longer needed; the caller has already written its end statement.
        /// </summary>
        public void EndSession()
        {
            FlushBatch();
            sessionStartStatement = null;
            sessionEndStatement = null;
            sessionEndInCurrentFile = null;
        }

        /// <summary>
        /// Copy a script file written by SMO, keeping its text exactly and splitting only at its "GO" lines.
        /// </summary>
        public void CopyFrom(string scriptFilePath)
        {
            using StreamReader reader = new(scriptFilePath, detectEncodingFromByteOrderMarks: true);
            foreach (string line in ReadLinesWithEndings(reader))
            {
                if (line.TrimEnd('\r', '\n') == "GO")
                {
                    EndBatch();
                }
                else
                {
                    Write(line);
                }
            }

            // Anything after the last "GO" is still a complete batch.
            FlushBatch();
        }

        public void Dispose()
        {
            FlushBatch();

            if (ownsWriter)
                writer.Close();
            else
                writer.Flush();
        }

        private void FlushBatch()
        {
            if (batch.Length == 0)
                return;

            string text = batch.ToString();
            batch.Clear();
            long size = Encoding.UTF8.GetByteCount(text);

            // Only start a new file if this one already has something in it; a batch is never split.
            if (maxFileSize != null && currentFileSize > 0 && currentFileSize + size > maxFileSize)
            {
                StartNextFile();
            }

            writer.Write(text);
            currentFileSize += size;
            sessionEndInCurrentFile = sessionEndStatement;

            if (maxFileSize != null && currentFileSize > maxFileSize)
            {
                Console.Error.WriteLine(string.Format("Warning: a single batch is larger than the maximum file size; {0} is {1:N0} bytes.", NumberedFilePath(fileNumber), currentFileSize));
            }
        }

        private void StartNextFile()
        {
            if (writer != null)
            {
                if (sessionEndInCurrentFile != null)
                {
                    WriteDirect(sessionEndInCurrentFile + Environment.NewLine + "GO" + Environment.NewLine);
                }
                writer.Close();
            }

            fileNumber++;
            OpenFile(NumberedFilePath(fileNumber));
            sessionEndInCurrentFile = null;

            // Before the session statement, which may name a table in that database.
            if (fileNumber > 1 && FileStartStatement != null)
            {
                WriteDirect(FileStartStatement + Environment.NewLine + "GO" + Environment.NewLine);
            }

            if (sessionStartStatement != null)
            {
                WriteDirect(sessionStartStatement + Environment.NewLine + "GO" + Environment.NewLine);
            }
        }

        private void OpenFile(string path)
        {
            // UTF-8 with a byte order mark: without one, sqlcmd reads the file as ANSI and garbles non-ASCII text
            // (e.g. "–" becomes "â€“"), which can also make values too long for their columns.
            UTF8Encoding encoding = new(encoderShouldEmitUTF8Identifier: true);
            writer = new StreamWriter(path, append: false, encoding);
            currentFileSize = encoding.GetPreamble().Length;
        }

        private void WriteDirect(string text)
        {
            writer.Write(text);
            currentFileSize += Encoding.UTF8.GetByteCount(text);
        }

        /// <summary>
        /// "C:\dir\name.sql" becomes "C:\dir\name-001.sql".
        /// </summary>
        private string NumberedFilePath(int number)
        {
            string directory = Path.GetDirectoryName(Path.GetFullPath(filePath));
            string name = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);
            return Path.Combine(directory, name + "-" + number.ToString("000") + extension);
        }

        /// <summary>
        /// Read lines including their line endings, so text (e.g. LF-only line endings in procedures) is copied unchanged.
        /// </summary>
        private static IEnumerable<string> ReadLinesWithEndings(StreamReader reader)
        {
            StringBuilder line = new();
            int c;
            while ((c = reader.Read()) != -1)
            {
                line.Append((char) c);
                if (c == '\n')
                {
                    yield return line.ToString();
                    line.Clear();
                }
            }

            if (line.Length > 0)
                yield return line.ToString();
        }
    }
}
