using System.IO;

namespace Mastersign.DashOps;

public static class ProjectLoaderFactory
{
    public static IProjectLoader CreateProjectLoaderFor(string file, Action<Action> dispatcher, out string version)
    {
        using var s = File.OpenRead(file);

        try
        {
            if (ProjectLoader_v2.IsCompatible(s, out version)) return new ProjectLoader_v2(file, dispatcher);
            if (ProjectLoader_v1.IsCompatible(s, out version)) return new ProjectLoader_v1(file, dispatcher);
        }
#if DEBUG
        catch (FormatException e)
        {
            UserInteraction.ShowMessage(
                "Create Project Loader",
                "Failed to determine project file version:"
                + Environment.NewLine
                + Environment.NewLine
                + e,
                symbol: InteractionSymbol.Error);
            version = null;
        }
#else
        catch (FormatException)
        {
            version = null;
        }
#endif

        return null;
    }
}
