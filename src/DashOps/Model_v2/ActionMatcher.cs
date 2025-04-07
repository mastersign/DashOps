namespace Mastersign.DashOps.Model_v2;

partial class ActionMatcher
{
    private bool MatchString(string value)
        => !string.IsNullOrWhiteSpace(value) &&
           (Pattern != null
               ? System.Text.RegularExpressions.Regex.IsMatch(value, Pattern)
               : string.Equals(value, Value));

    private string NormalizedFacet
    {
        get
        {
            // no normalization since v2
            return Facet;
        }
    }

    public bool Match(ActionView actionView) => Mode switch
    {
        ActionMatchMode.Description => MatchString(actionView.Title),
        ActionMatchMode.Command => MatchString(actionView.Command),
        ActionMatchMode.Facet => MatchString(actionView.GetFacetValue(NormalizedFacet)),
        ActionMatchMode.Tag => actionView.Tags.Any(MatchString),
        _ => throw new ArgumentOutOfRangeException(),
    };
}
