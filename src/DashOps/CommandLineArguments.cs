namespace Mastersign.DashOps;

internal class CommandLineArguments
{
    public CommandLineArguments(params string[] args)
    {
        var p = 0;
        while (p < args.Length)
        {
            switch (args[p++])
            {
                case "-?":
                case "/?":
                case "-h":
                case "--help":
                    ShowCommandLineHelp = true;
                    break;
                case "-p":
                case "--preserve-working-dir":
                    PreserveWorkingDirectory = true;
                    break;
                default:
                    if (ProjectFile is null)
                    {
                        ProjectFile = args[p++];
                    }
                    else
                    {
                        parsingErrors.Add(Properties.Resources.Common.CommandLine_MultiplePositionals);
                    }
                    break;
            }
        }
    }

    public bool ShowCommandLineHelp { get; private set; }

    public string ProjectFile { get; private set; }

    public bool PreserveWorkingDirectory { get; private set; }

    private readonly List<string> parsingErrors = [];

    public IReadOnlyList<string> ParsingErrors => parsingErrors;

    public bool HasErrors => parsingErrors.Count > 0;
}
