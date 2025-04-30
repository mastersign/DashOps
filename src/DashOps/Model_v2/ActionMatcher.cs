namespace Mastersign.DashOps.Model_v2;

partial class ActionMatcher
{
    private bool MatchString(string value)
        => !string.IsNullOrWhiteSpace(value) &&
           (Pattern != null
               ? System.Text.RegularExpressions.Regex.IsMatch(value, Pattern)
               : string.Equals(value, Value));


    public bool Match(MatchableAction actionView) => Mode switch
    {
        ActionMatchMode.Title => MatchString(actionView.Title),
        ActionMatchMode.Command => MatchString(actionView.Command),
        ActionMatchMode.Facet => MatchString(actionView.GetFacetValue(Facet)),
        ActionMatchMode.Tag => actionView.Tags.Any(MatchString),
        _ => throw new ArgumentOutOfRangeException(),
    };
}
