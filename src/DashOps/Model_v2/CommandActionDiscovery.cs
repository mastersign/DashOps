using Mastersign.DashOps.ViewModel;
using static Mastersign.DashOps.Model_v2.Helper;

namespace Mastersign.DashOps.Model_v2;

partial class CommandActionDiscovery
{
    private const string FILE_FACET = "file";

    public MatchableAction CreateMatchable(
        DefaultActionSettings actionDefaults,
        DefaultSettings commonDefaults,
        IReadOnlyDictionary<string, string> discoveryFacets,
        string filePath)
    {
        var fileVariable = new Dictionary<string, string> { { FILE_FACET, filePath } };
        var facets = CoalesceValues([Facets, fileVariable, discoveryFacets]);

        var interpreter = Coalesce([
            Interpreter,
            actionDefaults?.Interpreter,
            commonDefaults.Interpreter,
        ]);

        string cmd;
        if (!string.IsNullOrWhiteSpace(interpreter))
        {
            // custom interpreter for discovered files
            cmd = ExpandEnv(ExpandTemplate(interpreter, facets));
        }
        else
        {
            // discovered file is used as command itself
            cmd = filePath;
        }

        return new MatchableAction
        {
            Title = ExpandTemplate(Title, facets),
            Tags = Tags ?? [],
            Facets = facets,
            Command = cmd,
        };
    }

    public ActionView CreateView(
        IReadOnlyList<AutoActionSettings> autoSettings,
        DefaultActionSettings actionDefaults,
        DefaultSettings commonDefaults,
        IReadOnlyDictionary<string, string> discoveryFacets,
        string filePath)
    {
        var fileVariable = new Dictionary<string, string> { { FILE_FACET, filePath } };
        var facets = CoalesceValues([Facets, fileVariable, discoveryFacets]);

        var interpreter = Coalesce([
            Interpreter,
            .. autoSettings.Select(s => s.Interpreter),
            actionDefaults?.Interpreter,
            commonDefaults.Interpreter,
        ]);

        var arguments = Coalesce([
            Arguments,
            ..autoSettings.Select(s => s.Arguments),
            actionDefaults?.Arguments,
            commonDefaults.Arguments,
        ]);

        string cmd;
        string cmdArgs;
        if (!string.IsNullOrWhiteSpace(interpreter))
        {
            // custom interpreter for discovered files

            cmd = ExpandEnv(ExpandTemplate(interpreter, facets));
            if (arguments is null)
            {
                cmdArgs = FormatArguments([filePath]);
            }
            else
            {
                cmdArgs = FormatArguments(arguments
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
            cmdArgs = FormatArguments(
                arguments?
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
        view.UpdateWith(this, autoSettings, actionDefaults, commonDefaults, facets);
        view.UpdateStatusFromLogFile();

        return view;
    }
}
