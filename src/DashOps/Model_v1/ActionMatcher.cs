namespace Mastersign.DashOps.Model_v1;

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
            if (string.Equals(
                Facette, nameof(CommandAction.Verb),
                StringComparison.InvariantCultureIgnoreCase))
            {
                return nameof(CommandAction.Verb);
            }
            if (string.Equals(
                Facette, nameof(CommandAction.Service),
                StringComparison.InvariantCultureIgnoreCase))
            {
                return nameof(CommandAction.Service);
            }
            if (string.Equals(
                Facette, nameof(CommandAction.Host),
                StringComparison.InvariantCultureIgnoreCase))
            {
                return nameof(CommandAction.Host);
            }
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
