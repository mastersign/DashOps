using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Mastersign.DashOps
{
    static class LogFileManager
    {
        private const string TS_FORMAT = "yyyyMMdd_HHmmss";

        public static string LogNamePattern(IExecutable executable)
            => $"*_{executable.CommandId}_*.log";

        public static string PreliminaryLogFileName(IExecutable executable, DateTime timestamp)
            => timestamp.ToString(TS_FORMAT, CultureInfo.InvariantCulture) + "_" + executable.CommandId;

        public static string FinalizeLogFileName(string preliminaryLogFileName, int exitCode)
            => $"{preliminaryLogFileName}_{exitCode}.log";

        public static IEnumerable<string> FindLogFiles(IExecutable executable, string logDirectory)
            => logDirectory != null && Directory.Exists(logDirectory)
                ? System.IO.Directory.EnumerateFiles(logDirectory, LogNamePattern(executable))
                : Enumerable.Empty<string>();

        private readonly static Regex NamePattern = new Regex(@"^(?<ts>.{" + TS_FORMAT.Length + @"})_(?<aid>[^_]+)(?:_(?<ec>-?\d+))?\.log$");

        public static LogFileInfo GetInfo(string logFile)
        {
            var fileName = Path.GetFileName(logFile);
            var m = NamePattern.Match(fileName);
            if (!m.Success) return null;
            return new LogFileInfo(
                logFile,
                DateTime.ParseExact(m.Groups["ts"].Value, TS_FORMAT, CultureInfo.InvariantCulture),
                m.Groups["aid"].Value,
                m.Groups["ec"].Success,
                m.Groups["ec"].Success ? int.Parse(m.Groups["ec"].Value, CultureInfo.InvariantCulture) : 0);
        }

        private static readonly Regex TranscriptBeginPattern = new Regex(@"^(?<pre>#+\s+)BEGIN(?<post>\s+TRANSCRIPT\s+.+?\s+#+)$");

        public static void PostprocessLogFile(string fileName)
        {
            if (!File.Exists(fileName)) return;
            var tmpName = fileName + ".tmp";
            using (var sSrc = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var sTrg = File.Open(tmpName, FileMode.Create, FileAccess.Write, FileShare.None))
            using (var rSrc = new StreamReader(sSrc, Encoding.UTF8))
            using (var wTrg = new StreamWriter(sTrg, Encoding.UTF8))
            {
                var firstLine = rSrc.ReadLine(); // assuming the first line is the separator of the powershell transcript
                var line = firstLine;
                var separatorCount = 0;
                while (line != null)
                {
                    if (string.Equals(line, firstLine))
                    {
                        separatorCount++;
                    }
                    else if (separatorCount == 2)
                    {
                        wTrg.WriteLine(line);
                    }
                    line = rSrc.ReadLine();
                }
            }
            File.Delete(fileName);
            File.Move(tmpName, fileName);
        }
    }
}
