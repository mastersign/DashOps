using static Mastersign.DashOps.Model_v2.Helper;

namespace Mastersign.DashOps.Model_v2;

partial class CommandMonitor
{
    public MatchableMonitor CreateMatchable()
    {
        return new MatchableMonitor
        {
            Title = Title,
            Command = ExpandEnv(Command),
            Variables = [],
            Tags = Tags ?? [],
        };
    }

    private static readonly Dictionary<string, string> NO_VARIABLES = new(0);

    public CommandMonitorView CreateView(DefaultMonitorSettings defaults, IReadOnlyList<AutoMonitorSettings> autoSettings)
    {
        var view = new CommandMonitorView
        {
            Tags = Unite([Tags]),

            Command = ExpandEnv(Command),
            Arguments = FormatArguments(Arguments?.Select(ExpandEnv)),
        };
        view.UpdateWith(this, autoSettings, defaults, NO_VARIABLES);
        view.UpdateStatusFromLogFile();
        return view;
    }
}
