namespace Mastersign.DashOps.Model_v2;

partial class ActionMatcher
{
    private bool MatchString(string value)
        => !string.IsNullOrWhiteSpace(value) &&
           (Pattern != null
               ? System.Text.RegularExpressions.Regex.IsMatch(value, Pattern)
               : string.Equals(value, Value));

    private string NormalizedFacette
    {
        get
        {
            // no normalization since v2
            return Facette;
        }
    }

    public bool Match(ActionView actionView)
    {
        switch (Mode)
        {
            case ActionMatchMode.Description:
                return MatchString(actionView.Title);
            case ActionMatchMode.Command:
                return MatchString(actionView.Command);
            case ActionMatchMode.Facette:
                return MatchString(actionView.GetFacetteValue(NormalizedFacette));
            case ActionMatchMode.Tag:
                return actionView.Tags.Any(MatchString);
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
