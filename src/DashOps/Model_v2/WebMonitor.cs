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

    public WebMonitorView CreateView(DefaultMonitorSettings defaults, IReadOnlyList<AutoMonitorSettings> autoSettings)
    {
        var monitorView = new WebMonitorView
        {
            Tags = Unite([Tags]),
        };
        monitorView.UpdateWith(this, autoSettings, defaults, NO_VARIABLES);
        monitorView.UpdateStatusFromLogFile();
        return monitorView;
    }
}
