namespace Mastersign.DashOps.Model_v2;

partial class AutoMonitorSettings
{
    public bool Match(MatchableMonitor matchable)
    {
        if (Include != null && Include.Any(m => !m.Match(matchable))) return false;
        if (Exclude != null && Exclude.Any(m => m.Match(matchable))) return false;
        return true;
    }
}
