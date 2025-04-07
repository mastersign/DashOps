namespace Mastersign.DashOps.Model_v2;

partial class AutoAnnotation
{
    public bool Match(ActionView action)
    {
        if (Include != null && Include.Any(m => !m.Match(action))) return false;
        if (Exclude != null && Exclude.Any(m => m.Match(action))) return false;
        return true;
    }

    public void Apply(ActionView action)
    {
        if (Tags != null)
        {
            action.Tags = [.. action.Tags.Union(Tags)];
        }

        if (Facets != null)
        {
            foreach (var facetName in Facets.Keys)
            {
                action.Facets[facetName] = Facets[facetName];
            }
        }

        action.Reassure = Reassure ?? action.Reassure;
        action.NoLogs = NoLogs ?? action.NoLogs;
        action.KeepOpen = KeepOpen ?? action.KeepOpen;
        action.AlwaysClose = AlwaysClose ?? action.AlwaysClose;
        action.Visible = !(Background ?? !action.Visible);
    }
}
