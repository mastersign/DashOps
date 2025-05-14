using System.IO;
using Mastersign.DashOps.Exceptions;
using Mastersign.DashOps.Model_v1;
using Mastersign.DashOps.Model_v2;

namespace Mastersign.DashOps;

public static class ProjectLoaderFactory
{
    public static IProjectLoader CreateProjectLoaderFor(string file, Action<Action> dispatcher)
    {
        using var s = File.OpenRead(file);
        string version;
        try
        {
            if (ProjectLoader_v2.IsCompatible(s, out version)) return new ProjectLoader_v2(file, dispatcher);
            if (ProjectLoader_v1.IsCompatible(s, out version)) return new ProjectLoader_v1(file, dispatcher);
        }
        catch (FormatException e)
        {
            throw new ProjectLoaderFactoryException("Failed to determine project format version", e);
        }
        throw new UnsupportedProjectFormatException(version);
    }
}
