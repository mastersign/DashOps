using static Mastersign.DashOps.Model_v2.Helper;

namespace Mastersign.DashOps.Model_v2;

partial class WebMonitorPattern
{
    public MatchableMonitor CreateMatchable(IReadOnlyDictionary<string, string> instanceVariables)
    {
        var variables = CoalesceValues([instanceVariables]);
        return new MatchableMonitor
        {
            Title = ExpandTemplate(Title, variables),
            Url = ExpandTemplate(Url, variables),
            Headers = ExpandDictionaryTemplate(Headers, variables),
            Variables = variables,
            Tags = Tags ?? [],
        };
    }

    public WebMonitorView CreateView(DefaultMonitorSettings defaults, IReadOnlyList<AutoMonitorSettings> autoSettings, IReadOnlyDictionary<string, string> instanceVariables)
    {
        var view = new WebMonitorView
        {
            Tags = Unite([Tags]),
        };
        view.UpdateWith(this, autoSettings, defaults, instanceVariables);
        view.UpdateStatusFromLogFile();
        return view;
    }
}
