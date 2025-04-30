using static Mastersign.DashOps.Model_v2.Helper;

namespace Mastersign.DashOps.Model_v2;

partial class CommandAction
{
    public MatchableAction CreateMatchable()
    {
        var facets = CoalesceValues([Facets]);
        return new MatchableAction
        {
            Title = Title,
            Command = ExpandEnv(ExpandTemplate(Command, facets)),
            Tags = Tags ?? [],
            Facets = facets,
        };
    }

    public ActionView CreateView(DefaultActionSettings defaults, IReadOnlyList<AutoActionSettings> autoSettings)
    {
        var facets = Facets ?? [];
        var actionView = new ActionView
        {
            Title = Title,

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
