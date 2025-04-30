using static Mastersign.DashOps.Model_v2.Helper;

namespace Mastersign.DashOps.Model_v2;

partial class CommandMonitorDiscovery
{
    private const string FILE_VARIABLE = "file";

    public MatchableMonitor CreateMatchable(IReadOnlyDictionary<string, string> discoveryVariables, string filePath)
    {
        var fileVariable = new Dictionary<string, string> { { FILE_VARIABLE, filePath } };
        var variables = CoalesceValues([fileVariable, discoveryVariables]);

        string cmd;
        if (!string.IsNullOrWhiteSpace(Interpreter))
        {
            // custom interpreter for discovered files
            cmd = ExpandEnv(ExpandTemplate(Interpreter, variables));
        }
        else
        {
            // discovered file is used as command itself
            cmd = filePath;
        }

        return new MatchableMonitor
        {
            Title = ExpandTemplate(Title, variables),
            Command = cmd,
            Variables = variables,
            Tags = Tags ?? [],
        };
    }

    public CommandMonitorView CreateView(DefaultMonitorSettings defaults, IReadOnlyList<AutoMonitorSettings> autoSettings, IReadOnlyDictionary<string, string> discoveryVariables, string filePath)
    {
        var fileVariable = new Dictionary<string, string> { { FILE_VARIABLE, filePath } };
        var variables = CoalesceValues([fileVariable, discoveryVariables]);

        string cmd;
        string cmdArgs;
        if (!string.IsNullOrWhiteSpace(Interpreter))
        {
            // custom interpreter for discovered files

            cmd = ExpandEnv(ExpandTemplate(Interpreter, variables));
            if (Arguments is null)
            {
                cmdArgs = FormatArguments([filePath]);
            }
            else
            {
                cmdArgs = FormatArguments(Arguments
                    // expand facets
                    .Select(a => ExpandTemplate(a, variables))
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
                .Select(a => ExpandTemplate(a, variables))
                // expand CMD-style environment variables
                .Select(ExpandEnv));
        }

        var view = new CommandMonitorView
        {
            Command = cmd,
            Arguments = cmdArgs,
        };
        view.UpdateWith(this, autoSettings, defaults, variables);
        view.UpdateStatusFromLogFile();

        return view;
    }
}
