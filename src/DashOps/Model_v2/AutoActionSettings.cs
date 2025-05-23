﻿namespace Mastersign.DashOps.Model_v2;

partial class AutoActionSettings
{
    public bool Match(MatchableAction matchable)
    {
        if (Include != null && Include.Any(m => !m.Match(matchable))) return false;
        if (Exclude != null && Exclude.Any(m => m.Match(matchable))) return false;
        return true;
    }
}
