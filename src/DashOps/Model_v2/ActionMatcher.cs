namespace Mastersign.DashOps.Model_v2;

partial class ActionMatcher
{
    public bool Match(MatchableAction matchable) => Mode switch
    {
        ActionMatchMode.Title => MatchString(matchable.Title),
        ActionMatchMode.Command => MatchString(matchable.Command),
        ActionMatchMode.Facet => MatchString(matchable.GetFacetValue(Facet)),
        ActionMatchMode.Tag => matchable.Tags.Any(MatchString),
        _ => throw new ArgumentOutOfRangeException(),
    };
}
