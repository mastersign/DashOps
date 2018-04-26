using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Mastersign.DashOps
{
    partial class ActionView
    {
        private static readonly MD5 Md5 = MD5.Create();

        public bool HasFacette(string name)
            => Facettes?.ContainsKey(name) ?? false;

        public string[] GetFacettes()
            => Facettes?.Keys.ToArray() ?? Array.Empty<string>();

        public bool HasFacetteValue(string name, string value)
            => string.Equals(GetFacetteValue(name), value);

        public string GetFacetteValue(string name)
            => Facettes != null
                ? Facettes.TryGetValue(name, out var value) ? value : null
                : null;

        public string CommandLabel => ExpandedCommand
            + (string.IsNullOrWhiteSpace(ExpandedArguments)
                ? string.Empty
                : " " + ExpandedArguments);

        public string ExpandedCommand => Environment.ExpandEnvironmentVariables(Command);

        public string ExpandedArguments 
            => CommandLine.FormatArgumentList(Arguments.Select(Environment.ExpandEnvironmentVariables).ToArray());

        public string ActionId
        {
            get
            {
                var str = Command + " " + CommandLine.FormatArgumentList(Arguments);
                var data = Encoding.UTF8.GetBytes(str);
                var hash = Md5.ComputeHash(data);
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }

        public override string ToString() => $"[{ActionId}] {Description}: {CommandLabel}";

        public string LogNamePattern => $"*_{ActionId}.log";

        public string CreateLogName() => $"{DateTime.Now:yyyyMMdd_HHmmss}_{ActionId}.log";

        private IEnumerable<string> LogFilePaths(string logDirectory)
            => logDirectory != null && System.IO.Directory.Exists(logDirectory)
                ? System.IO.Directory.EnumerateFiles(logDirectory, LogNamePattern).OrderByDescending(n => n)
                : Enumerable.Empty<string>();

        public string[] LogFiles(string logDirectory) => LogFilePaths(logDirectory).ToArray();

        public string LastLogFile(string logDirectory) => LogFilePaths(logDirectory).FirstOrDefault();
    }
}
