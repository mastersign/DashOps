using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Mastersign.DashOps;

internal static class ProjectVersionDetection
{
    private static readonly Regex VersionPattern = new Regex(@"^version\:\s+('|"")(?<version>.*?)\1\s*$");

    public static string FindVersionString(TextReader r)
    {
        var line = r.ReadLine();
        while (line != null)
        {
            var m = VersionPattern.Match(line);
            if (m.Success) return m.Groups["version"].Value;
            line = r.ReadLine();
        }
        return null;
    }

    public static bool IsVersionSupported(Stream s, string[] supportedVersions, out string version)
    {
        using (var r = new StreamReader(s, Encoding.UTF8, false, 1024, true))
        {
            version = FindVersionString(r);
        }
        s.Seek(0, SeekOrigin.Begin);
        if (version is null) throw new FormatException("No version attribute found.");
        if (!supportedVersions.Contains(version)) return false;
        return true;
    }
}
