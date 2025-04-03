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
            action.Tags = action.Tags
                .Union(Tags)
                .ToArray();
        }

        if (Facettes != null)
        {
            foreach (var facetteName in Facettes.Keys)
            {
                if (!action.Facettes.ContainsKey(facetteName))
                {
                    action.Facettes[facetteName] = Facettes[facetteName];
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
