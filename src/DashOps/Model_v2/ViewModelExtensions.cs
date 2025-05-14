using Mastersign.DashOps.ViewModel;
using static Mastersign.DashOps.Model_v2.Helper;

namespace Mastersign.DashOps.Model_v2;

internal static class ViewModelExtensions
{
    public static void UpdateWith(this ActionView view,
        CommandActionSettings settings,
        IReadOnlyCollection<AutoActionSettings> autoSettings,
        DefaultActionSettings actionDefaults,
        DefaultSettings commonDefaults,
        IReadOnlyDictionary<string, string> facets)
    {
        view.Facets = CoalesceValues([facets, .. autoSettings.Select(s => view.Facets)]);

        view.Reassure = Coalesce([settings.Reassure, .. autoSettings.Select(s => s.Reassure), actionDefaults?.Reassure]);
        view.Visible = !Coalesce([settings.Background, .. autoSettings.Select(s => s.Background), actionDefaults?.Background]);
        view.KeepOpen = Coalesce([settings.KeepOpen, .. autoSettings.Select(s => s.KeepOpen), actionDefaults?.KeepOpen]);
        view.AlwaysClose = Coalesce([settings.AlwaysClose, .. autoSettings.Select(s => s.AlwaysClose), actionDefaults?.AlwaysClose]);

        view.NoLogs = Coalesce([
            settings.NoLogs,
            .. autoSettings.Select(s => s.NoLogs),
            actionDefaults?.NoLogs,
            commonDefaults.NoLogs,
        ]);

        view.Logs = view.NoLogs ? null : BuildAbsolutePath(ExpandEnv(ExpandTemplate(
            Coalesce([
                settings.Logs,
                .. autoSettings.Select(s => s.Logs),
                actionDefaults?.Logs,
                commonDefaults.Logs,
            ]),
            facets)));

        view.NoExecutionInfo = Coalesce([
            settings.NoExecutionInfo,
            .. autoSettings.Select(s => s.NoExecutionInfo),
            actionDefaults?.NoExecutionInfo,
            commonDefaults.NoExecutionInfo,
        ]);

        view.WorkingDirectory = BuildAbsolutePath(ExpandEnv(ExpandTemplate(
            Coalesce([
                settings.WorkingDirectory,
                .. autoSettings.Select(s => s.WorkingDirectory),
                actionDefaults?.WorkingDirectory,
                commonDefaults.WorkingDirectory,
            ]),
            facets)));

        view.Environment = ExpandEnv(ExpandDictionaryTemplate(
            CoalesceValues([
                settings.Environment,
                .. autoSettings.Select(s => s.Environment),
                actionDefaults?.Environment,
                commonDefaults.Environment,
            ]),
            facets));

        view.ExePaths = Coalesce([
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

        view.ExitCodes = Coalesce([
            settings.ExitCodes,
            .. autoSettings.Select(s => s.ExitCodes),
            actionDefaults?.ExitCodes,
            commonDefaults.ExitCodes,
            [0],
        ]);

        view.UsePowerShellCore =
            Coalesce([
                settings.UsePowerShellCore,
                .. autoSettings.Select(s => s.UsePowerShellCore),
                actionDefaults?.UsePowerShellCore,
                commonDefaults.UsePowerShellCore,
            ]);

        view.PowerShellExe = BuildAbsolutePath(ExpandEnv(ExpandTemplate(
            CoalesceWhitespace([
                settings.PowerShellExe,
                .. autoSettings.Select(s => s.PowerShellExe),
                actionDefaults?.PowerShellExe,
                commonDefaults.PowerShellExe,
            ]),
            facets)));

        view.UsePowerShellProfile =
            Coalesce([
                settings.UsePowerShellProfile,
                .. autoSettings.Select(s => s.UsePowerShellProfile),
                actionDefaults?.UsePowerShellProfile,
                commonDefaults.UsePowerShellProfile,
            ]);

        view.PowerShellExecutionPolicy =
            CoalesceWhitespace([
                settings.PowerShellExecutionPolicy,
                .. autoSettings.Select(s => s.PowerShellExecutionPolicy),
                actionDefaults?.PowerShellExecutionPolicy,
                commonDefaults.PowerShellExecutionPolicy,
            ]);

        view.UseWindowsTerminal =
            Coalesce([
                settings.UseWindowsTerminal,
                .. autoSettings.Select(s => s.UseWindowsTerminal),
                actionDefaults?.UseWindowsTerminal,
            ]);

        view.WindowsTerminalArguments = FormatArguments(
            Coalesce([
                settings.WindowsTerminalArgs,
                .. autoSettings.Select(s => s.WindowsTerminalArgs),
                actionDefaults?.WindowsTerminalArgs,
            ])
                .Select(a => ExpandTemplate(a, facets))
                .Select(ExpandEnv));
    }

    public static void UpdateWith(
        this MonitorView view,
        MonitorBase settings,
        IReadOnlyList<AutoMonitorSettings> autoSettings,
        DefaultMonitorSettings monitorDefaults,
        DefaultSettings commonDefaults,
        IReadOnlyDictionary<string, string> variables)
    {
        view.Title = ExpandTemplate(settings.Title, variables);

        view.Interval = TimeSpan.FromSeconds(
            Coalesce([
                settings.Interval,
                    .. autoSettings.Select(s => s.Interval),
                    monitorDefaults?.Interval,
            ]));

        view.Deactivated = Coalesce([
            settings.Deactivated,
                ..autoSettings.Select(s => s.Deactivated),
                monitorDefaults?.Deactivated,
            ]);

        view.NoLogs = Coalesce([
            settings.NoLogs,
                .. autoSettings.Select(s => s.NoLogs),
                monitorDefaults?.NoLogs,
                commonDefaults.NoLogs,
            ]);

        view.Logs = view.NoLogs ? null : BuildAbsolutePath(ExpandEnv(ExpandTemplate(
            Coalesce([
                settings.Logs,
                    .. autoSettings.Select(s => s.Logs),
                    monitorDefaults?.Logs,
                    commonDefaults.Logs,
            ]),
            variables)));

        view.NoExecutionInfo = Coalesce([
            settings.NoExecutionInfo,
                .. autoSettings.Select(s => s.NoExecutionInfo),
                monitorDefaults?.NoExecutionInfo,
                commonDefaults.NoExecutionInfo,
            ]);

        view.RequiredPatterns = BuildTextPatterns(
            Coalesce([
                settings.RequiredPatterns,
                    .. autoSettings.Select(s => s.RequiredPatterns),
                    monitorDefaults?.RequiredPatterns,
            ]));

        view.ForbiddenPatterns = BuildTextPatterns(
            Coalesce([
                settings.ForbiddenPatterns,
                    .. autoSettings.Select(s => s.ForbiddenPatterns),
                    monitorDefaults?.ForbiddenPatterns,
            ]));
    }

    public static void UpdateWith(
        this CommandMonitorView view,
        CommandMonitor settings,
        IReadOnlyList<AutoMonitorSettings> autoSettings,
        DefaultMonitorSettings monitorDefaults,
        DefaultSettings commonDefaults,
        IReadOnlyDictionary<string, string> variables)
    {
        UpdateWith((MonitorView)view, settings, autoSettings, monitorDefaults, commonDefaults, variables);

        view.WorkingDirectory = BuildAbsolutePath(ExpandEnv(ExpandTemplate(
            Coalesce([
                settings.WorkingDirectory,
                    .. autoSettings.Select(s => s.WorkingDirectory),
                    monitorDefaults?.WorkingDirectory,
                    commonDefaults.WorkingDirectory,
            ]),
            variables)));

        view.Environment = ExpandEnv(ExpandDictionaryTemplate(
            CoalesceValues([
                settings.Environment,
                    .. autoSettings.Select(s => s.Environment),
                    monitorDefaults?.Environment,
                    commonDefaults.Environment,
            ]),
            variables));

        view.ExePaths = Coalesce([
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

        view.ExitCodes = Coalesce([
            settings.ExitCodes,
                .. autoSettings.Select(s => s.ExitCodes),
                monitorDefaults?.ExitCodes,
                commonDefaults.ExitCodes,
                [0],
            ]);

        view.UsePowerShellCore =
            Coalesce([
                settings.UsePowerShellCore,
                    .. autoSettings.Select(s => s.UsePowerShellCore),
                    monitorDefaults?.UsePowerShellCore,
                    commonDefaults.UsePowerShellCore,
            ]);

        view.PowerShellExe = BuildAbsolutePath(ExpandEnv(ExpandTemplate(
            CoalesceWhitespace([
                settings.PowerShellExe,
                    .. autoSettings.Select(s => s.PowerShellExe),
                    monitorDefaults?.PowerShellExe,
                    commonDefaults.PowerShellExe,
            ]),
            variables)));

        view.UsePowerShellProfile =
            Coalesce([
                settings.UsePowerShellProfile,
                    .. autoSettings.Select(s => s.UsePowerShellProfile),
                    monitorDefaults?.UsePowerShellProfile,
                    commonDefaults.UsePowerShellProfile,
            ]);

        view.PowerShellExecutionPolicy =
            CoalesceWhitespace([
                settings.PowerShellExecutionPolicy,
                    .. autoSettings.Select(s => s.PowerShellExecutionPolicy),
                    monitorDefaults?.PowerShellExecutionPolicy,
                    commonDefaults.PowerShellExecutionPolicy,
            ]);
    }

    public static void UpdateWith(
        this WebMonitorView view,
        WebMonitor settings,
        IReadOnlyList<AutoMonitorSettings> autoSettings,
        DefaultMonitorSettings monitorDefaults,
        DefaultSettings commonDefaults,
        IReadOnlyDictionary<string, string> variables)
    {
        UpdateWith((MonitorView)view, settings, autoSettings, monitorDefaults, commonDefaults, variables);

        view.Url = ExpandTemplate(settings.Url, variables);

        view.Headers = ExpandDictionaryTemplate(
            CoalesceValues([
                settings.Headers,
                    .. autoSettings.Select(s => s.Headers),
                    monitorDefaults?.Headers,
            ]),
            variables);

        view.Timeout = TimeSpan.FromSeconds(
            Coalesce([
                settings.HttpTimeout,
                    .. autoSettings.Select(s => s.HttpTimeout),
                    monitorDefaults?.HttpTimeout,
            ]));

        view.ServerCertificateHash =
            Coalesce([
                settings.ServerCertificateHash,
                    .. autoSettings.Select(s => s.ServerCertificateHash),
                    monitorDefaults?.ServerCertificateHash,
            ]);

        view.NoTlsVerify = Coalesce([
            settings.NoTlsVerify,
                .. autoSettings.Select(s => s.NoTlsVerify),
                monitorDefaults?.NoTlsVerify,
            ]);

        view.StatusCodes = Coalesce([
            settings.StatusCodes,
                .. autoSettings.Select(s => s.StatusCodes),
                monitorDefaults?.StatusCodes,
                [200, 201, 202, 203, 204]]);
    }

}
