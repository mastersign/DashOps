using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

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

        public string ExpandedWorkingDirectory => Environment.ExpandEnvironmentVariables(WorkingDirectory);

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

        public string CreatePreliminaryLogName() 
            => LogFileManager.PreliminaryLogFileName(this);

        public string FinalizeLogName(string preliminaryLogName, int exitCode)
            => LogFileManager.FinalizeLogFileName(preliminaryLogName, exitCode);

        public IEnumerable<string> LogFiles()
            => LogFileManager.FindLogFiles(this, Logs);

        public string LastLogFile
            => LogFileManager.FindLogFiles(this, Logs)
                .OrderByDescending(f => f)
                .FirstOrDefault();

        public LogFileInfo LastLogFileInfo()
        {
            var lastLogFile = LastLogFile;
            return lastLogFile != null
                ? LogFileManager.GetInfo(lastLogFile)
                : null;
        }

        public ControlTemplate LogIcon
        {
            get
            {
                var logInfo = LastLogFileInfo();
                var resourceName =
                    logInfo != null
                        ? logInfo.HasExitCode
                            ? logInfo.IsSuccess ? "IconLogOK" : "IconLogError"
                            : "IconLog"
                        : "IconLogEmpty";
                return resourceName != null
                    ? App.Current.FindResource(resourceName) as ControlTemplate
                    : null;
            }
        }

        public void NotifyLogChange()
        {
            OnPropertyChanged(nameof(LogIcon));
            LogIconChanged?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler LogIconChanged;

    }
}
