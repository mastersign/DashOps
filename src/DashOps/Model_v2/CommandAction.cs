using Mastersign.DashOps.ViewModel;
using static Mastersign.DashOps.Model_v2.Helper;

namespace Mastersign.DashOps.Model_v2;

partial class CommandAction
{
    public MatchableAction CreateMatchable()
    {
        var facets = CoalesceValues([Facets]);
        return new MatchableAction
        {
            Title = ExpandTemplate(Title, facets),
            Tags = Tags ?? [],
            Facets = facets,
            Command = ExpandEnv(ExpandTemplate(Command, facets)),
        };
    }

    public ActionView CreateView(
        IReadOnlyList<AutoActionSettings> autoSettings, 
        DefaultActionSettings actionDefaults, 
        DefaultSettings commonDefaults)
    {
        var facets = Facets ?? [];
        var view = new ActionView
        {
            Title = ExpandTemplate(Title, facets),
            Tags = Unite([Tags, .. autoSettings.Select(s => s.Tags)]),

            Command = ExpandEnv(ExpandTemplate(Command, facets)),
            Arguments = FormatArguments(
                Coalesce([
                    Arguments,
                    ..autoSettings.Select(s => s.Arguments),
                    actionDefaults?.Arguments,
                    commonDefaults.Arguments,
                ])?
                    .Select(a => ExpandTemplate(a, facets))
                    .Select(ExpandEnv)),
        };
        view.UpdateWith(this, autoSettings, actionDefaults, commonDefaults, facets);
        view.UpdateStatusFromLogFile();

        return view;
    }
}
