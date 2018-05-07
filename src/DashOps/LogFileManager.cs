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

        public static string LogNamePattern(ILogged logged)
            => $"*_{logged.CommandId}_*.log";

        public static string PreliminaryLogFileName(this ILogged executable, DateTime timestamp)
            => timestamp.ToString(TS_FORMAT, CultureInfo.InvariantCulture) + "_" + executable.CommandId;

        public static string FinalizeLogFileName(string preliminaryLogFileName, int exitCode)
            => $"{preliminaryLogFileName}_{exitCode}.log";

        public static IEnumerable<string> FindLogFiles(this ILogged logged)
            => logged.Logs != null && Directory.Exists(logged.Logs)
                ? Directory.EnumerateFiles(logged.Logs, LogNamePattern(logged))
                : Enumerable.Empty<string>();

        public static bool HasLogFile(this ILogged logged)
            => logged.FindLogFiles().Any();

        public static string FindLastLogFile(this ILogged logged)
            => logged.FindLogFiles().OrderByDescending(f => f).FirstOrDefault();

        private static readonly Regex NamePattern = new Regex(@"^(?<ts>.{" + TS_FORMAT.Length + @"})_(?<aid>[^_]+)(?:_(?<ec>-?\d+))?\.log$");

        public static LogFileInfo GetInfo(string logFile)
        {
            if (string.IsNullOrWhiteSpace(logFile)) return null;
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

        public static LogFileInfo GetLastLogFileInfo(this ILogged logged)
            => GetInfo(logged.FindLastLogFile());

        public static void PostprocessLogFile(string srcName, string trgName, StringBuilder outputBuffer)
        {
            var sSrc = File.Open(srcName, FileMode.Open, FileAccess.Read, FileShare.Read);
            var sTrg = File.Open(trgName, FileMode.Create, FileAccess.Write, FileShare.None);
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
                        outputBuffer?.AppendLine(line);
                    }
                    line = rSrc.ReadLine();
                }
            }
        }
    }
}
