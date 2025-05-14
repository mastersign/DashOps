using Mastersign.DashOps.ViewModel;
using static Mastersign.DashOps.Model_v2.Helper;

namespace Mastersign.DashOps.Model_v2;

partial class CommandMonitorDiscovery
{
    private const string FILE_VARIABLE = "file";

    public MatchableMonitor CreateMatchable(
        DefaultMonitorSettings monitorDefaults,
        DefaultSettings commonDefaults,
        IReadOnlyDictionary<string, string> discoveryVariables,
        string filePath)
    {
        var fileVariable = new Dictionary<string, string> { { FILE_VARIABLE, filePath } };
        var variables = CoalesceValues([fileVariable, discoveryVariables]);

        var interpreter = Coalesce([
            Interpreter,
            monitorDefaults?.Interpreter,
            commonDefaults.Interpreter,
        ]);

        string cmd;
        if (!string.IsNullOrWhiteSpace(interpreter))
        {
            // custom interpreter for discovered files
            cmd = ExpandEnv(ExpandTemplate(interpreter, discoveryVariables));
        }
        else
        {
            // discovered file is used as command itself
            cmd = filePath;
        }

        return new MatchableMonitor
        {
            Title = ExpandTemplate(Title, variables),
            Tags = Tags ?? [],
            Variables = variables,
            Command = cmd,
        };
    }

    public CommandMonitorView CreateView(
        IReadOnlyList<AutoMonitorSettings> autoSettings,
        DefaultMonitorSettings monitorDefaults,
        DefaultSettings commonDefaults,
        IReadOnlyDictionary<string, string> discoveryVariables, 
        string filePath)
    {
        var fileVariable = new Dictionary<string, string> { { FILE_VARIABLE, filePath } };
        var variables = CoalesceValues([fileVariable, discoveryVariables]);

        var interpreter = Coalesce([
            Interpreter,
            .. autoSettings.Select(s => s.Interpreter),
            monitorDefaults?.Interpreter,
            commonDefaults.Interpreter,
        ]);

        var arguments = Coalesce([
            Arguments,
            ..autoSettings.Select(s => s.Arguments),
            monitorDefaults?.Arguments,
            commonDefaults.Arguments,
        ]);

        string cmd;
        string cmdArgs;
        if (!string.IsNullOrWhiteSpace(interpreter))
        {
            // custom interpreter for discovered files

            cmd = ExpandEnv(ExpandTemplate(interpreter, variables));
            if (arguments is null)
            {
                cmdArgs = FormatArguments([filePath]);
            }
            else
            {
                cmdArgs = FormatArguments(arguments
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
            cmdArgs = FormatArguments(arguments?
                // expand facets
                .Select(a => ExpandTemplate(a, variables))
                // expand CMD-style environment variables
                .Select(ExpandEnv));
        }

        var view = new CommandMonitorView
        {
            Tags = Unite([Tags]),

            Command = cmd,
            Arguments = cmdArgs,
        };
        view.UpdateWith(this, autoSettings, monitorDefaults, commonDefaults, variables);
        view.UpdateStatusFromLogFile();

        return view;
    }
}
