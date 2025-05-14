namespace Mastersign.DashOps.ViewModel;

partial class ActionView : IExecutable, ILogged
{
    public bool HasFacet(string name)
        => Facets?.ContainsKey(name) ?? false;

    public string[] GetFacets()
        => Facets?.Keys.ToArray() ?? Array.Empty<string>();

    public bool HasFacetValue(string name, string value)
        => string.Equals(GetFacetValue(name), value);

    public string GetFacetValue(string name)
        => Facets != null
            ? Facets.TryGetValue(name, out var value) ? value : null
            : null;

    public string CommandLabel => Command
        + (string.IsNullOrWhiteSpace(Arguments)
            ? string.Empty
            : " " + Arguments);

    private string _commandId;

    public string CommandId
        => _commandId ??= IdBuilder.BuildId(Command + " " + Arguments);

    public override string ToString() => $"[{CommandId}] {Title}: {CommandLabel}";

    public void NotifyExecutionFinished()
    {
        // nothing for now
    }

    public bool PrintExecutionInfo => !NoExecutionInfo;

    public Task<ExecutionResult> ExecuteAsync() => App.Instance.Executor.ExecuteAsync(this);

    public Func<string, bool> SuccessCheck => null;

    public string ExitCodesFormatted => string.Join(", ", ExitCodes);

    public void UpdateStatusFromLogFile()
    {
        var logInfo = this.GetLastLogFileInfo();
        if (logInfo != null)
        {
            Status = logInfo.Success ? ActionStatus.Success : ActionStatus.Failed;
        }
        else
        {
            Status = ActionStatus.Unknown;
        }
    }

}
