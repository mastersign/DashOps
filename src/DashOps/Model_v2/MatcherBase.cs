namespace Mastersign.DashOps.Model_v2;

partial class MatcherBase
{
    protected bool MatchString(string value)
        => !string.IsNullOrWhiteSpace(value) &&
           (Pattern != null
               ? System.Text.RegularExpressions.Regex.IsMatch(value, Pattern)
               : string.Equals(value, Value));
}
