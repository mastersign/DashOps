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

        public void UpdateWith(
            CommandActionSettings settings, 
            IReadOnlyCollection<AutoActionSettings> autoSettings,
            DefaultActionSettings actionDefaults, 
            DefaultSettings commonDefaults, 
            IReadOnlyDictionary<string, string> facets)
        {
            Facets = CoalesceValues([facets, .. autoSettings.Select(s => Facets)]);

            Reassure = Coalesce([settings.Reassure, .. autoSettings.Select(s => s.Reassure), actionDefaults?.Reassure]);
            Visible = !Coalesce([settings.Background, .. autoSettings.Select(s => s.Background), actionDefaults?.Background]);
            KeepOpen = Coalesce([settings.KeepOpen, .. autoSettings.Select(s => s.KeepOpen), actionDefaults?.KeepOpen]);
            AlwaysClose = Coalesce([settings.AlwaysClose, .. autoSettings.Select(s => s.AlwaysClose), actionDefaults?.AlwaysClose]);

            NoLogs = Coalesce([
                settings.NoLogs, 
                .. autoSettings.Select(s => s.NoLogs),
                actionDefaults?.NoLogs, 
                commonDefaults.NoLogs,
            ]);

            Logs = NoLogs ? null : BuildAbsolutePath(ExpandEnv(ExpandTemplate(
                Coalesce([
                    settings.Logs, 
                    .. autoSettings.Select(s => s.Logs), 
                    actionDefaults?.Logs, 
                    commonDefaults.Logs,
                ]),
                facets)));

            NoExecutionInfo = Coalesce([
                settings.NoExecutionInfo, 
                .. autoSettings.Select(s => s.NoExecutionInfo),
                actionDefaults?.NoExecutionInfo,
                commonDefaults.NoExecutionInfo,
            ]);

            WorkingDirectory = BuildAbsolutePath(ExpandEnv(ExpandTemplate(
                Coalesce([
                    settings.WorkingDirectory,
                    .. autoSettings.Select(s => s.WorkingDirectory),
                    actionDefaults?.WorkingDirectory,
                    commonDefaults.WorkingDirectory,
                ]),
                facets)));

            Environment = ExpandEnv(ExpandDictionaryTemplate(
                CoalesceValues([
                    settings.Environment, 
                    .. autoSettings.Select(s => s.Environment), 
                    actionDefaults?.Environment,
                    commonDefaults.Environment,
                ]),
                facets));

            ExePaths = Coalesce([
                settings.ExePaths, 
                .. autoSettings.Select(s => s.ExePaths),
                actionDefaults?.ExePaths, 
                commonDefaults.ExePaths,
            ])
                .Select(p => ExpandTemplate(p, facets))
                .Select(ExpandEnv)
                .Select(BuildAbsolutePath)
                .Where(p => !string.IsNullOrWhiteSpace(p))
                .ToArray();

            ExitCodes = Coalesce([
                settings.ExitCodes, 
                .. autoSettings.Select(s => s.ExitCodes),
                actionDefaults?.ExitCodes,
                commonDefaults.ExitCodes,
                [0],
            ]);

            UsePowerShellCore =
                Coalesce([
                    settings.UsePowerShellCore, 
                    .. autoSettings.Select(s => s.UsePowerShellCore),
                    actionDefaults?.UsePowerShellCore, 
                    commonDefaults.UsePowerShellCore,
                ]);

            PowerShellExe = BuildAbsolutePath(ExpandEnv(ExpandTemplate(
                CoalesceWhitespace([
                    settings.PowerShellExe, 
                    .. autoSettings.Select(s => s.PowerShellExe),
                    actionDefaults?.PowerShellExe,
                    commonDefaults.PowerShellExe,
                ]),
                facets)));

            UsePowerShellProfile =
                Coalesce([
                    settings.UsePowerShellProfile,
                    .. autoSettings.Select(s => s.UsePowerShellProfile),
                    actionDefaults?.UsePowerShellProfile,
                    commonDefaults.UsePowerShellProfile,
                ]);

            PowerShellExecutionPolicy =
                CoalesceWhitespace([
                    settings.PowerShellExecutionPolicy, 
                    .. autoSettings.Select(s => s.PowerShellExecutionPolicy),
                    actionDefaults?.PowerShellExecutionPolicy,
                    commonDefaults.PowerShellExecutionPolicy,
                ]);

            UseWindowsTerminal =
                Coalesce([
                    settings.UseWindowsTerminal, 
                    .. autoSettings.Select(s => s.UseWindowsTerminal),
                    actionDefaults?.UseWindowsTerminal,
                ]);

            WindowsTerminalArguments = FormatArguments(
                Coalesce([
                    settings.WindowsTerminalArgs, 
                    .. autoSettings.Select(s => s.WindowsTerminalArgs), 
                    actionDefaults?.WindowsTerminalArgs,
                ])
                    .Select(a => ExpandTemplate(a, facets))
                    .Select(ExpandEnv));
        }

    }
}
