using static Mastersign.DashOps.Model_v2.Helper;

namespace Mastersign.DashOps.Model_v2;

partial class CommandActionPattern
{
    public MatchableAction CreateMatchable(IDictionary<string, string> instanceFacets)
    {
        var tags = Tags ?? [];
        var facets = CoalesceValues([instanceFacets]);

        return new MatchableAction
        {
            Title = ExpandTemplate(Title, facets),
            Command = ExpandEnv(ExpandTemplate(Command, facets)),
            Tags = tags,
            Facets = facets,
        };
    }

    public ActionView CreateView(DefaultActionSettings defaults, IReadOnlyList<AutoActionSettings> autoSettings, IDictionary<string, string> instanceFacets)
    {
        var stableFacets = instanceFacets;

        var tags = Unite([Tags, .. autoSettings.Select(s => s.Tags)]);
        var facets = CoalesceValues([stableFacets, .. autoSettings.Select(s => s.Facets)]);

        var noLogs = Coalesce([NoLogs, .. autoSettings.Select(s => s.NoLogs), defaults.NoLogs]);

        var actionView = new ActionView
        {
            Title = ExpandTemplate(Title, stableFacets),

            Reassure = Coalesce([Reassure, .. autoSettings.Select(s => s.Reassure), defaults.Reassure]),
            Visible = !Coalesce([Background, .. autoSettings.Select(s => s.Background), defaults.Background]),
            KeepOpen = Coalesce([KeepOpen, .. autoSettings.Select(s => s.KeepOpen), defaults.KeepOpen]),
            AlwaysClose = Coalesce([AlwaysClose, .. autoSettings.Select(s => s.AlwaysClose), defaults.AlwaysClose]),

            Logs = noLogs ? null : BuildAbsolutePath(ExpandEnv(ExpandTemplate(
                Coalesce([Logs, .. autoSettings.Select(s => s.Logs), defaults.Logs]),
                stableFacets))),
            NoLogs = noLogs,
            NoExecutionInfo = Coalesce([NoExecutionInfo, .. autoSettings.Select(s => s.NoExecutionInfo), defaults.NoExecutionInfo]),

            Command = ExpandEnv(ExpandTemplate(Command, stableFacets)),
            Arguments = FormatArguments(Arguments?
                .Select(a => ExpandTemplate(a, stableFacets))
                .Select(ExpandEnv)),

            WorkingDirectory = BuildAbsolutePath(ExpandEnv(ExpandTemplate(
                Coalesce([WorkingDirectory, .. autoSettings.Select(s => s.WorkingDirectory), defaults.WorkingDirectory]),
                stableFacets))),
            Environment = ExpandEnv(
                ExpandDictionaryTemplate(
                    CoalesceValues([Environment, .. autoSettings.Select(s => s.Environment), defaults.Environment]),
                    stableFacets)),
            ExePaths = Coalesce([ExePaths, .. autoSettings.Select(s => s.ExePaths), defaults.ExePaths])
                .Select(p => ExpandTemplate(p, stableFacets))
                .Select(ExpandEnv)
                .Select(BuildAbsolutePath)
                .Where(p => !string.IsNullOrWhiteSpace(p))
                .ToArray(),
            ExitCodes = Coalesce([ExitCodes, .. autoSettings.Select(s => s.ExitCodes), defaults.ExitCodes, [0]]),

            UsePowerShellCore =
                Coalesce([UsePowerShellCore, .. autoSettings.Select(s => s.UsePowerShellCore), defaults.UsePowerShellCore]),
            PowerShellExe = BuildAbsolutePath(ExpandEnv(ExpandTemplate(
                CoalesceWhitespace([PowerShellExe, .. autoSettings.Select(s => s.PowerShellExe), defaults.PowerShellExe]),
                stableFacets))),
            UsePowerShellProfile =
                Coalesce([UsePowerShellProfile, .. autoSettings.Select(s => s.UsePowerShellProfile), defaults.UsePowerShellProfile]),
            PowerShellExecutionPolicy =
                CoalesceWhitespace([PowerShellExecutionPolicy, .. autoSettings.Select(s => s.PowerShellExecutionPolicy), defaults.PowerShellExecutionPolicy]),

            UseWindowsTerminal =
                Coalesce([UseWindowsTerminal, .. autoSettings.Select(s => s.UseWindowsTerminal), defaults.UseWindowsTerminal]),
            WindowsTerminalArguments = FormatArguments(
                Coalesce([WindowsTerminalArgs, .. autoSettings.Select(s => s.WindowsTerminalArgs), defaults.WindowsTerminalArgs])
                    .Select(a => ExpandTemplate(a, stableFacets))
                    .Select(ExpandEnv)),

            Tags = tags,
            Facets = facets,
        };

        actionView.UpdateStatusFromLogFile();

        return actionView;
    }
}
