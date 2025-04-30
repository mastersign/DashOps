using Mastersign.DashOps.Model_v2;
using static Mastersign.DashOps.Model_v2.Helper;

namespace Mastersign.DashOps
{
    partial class ActionView : IExecutable, ILogged
    {
        public bool HasFacet(string name)
            => Facets?.ContainsKey(name) ?? false;

        public string[] GetFacets()
            => Facets?.Keys.ToArray() ?? Array.Empty<string>();

        public bool HasFacetValue(string name, string value)
            => string.Equals(GetFacetValue(name), value);

        public string GetFacetValue(string name)
            => Facets != null
                ? Facets.TryGetValue(name, out var value) ? value : null
                : null;

        public string CommandLabel => Command
            + (string.IsNullOrWhiteSpace(Arguments)
                ? string.Empty
                : " " + Arguments);

        private string _commandId;

        public string CommandId
            => _commandId ??= IdBuilder.BuildId(Command + " " + Arguments);

        public override string ToString() => $"[{CommandId}] {Title}: {CommandLabel}";

        public void NotifyExecutionFinished()
        {
            // nothing for now
        }

        public bool PrintExecutionInfo => !NoExecutionInfo;

        public Task<ExecutionResult> ExecuteAsync() => App.Instance.Executor.ExecuteAsync(this);

        public Func<string, bool> SuccessCheck => null;

        public string ExitCodesFormatted => string.Join(", ", ExitCodes);

        public void UpdateStatusFromLogFile()
        {
            var logInfo = LogFileManager.GetLastLogFileInfo(this);
            if (logInfo != null)
            {
                Status = logInfo.Success ? ActionStatus.Success : ActionStatus.Failed;
            }
            else
            {
                Status = ActionStatus.Unknown;
            }
        }

        public void UpdateWith(CommandActionSettings settings, IReadOnlyCollection<AutoActionSettings> autoSettings, DefaultActionSettings defaults, IReadOnlyDictionary<string, string> facets)
        {
            Reassure = Coalesce([settings.Reassure, .. autoSettings.Select(s => s.Reassure), defaults.Reassure]);
            Visible = !Coalesce([settings.Background, .. autoSettings.Select(s => s.Background), defaults.Background]);
            KeepOpen = Coalesce([settings.KeepOpen, .. autoSettings.Select(s => s.KeepOpen), defaults.KeepOpen]);
            AlwaysClose = Coalesce([settings.AlwaysClose, .. autoSettings.Select(s => s.AlwaysClose), defaults.AlwaysClose]);

            NoLogs = Coalesce([settings.NoLogs, .. autoSettings.Select(s => s.NoLogs), defaults.NoLogs]);
            Logs = NoLogs ? null : BuildAbsolutePath(ExpandEnv(ExpandTemplate(
                Coalesce([settings.Logs, .. autoSettings.Select(s => s.Logs), defaults.Logs]),
                facets)));
            NoExecutionInfo = Coalesce([settings.NoExecutionInfo, .. autoSettings.Select(s => s.NoExecutionInfo), defaults.NoExecutionInfo]);

            WorkingDirectory = BuildAbsolutePath(ExpandEnv(ExpandTemplate(
                Coalesce([settings.WorkingDirectory, .. autoSettings.Select(s => s.WorkingDirectory), defaults.WorkingDirectory]),
                facets)));
            Environment = ExpandEnv(ExpandDictionaryTemplate(
                CoalesceValues([settings.Environment, .. autoSettings.Select(s => s.Environment), defaults.Environment]),
                facets));
            ExePaths = Coalesce([settings.ExePaths, .. autoSettings.Select(s => s.ExePaths), defaults.ExePaths])
                .Select(p => ExpandTemplate(p, facets))
                .Select(ExpandEnv)
                .Select(BuildAbsolutePath)
                .Where(p => !string.IsNullOrWhiteSpace(p))
                .ToArray();
            ExitCodes = Coalesce([settings.ExitCodes, .. autoSettings.Select(s => s.ExitCodes), defaults.ExitCodes, [0]]);

            UsePowerShellCore =
                Coalesce([settings.UsePowerShellCore, .. autoSettings.Select(s => s.UsePowerShellCore), defaults.UsePowerShellCore]);
            PowerShellExe = BuildAbsolutePath(ExpandEnv(ExpandTemplate(
                CoalesceWhitespace([settings.PowerShellExe, .. autoSettings.Select(s => s.PowerShellExe), defaults.PowerShellExe]),
                facets)));
            UsePowerShellProfile =
                Coalesce([settings.UsePowerShellProfile, .. autoSettings.Select(s => s.UsePowerShellProfile), defaults.UsePowerShellProfile]);
            PowerShellExecutionPolicy =
                CoalesceWhitespace([settings.PowerShellExecutionPolicy, .. autoSettings.Select(s => s.PowerShellExecutionPolicy), defaults.PowerShellExecutionPolicy]);

            UseWindowsTerminal =
                Coalesce([settings.UseWindowsTerminal, .. autoSettings.Select(s => s.UseWindowsTerminal), defaults.UseWindowsTerminal]);
            WindowsTerminalArguments = FormatArguments(
                Coalesce([settings.WindowsTerminalArgs, .. autoSettings.Select(s => s.WindowsTerminalArgs), defaults.WindowsTerminalArgs])
                    .Select(a => ExpandTemplate(a, facets))
                    .Select(ExpandEnv));

            Tags = Unite([Tags, .. autoSettings.Select(s => s.Tags)]);
            Facets = CoalesceValues([facets, .. autoSettings.Select(s => Facets)]);
        }

    }
}
