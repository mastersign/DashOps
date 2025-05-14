using Mastersign.DashOps.ViewModel;

namespace Mastersign.DashOps.Model_v1;

partial class ActionMatcher
{
    private bool MatchString(string value)
        => !string.IsNullOrWhiteSpace(value) &&
           (Pattern != null
               ? System.Text.RegularExpressions.Regex.IsMatch(value, Pattern)
               : string.Equals(value, Value));

    private string GetFacet() => Facet ?? Facette;

    private string NormalizedFacet
    {
        get
        {
            var facet = GetFacet();
            if (string.Equals(
                facet, nameof(CommandAction.Verb),
                StringComparison.InvariantCultureIgnoreCase))
            {
                return nameof(CommandAction.Verb);
            }
            if (string.Equals(
                facet, nameof(CommandAction.Service),
                StringComparison.InvariantCultureIgnoreCase))
            {
                return nameof(CommandAction.Service);
            }
            if (string.Equals(
                facet, nameof(CommandAction.Host),
                StringComparison.InvariantCultureIgnoreCase))
            {
                return nameof(CommandAction.Host);
            }
            return facet;
        }
    }

    public bool Match(ActionView actionView) => Mode switch
    {
        ActionMatchMode.Description => MatchString(actionView.Title),
        ActionMatchMode.Command => MatchString(actionView.Command),
        ActionMatchMode.Facet => MatchString(actionView.GetFacetValue(NormalizedFacet)),
        ActionMatchMode.Facette => MatchString(actionView.GetFacetValue(NormalizedFacet)),
        ActionMatchMode.Tag => actionView.Tags.Any(MatchString),
        _ => throw new ArgumentOutOfRangeException(),
    };
}
