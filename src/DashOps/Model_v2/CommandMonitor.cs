using static Mastersign.DashOps.Model_v2.Helper;

namespace Mastersign.DashOps.Model_v2;

partial class CommandMonitor
{
    public MatchableMonitor CreateMatchable()
    {
        return new MatchableMonitor
        {
            Title = Title,
            Tags = Tags ?? [],
            Variables = [],
            Command = ExpandEnv(Command),
        };
    }

    private static readonly Dictionary<string, string> NO_VARIABLES = new(0);

    public CommandMonitorView CreateView(
        IReadOnlyList<AutoMonitorSettings> autoSettings,
        DefaultMonitorSettings monitorDefaults,
        DefaultSettings commonDefaults)
    {
        var view = new CommandMonitorView
        {
            Tags = Unite([Tags]),

            Command = ExpandEnv(Command),
            Arguments = FormatArguments(
                Coalesce([
                    Arguments,
                    ..autoSettings.Select(s => s.Arguments),
                    monitorDefaults?.Arguments,
                    commonDefaults.Arguments,
                ])?
                    .Select(ExpandEnv)),
        };
        view.UpdateWith(this, autoSettings, monitorDefaults, commonDefaults, NO_VARIABLES);
        view.UpdateStatusFromLogFile();
        return view;
    }
}
