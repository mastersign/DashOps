using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace Mastersign.DashOps.Model_v2;

static internal class Helper
{
    public static bool Coalesce(IEnumerable<bool?> values, bool fallback = false)
    {
        foreach (var v in values)
        {
            if (v.HasValue) return v.Value;
        }
        return fallback;
    }

    public static double Coalesce(IEnumerable<double?> values, double fallback = 0)
    {
        foreach (var v in values)
        {
            if (v.HasValue) return v.Value;
        }
        return fallback;
    }

    public static string Coalesce(IEnumerable<string> values)
    {
        foreach (var v in values)
        {
            if (v != null) return v;
        }
        return null;
    }
    
    public static string CoalesceEmpty(IEnumerable<string> values)
    {
        foreach (var v in values)
        {
            if (!string.IsNullOrEmpty(v)) return v;
        }
        return null;
    }

    public static string CoalesceWhitespace(IEnumerable<string> values)
    {
        foreach (var v in values)
        {
            if (!string.IsNullOrWhiteSpace(v)) return v;
        }
        return null;
    }

    public static T[] Coalesce<T>(IEnumerable<T[]> lists)
    {
        foreach (var l in lists)
        {
            if (l != null) return l;
        }
        return [];
    }

    public static string[] Unite(IEnumerable<string[]> lists) 
        => [.. lists.Where(l => l is not null).SelectMany(l => l).Distinct()];

    public static Dictionary<TKey, TValue> CoalesceValues<TKey, TValue>(IEnumerable<IReadOnlyDictionary<TKey, TValue>> dicts)
    {
        var result = new Dictionary<TKey, TValue>();
        foreach (var d in dicts.Reverse())
        {
            if (d is null) continue;
            foreach (var kvp in d)
            {
                result[kvp.Key] = kvp.Value;
            }
        }
        return result;
    }

    public static Dictionary<TKey, TValue> Merge<TKey, TValue>(params IReadOnlyDictionary<TKey, TValue>[] dicts)
    {
        var result = new Dictionary<TKey, TValue>();
        foreach (var d in dicts)
        {
            if (d is null) continue;
            foreach (var kvp in d)
            {
                result[kvp.Key] = kvp.Value;
            }
        }
        return result;
    }

    public static IEnumerable<Dictionary<string, string>> EnumerateVariations(Dictionary<string, string[]> dimensions)
    {
        IEnumerable<Dictionary<string, string>> AddDimension(IEnumerable<Dictionary<string, string>> values, string key)
        {
            return values.SelectMany(d => dimensions[key], (d2, v) => new Dictionary<string, string>(d2) { { key, v } });
        }
        return dimensions.Keys.Aggregate(Enumerable.Repeat(new Dictionary<string, string>(), 1), AddDimension);
    }

    public static Dictionary<string, string> ExpandDictionaryTemplate(IReadOnlyDictionary<string, string> dict, IReadOnlyDictionary<string, string> variables)
        => MapValues(dict, v => ExpandTemplate(v, variables));

    public static string ExpandEnv(string template)
        => string.IsNullOrWhiteSpace(template)
            ? template
            : Environment.ExpandEnvironmentVariables(template);

    public static Dictionary<string, string> ExpandEnv(IReadOnlyDictionary<string, string> dict)
        => MapValues(dict, ExpandEnv);

    public static string ExpandTemplate(string template, IReadOnlyDictionary<string, string> variables)
    {
        if (string.IsNullOrWhiteSpace(template)) return template;
        var result = template;
        foreach (var kvp in variables)
        {
            result = result.Replace("${" + kvp.Key + "}", kvp.Value);
            result = result.Replace("${" + kvp.Key.ToLowerInvariant() + "}", kvp.Value);
        }
        return result;
    }

    public static string FormatArguments(IEnumerable<string> arguments)
        => arguments != null
            ? CommandLine.FormatArgumentList([.. arguments.Select(ExpandEnv)])
            : null;

    private static Dictionary<TKey, TValue> MapValues<TKey, TValue>(
        IReadOnlyDictionary<TKey, TValue> dict, Func<TValue, TValue> f)
    {
        if (dict == null) return [];
        var result = new Dictionary<TKey, TValue>();
        foreach (var kvp in dict)
        {
            result.Add(kvp.Key, f(kvp.Value));
        }
        return result;
    }

    public static string BuildAbsolutePath(string path) => BuildAbsolutePath(path, basePath: null);

    public static string BuildAbsolutePath(string path, string basePath, bool resolveNull = false)
    {
        if (path is null && !resolveNull) return null;
        basePath ??= Environment.CurrentDirectory;
        if (path is null || path.Length == 0 || path == ".")
        {
            return basePath;
        }
        path = ExpandEnv(path.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
        if (!Path.IsPathRooted(path))
        {
            path = Path.Combine(basePath, path);
        }
        return path;
    }

    public static IEnumerable<Tuple<string, Match>> DiscoverFiles(string basePath, Regex pattern)
    {
        foreach (var file in Directory.EnumerateFiles(basePath, "*", SearchOption.AllDirectories))
        {
            Debug.Assert(file.StartsWith(basePath, StringComparison.OrdinalIgnoreCase));
            var relativePath = file.Substring(basePath.Length + 1);
            var m = pattern.Match(relativePath);
            if (!m.Success) continue;
            yield return Tuple.Create(file, m);
        }
    }

    public static Regex[] BuildTextPatterns(IEnumerable<string> patterns)
    {
        try
        {
            return patterns?.Select(p => new Regex(p,
                    RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Multiline,
                    TimeSpan.FromMilliseconds(1000))
                ).ToArray() ?? [];
        }
        catch (ArgumentException exc)
        {
            UserInteraction.ShowMessage(
                "Parsing Regular Expression",
                "Error in regular expression: " + exc.Message,
                symbol: InteractionSymbol.Error);
            return [];
        }
    }

    public static Regex BuildPathPattern(string pattern)
    {
        if (string.IsNullOrWhiteSpace(pattern)) return null;
        try
        {
            return new Regex(pattern,
                RegexOptions.ExplicitCapture | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
        }
        catch (ArgumentException exc)
        {
            UserInteraction.ShowMessage(
                "Parsing Regular Expression",
                "Error in regular expression: " + exc.Message,
                symbol: InteractionSymbol.Warning);
            return null;
        }
    }

}
