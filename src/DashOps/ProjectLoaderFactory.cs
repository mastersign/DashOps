using System.IO;
using System.Windows;

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
        catch (FormatException e)
        {
#if DEBUG
            MessageBox.Show(
                "Failed to determine project file version:"
                + Environment.NewLine
                + Environment.NewLine
                + e,
                "Create Project Loader",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
#endif
            version = null;
        }

        return null;
    }
}
