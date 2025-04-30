namespace Mastersign.DashOps.Model_v2;

partial class AutoActionSettings
{
    public bool Match(MatchableAction action)
    {
        if (Include != null && Include.Any(m => !m.Match(action))) return false;
        if (Exclude != null && Exclude.Any(m => m.Match(action))) return false;
        return true;
    }
}
