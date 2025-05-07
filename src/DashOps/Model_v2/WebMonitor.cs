using static Mastersign.DashOps.Model_v2.Helper;

namespace Mastersign.DashOps.Model_v2;

partial class WebMonitor
{
    public MatchableMonitor CreateMatchable()
    {
        return new MatchableMonitor
        {
            Title = Title,
            Url = Url,
            Headers = Headers,
            Variables = [],
            Tags = Tags ?? [],
        };
    }

    private static readonly Dictionary<string, string> NO_VARIABLES = new(0);

    public WebMonitorView CreateView(
        IReadOnlyList<AutoMonitorSettings> autoSettings,
        DefaultMonitorSettings monitorDefaults,
        DefaultSettings commonDefaults)
    {
        var monitorView = new WebMonitorView
        {
            Tags = Unite([Tags]),
        };
        monitorView.UpdateWith(this, autoSettings, monitorDefaults, commonDefaults, NO_VARIABLES);
        monitorView.UpdateStatusFromLogFile();
        return monitorView;
    }
}
