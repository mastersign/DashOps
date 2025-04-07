namespace Mastersign.DashOps.Model_v1;

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
            action.Tags = action.Tags
                .Union(Tags)
                .ToArray();
        }

        if (Facets != null)
        {
            foreach (var facetName in Facets.Keys)
            {
                if (!action.Facets.ContainsKey(facetName))
                {
                    action.Facets[facetName] = Facets[facetName];
                }
            }
        }

        action.Reassure = action.Reassure || Reassure;
        action.NoLogs = action.NoLogs || NoLogs;
        action.KeepOpen = action.KeepOpen || KeepOpen;
        action.AlwaysClose = action.AlwaysClose || AlwaysClose;
        action.Visible = action.Visible && !Background;
    }
}
