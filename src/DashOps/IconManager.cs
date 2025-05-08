using System.Reflection;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Mastersign.DashOps;

internal static class IconManager
{
    private static ColorTheme _theme = ColorTheme.Light;
    private static ThemePaletteColor _color = ThemePaletteColor.Default;

    public static void Initialize(ColorTheme theme, ThemePaletteColor color)
    {
        _theme = theme;
        _color = color;
    }

    public static void LoadIcon(Window window)
    {
        var iconUri = new Uri($"pack://application:,,,/{Assembly.GetExecutingAssembly().GetName().Name};component/WpfResources/Icons/{_color}_{_theme}.ico");
        var iconFrame = BitmapFrame.Create(iconUri, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
        window.Icon = iconFrame;
    }

    public static ImageSource LoadLogo(int size)
    {
        throw new NotImplementedException();
    }
}
