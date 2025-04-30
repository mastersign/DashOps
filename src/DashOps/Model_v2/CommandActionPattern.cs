using static Mastersign.DashOps.Model_v2.Helper;

namespace Mastersign.DashOps.Model_v2;

partial class CommandActionPattern
{
    public MatchableAction CreateMatchable(IDictionary<string, string> instanceFacets)
    {
        var facets = CoalesceValues([instanceFacets]);
        return new MatchableAction
        {
            Title = ExpandTemplate(Title, facets),
            Command = ExpandEnv(ExpandTemplate(Command, facets)),
            Tags = Tags ?? [],
            Facets = facets,
        };
    }

    public ActionView CreateView(DefaultActionSettings defaults, IReadOnlyList<AutoActionSettings> autoSettings, IDictionary<string, string> instanceFacets)
    {
        var facets = instanceFacets;
        var actionView = new ActionView
        {
            Title = ExpandTemplate(Title, facets),

            Command = ExpandEnv(ExpandTemplate(Command, facets)),
            Arguments = FormatArguments(
                Arguments?
                    .Select(a => ExpandTemplate(a, facets))
                    .Select(ExpandEnv)),
        };
        actionView.UpdateWith(this, autoSettings, defaults, facets);
        actionView.UpdateStatusFromLogFile();

        return actionView;
    }
}
