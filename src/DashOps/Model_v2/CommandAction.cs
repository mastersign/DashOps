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
            Command = ExpandEnv(ExpandTemplate(Command, facets)),
            Tags = Tags ?? [],
            Facets = facets,
        };
    }

    public ActionView CreateView(DefaultActionSettings defaults, IReadOnlyList<AutoActionSettings> autoSettings)
    {
        var facets = Facets ?? [];
        var view = new ActionView
        {
            Title = ExpandTemplate(Title, facets),

            Command = ExpandEnv(ExpandTemplate(Command, facets)),
            Arguments = FormatArguments(
                Arguments?
                    .Select(a => ExpandTemplate(a, facets))
                    .Select(ExpandEnv)),
        };
        view.UpdateWith(this, autoSettings, defaults, facets);
        view.UpdateStatusFromLogFile();

        return view;
    }
}
