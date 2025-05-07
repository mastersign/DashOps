using static Mastersign.DashOps.Model_v2.Helper;

namespace Mastersign.DashOps.Model_v2;

partial class CommandActionDiscovery
{
    private const string FILE_FACET = "file";

    public MatchableAction CreateMatchable(IReadOnlyDictionary<string, string> discoveryFacets, string filePath)
    {
        var fileVariable = new Dictionary<string, string> { { FILE_FACET, filePath } };
        var facets = CoalesceValues([Facets, fileVariable, discoveryFacets]);

        string cmd;
        if (!string.IsNullOrWhiteSpace(Interpreter))
        {
            // custom interpreter for discovered files
            cmd = ExpandEnv(ExpandTemplate(Interpreter, facets));
        }
        else
        {
            // discovered file is used as command itself
            cmd = filePath;
        }

        return new MatchableAction
        {
            Title = ExpandTemplate(Title, facets),
            Command = cmd,
            Tags = Tags ?? [],
            Facets = facets,
        };
    }

    public ActionView CreateView(
        IReadOnlyList<AutoActionSettings> autoSettings,
        DefaultActionSettings defaults,
        DefaultSettings commonDefaults,
        IReadOnlyDictionary<string, string> discoveryFacets,
        string filePath)
    {
        var fileVariable = new Dictionary<string, string> { { FILE_FACET, filePath } };
        var facets = CoalesceValues([Facets, fileVariable, discoveryFacets]);

        string cmd;
        string cmdArgs;
        if (!string.IsNullOrWhiteSpace(Interpreter))
        {
            // custom interpreter for discovered files

            cmd = ExpandEnv(ExpandTemplate(Interpreter, facets));
            if (Arguments is null)
            {
                cmdArgs = FormatArguments([filePath]);
            }
            else
            {
                cmdArgs = FormatArguments(Arguments
                    // expand facets
                    .Select(a => ExpandTemplate(a, facets))
                    // expand CMD-style environment variables
                    .Select(ExpandEnv));
            }
        }
        else
        {
            // discovered file is used as command itself

            cmd = filePath;
            cmdArgs = FormatArguments(Arguments?
                // expand facets
                .Select(a => ExpandTemplate(a, facets))
                // expand CMD-style environment variables
                .Select(ExpandEnv));
        }

        var view = new ActionView
        {
            Title = ExpandTemplate(Title, facets),
            Tags = Unite([Tags, .. autoSettings.Select(s => s.Tags)]),

            Command = cmd,
            Arguments = cmdArgs,
        };
        view.UpdateWith(this, autoSettings, defaults, commonDefaults, facets);
        view.UpdateStatusFromLogFile();

        return view;
    }
}
