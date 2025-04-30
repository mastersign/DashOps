namespace Mastersign.DashOps.Model_v2;

partial class MatchableAction
{
    public string GetFacetValue(string facetName)
        => Facets.TryGetValue(facetName, out var facetValue) ? facetValue : null;
}
