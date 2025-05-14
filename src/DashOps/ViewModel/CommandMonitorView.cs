using System.IO;

namespace Mastersign.DashOps.ViewModel;

partial class CommandMonitorView : IExecutable
{
    public bool Visible => false;

    public bool KeepOpen => false;

    public bool AlwaysClose => false;

    private string _commandId;

    public override string CommandId
        => _commandId ??= IdBuilder.BuildId(Command + " " + Arguments);

    public string CommandLabel => Command
        + (string.IsNullOrWhiteSpace(Arguments)
            ? string.Empty
            : " " + Arguments);

    public override string ToString() => $"[{CommandId}] {Title}: {CommandLabel}";

    public override async Task<bool> Check(DateTime startTime)
    {
        NotifyMonitorBegin(startTime);
        var result = await App.Instance.Executor.ExecuteAsync(this);
        NotifyMonitorFinished(result.Success);
        return result.Success;
    }

    public Func<string, bool> SuccessCheck
        => output => RequiredPatterns.All(p => p.IsMatch(output)) &&
                     !ForbiddenPatterns.Any(p => p.IsMatch(output));

    protected override void NotifyMonitorFinished(bool success)
    {
        base.NotifyMonitorFinished(success);
        if (!HasExecutionResultChanged && CurrentLogFile != null && File.Exists(CurrentLogFile))
        {
            File.Delete(CurrentLogFile);
            CurrentLogFile = null;
        }
    }

    public void NotifyExecutionFinished()
    {
        // nothing
    }

    public bool PrintExecutionInfo => !NoExecutionInfo;

    public string ExitCodesFormatted => string.Join(", ", ExitCodes ?? []);

    public bool UseWindowsTerminal => false;

    public string WindowsTerminalArguments => null;
}
