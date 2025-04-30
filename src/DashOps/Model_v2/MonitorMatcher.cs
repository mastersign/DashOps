namespace Mastersign.DashOps.Model_v2;

partial class MonitorMatcher
{
    public bool Match(MatchableMonitor matchable) => Mode switch
    {
        MonitorMatchMode.Title => MatchString(matchable.Title),
        MonitorMatchMode.Command => MatchString(matchable.Command),
        MonitorMatchMode.Url => MatchString(matchable.Url),
        MonitorMatchMode.Header => MatchString(matchable.GetHeaderValue(Header)),
        MonitorMatchMode.Variable => MatchString(matchable.GetVariableValue(Variable)),
        MonitorMatchMode.Tag => matchable.Tags.Any(MatchString),
        _ => throw new ArgumentOutOfRangeException(),
    };
}
