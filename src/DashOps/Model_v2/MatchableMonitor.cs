namespace Mastersign.DashOps.Model_v2;

partial class MatchableMonitor
{
    public string GetHeaderValue(string name)
        => Headers.TryGetValue(name, out var v) ? v : null;

    public string GetVariableValue(string name)
        => Variables.TryGetValue(name, out var v) ? v : null;
}
