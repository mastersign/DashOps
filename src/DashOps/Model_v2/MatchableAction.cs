namespace Mastersign.DashOps.Model_v2;

partial class MatchableAction
{
    public string GetFacetValue(string name)
        => Facets.TryGetValue(name, out var v) ? v : null;
}
