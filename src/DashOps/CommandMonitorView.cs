using System.IO;
using Mastersign.DashOps.Model_v2;
using static Mastersign.DashOps.Model_v2.Helper;

namespace Mastersign.DashOps
{
    partial class CommandMonitorView : IExecutable
    {
        public bool Visible => false;

        public bool KeepOpen => false;

        public bool AlwaysClose => false;

        private string _commandId;

        public override string CommandId
            => _commandId ??= IdBuilder.BuildId(Command + " " + Arguments);

        public string CommandLabel => Command
            + (string.IsNullOrWhiteSpace(Arguments)
                ? string.Empty
                : " " + Arguments);

        public override string ToString() => $"[{CommandId}] {Title}: {CommandLabel}";

        public override async Task<bool> Check(DateTime startTime)
        {
            NotifyMonitorBegin(startTime);
            var result = await App.Instance.Executor.ExecuteAsync(this);
            NotifyMonitorFinished(result.Success);
            return result.Success;
        }

        public Func<string, bool> SuccessCheck
            => output => RequiredPatterns.All(p => p.IsMatch(output)) &&
                         !ForbiddenPatterns.Any(p => p.IsMatch(output));

        protected override void NotifyMonitorFinished(bool success)
        {
            base.NotifyMonitorFinished(success);
            if (!HasExecutionResultChanged && CurrentLogFile != null && File.Exists(CurrentLogFile))
            {
                File.Delete(CurrentLogFile);
                CurrentLogFile = null;
            }
        }

        public void NotifyExecutionFinished()
        {
            // nothing
        }

        public bool PrintExecutionInfo => !NoExecutionInfo;

        public string ExitCodesFormatted => string.Join(", ", ExitCodes ?? []);

        public bool UseWindowsTerminal => false;

        public string WindowsTerminalArguments => null;

        public void UpdateWith(
            CommandMonitor settings, 
            IReadOnlyList<AutoMonitorSettings> autoSettings,
            DefaultMonitorSettings monitorDefaults, 
            DefaultSettings commonDefaults,
            IReadOnlyDictionary<string, string> variables)
        {
            base.UpdateWith(settings, autoSettings, monitorDefaults, commonDefaults, variables);

            WorkingDirectory = BuildAbsolutePath(ExpandEnv(ExpandTemplate(
                Coalesce([
                    settings.WorkingDirectory, 
                    .. autoSettings.Select(s => s.WorkingDirectory),
                    monitorDefaults?.WorkingDirectory,
                    commonDefaults.WorkingDirectory,
                ]),
                variables)));

            Environment = ExpandEnv(ExpandDictionaryTemplate(
                CoalesceValues([
                    settings.Environment, 
                    .. autoSettings.Select(s => s.Environment),
                    monitorDefaults?.Environment,
                    commonDefaults.Environment,
                ]),
                variables));

            ExePaths = Coalesce([
                settings.ExePaths,
                .. autoSettings.Select(s => s.ExePaths), 
                monitorDefaults?.ExePaths,
                commonDefaults.ExePaths,
            ])
                .Select(p => ExpandTemplate(p, variables))
                .Select(ExpandEnv)
                .Select(BuildAbsolutePath)
                .Where(p => !string.IsNullOrWhiteSpace(p))
                .ToArray();

            ExitCodes = Coalesce([
                settings.ExitCodes, 
                .. autoSettings.Select(s => s.ExitCodes), 
                monitorDefaults?.ExitCodes,
                commonDefaults.ExitCodes,
                [0],
            ]);

            UsePowerShellCore =
                Coalesce([
                    settings.UsePowerShellCore, 
                    .. autoSettings.Select(s => s.UsePowerShellCore),
                    monitorDefaults?.UsePowerShellCore,
                    commonDefaults.UsePowerShellCore,
                ]);

            PowerShellExe = BuildAbsolutePath(ExpandEnv(ExpandTemplate(
                CoalesceWhitespace([
                    settings.PowerShellExe, 
                    .. autoSettings.Select(s => s.PowerShellExe), 
                    monitorDefaults?.PowerShellExe,
                    commonDefaults.PowerShellExe,
                ]),
                variables)));

            UsePowerShellProfile =
                Coalesce([
                    settings.UsePowerShellProfile,
                    .. autoSettings.Select(s => s.UsePowerShellProfile),
                    monitorDefaults?.UsePowerShellProfile,
                    commonDefaults.UsePowerShellProfile,
                ]);

            PowerShellExecutionPolicy =
                CoalesceWhitespace([
                    settings.PowerShellExecutionPolicy, 
                    .. autoSettings.Select(s => s.PowerShellExecutionPolicy), 
                    monitorDefaults?.PowerShellExecutionPolicy,
                    commonDefaults.PowerShellExecutionPolicy,
                ]);
        }
    }
}
