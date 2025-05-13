using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using Wpf.Ui.Appearance;
using System.Diagnostics;
using Wpf.Ui;
using System.Collections;
using System.Windows.Markup;
using Wpf.Ui.Extensions;

namespace Mastersign.WpfUiTools;

public class ThemeManager
{
    private static readonly Theme[] REAL_THEMES = [
        Theme.Light,
        Theme.Dark,
    ];

    private bool IsRealTheme(Theme theme) => REAL_THEMES.Contains(theme);

    private static Theme ThemeFrom(SystemTheme systemTheme)
        => systemTheme switch
        {
            SystemTheme.Dark => Theme.Dark,
            SystemTheme.Light => Theme.Light,
            _ => Theme.Light,
        };

    private static ApplicationTheme UiThemeFrom(Theme theme)
        => theme switch
        {
            Theme.Dark => ApplicationTheme.Dark,
            Theme.Light => ApplicationTheme.Light,
            _ => ApplicationTheme.Unknown,
        };

    public Assembly ResourceAssembly { get; set; } = Assembly.GetExecutingAssembly();

    private string ResourceAssemblyName => ResourceAssembly.GetName().Name;

    public string IconBasePath { get; set; } = "Icons";

    public ResourceDictionary AppResources { get; set; }

    private readonly Dictionary<int, string> iconImageResourceNames = [];

    private Window mainWindow;

    private readonly List<Window> windows = [];

    private Theme appTheme = Theme.Auto;
    private Theme selectedTheme = Theme.System;
    private Theme systemTheme = Theme.Light;
    private Theme theme = Theme.Light;

    private ThemeAccentColor accentColor = ThemeAccentColor.Default;

    public ThemeManager()
    {
        systemTheme = ThemeFrom(SystemThemeManager.GetCachedSystemTheme());
        Debug.Assert(IsRealTheme(systemTheme));
        ApplyTheme();
    }

    public Theme SelectedTheme
    {
        get => selectedTheme;
        set
        {
            if (value == selectedTheme) return;
            selectedTheme = value;
            SelectedThemeChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public event EventHandler SelectedThemeChanged;

    private Uri ResourceUri(string path)
        => new($"pack://application:,,,/{ResourceAssemblyName};component/{path}");

    private Uri IconResourceUri(string filename)
        => ResourceUri($"{IconBasePath.Trim('/')}/{filename}");

    public ImageSource GetIconImage(int size)
        => new BitmapImage(IconResourceUri($"{accentColor}_{theme}_{size}.png"));

    public void RegisterIconImageResource(int size, string name)
        => iconImageResourceNames[size] = name;

    private void UpdateIconResources()
    {
        if (AppResources is null) return;
        foreach (var kvp in iconImageResourceNames)
        {
            AppResources[kvp.Value] = GetIconImage(kvp.Key);
        }
    }

    public void UpdateWindowIcon(Window window)
    {
        var iconUri = IconResourceUri($"{accentColor}_{theme}.ico");
        var iconFrame = BitmapFrame.Create(iconUri, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
        window.Icon = iconFrame;
    }

    private bool UseSystemAccent => accentColor == ThemeAccentColor.Default;

    private void UpdateWindow(Window window)
    {
        UpdateWindowIcon(window);
        var uiTheme = UiThemeFrom(theme);
        WindowBackgroundManager.UpdateBackground(window, uiTheme, Wpf.Ui.Controls.WindowBackdropType.Mica);

        SystemThemeWatcher.UnWatch(window);
        if (SelectedTheme == Theme.System)
        {
            SystemThemeWatcher.Watch(
                window,                         // Window class
                Wpf.Ui.Controls.WindowBackdropType.Mica,
                updateAccents: UseSystemAccent  // Whether to change accents automatically
            );
        }
    }

    public Window MainWindow
    {
        get => mainWindow;
        set
        {
            if (mainWindow != null)
            {
                UnregisterMainWindowHook(mainWindow);
                mainWindow.DpiChanged -= MainWindowDpiChangedHandler;
            }
            mainWindow = value;
            if (mainWindow != null)
            {
                RegisterMainWindowHook(mainWindow);
                mainWindow.DpiChanged += MainWindowDpiChangedHandler;
            }
        }
    }

    private void RegisterMainWindowHook(Window window)
    {
        var source = (HwndSource)PresentationSource.FromVisual(window);
        source.AddHook(MainWindowHook);
    }

    private void UnregisterMainWindowHook(Window window)
    {
        var source = (HwndSource)PresentationSource.FromVisual(window);
        source.RemoveHook(MainWindowHook);
    }

    private IntPtr MainWindowHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        const int WM_SETTINGCHANGE = 0x001A;
        if (msg == WM_SETTINGCHANGE
            && wParam == IntPtr.Zero
            && Marshal.PtrToStringUni(lParam) == "ImmersiveColorSet")
        {
            SystemThemeChangedHandler();
        }
        return IntPtr.Zero;
    }

    private void SystemThemeChangedHandler()
    {
        SystemThemeManager.UpdateSystemThemeCache();
        systemTheme = ThemeFrom(SystemThemeManager.GetCachedSystemTheme());
        Debug.Assert(IsRealTheme(systemTheme));
        ApplyTheme();
    }

    private void MainWindowDpiChangedHandler(object sender, DpiChangedEventArgs args)
    {
        // TODO reload window icons and application icon images, best fitting the current HighDPI factor
    }

    private static Theme AccentColorThemePreference(ThemeAccentColor color)
        => color switch
        {
            ThemeAccentColor.Red => Theme.System,
            ThemeAccentColor.Pink => Theme.System,
            ThemeAccentColor.Purple => Theme.System,
            ThemeAccentColor.DeepPurple => Theme.Light,
            ThemeAccentColor.Indigo => Theme.Light,
            ThemeAccentColor.Blue => Theme.System,
            ThemeAccentColor.LightBlue => Theme.Dark,
            ThemeAccentColor.Cyan => Theme.System,
            ThemeAccentColor.Teal => Theme.System,
            ThemeAccentColor.Green => Theme.System,
            ThemeAccentColor.LightGreen => Theme.Dark,
            ThemeAccentColor.Lime => Theme.Dark,
            ThemeAccentColor.Yellow => Theme.Dark,
            ThemeAccentColor.Amber => Theme.System,
            ThemeAccentColor.Orange => Theme.System,
            ThemeAccentColor.DeepOrange => Theme.System,
            ThemeAccentColor.Brown => Theme.System,
            ThemeAccentColor.Grey => Theme.System,
            ThemeAccentColor.BlueGrey => Theme.System,
            _ => Theme.System,
        };

    private void SelectTheme()
    {
        SelectedTheme = appTheme == Theme.Auto
            ? AccentColorThemePreference(accentColor)
            : appTheme;
        Debug.Assert(SelectedTheme == Theme.System || IsRealTheme(SelectedTheme));

        theme = SelectedTheme == Theme.System
            ? systemTheme
            : SelectedTheme;
        Debug.Assert(IsRealTheme(theme));
    }

    public void Register(Window window)
    {
        window.Closed += WindowClosedHandler;
        windows.Add(window);
        if (window.IsLoaded)
        {
            UpdateWindow(window);
        }
        else
        {
            window.Loaded += (sender, ea) =>
            {
                UpdateWindow(window);
            };
        }
    }

    private void Unregister(Window window)
    {
        windows.Remove(window);
        window.Closed -= WindowClosedHandler;
    }

    private void WindowClosedHandler(object sender, EventArgs e) => Unregister((Window)sender);

    public void SetTheme(Theme theme, ThemeAccentColor accentColor)
    {
        this.appTheme = theme;
        this.accentColor = accentColor;
        Refresh();
    }

    public void Refresh()
    {
        ApplyTheme();
        foreach (var window in windows)
        {
            UpdateWindow(window);
        }
    }

    // https://github.com/lepoco/wpfui/issues/1188
    private void ApplyAccentColorBrushFix()
    {
        var converter = new ResourceReferenceExpressionConverter();
        var brushes = new Dictionary<string, SolidColorBrush>
        {
            ["SystemAccentColor"] = ((Color)UiApplication.Current.Resources["SystemAccentColor"]).ToBrush(),
            ["SystemAccentColorPrimary"] = ((Color)UiApplication.Current.Resources["SystemAccentColorPrimary"]).ToBrush(),
            ["SystemAccentColorSecondary"] = ((Color)UiApplication.Current.Resources["SystemAccentColorSecondary"]).ToBrush(),
            ["SystemAccentColorTertiary"] = ((Color)UiApplication.Current.Resources["SystemAccentColorTertiary"]).ToBrush()
        };
        ResourceDictionary themeDictionary = UiApplication.Current.Resources.MergedDictionaries[0];
        foreach (DictionaryEntry entry in themeDictionary)
        {
            if (entry.Value is SolidColorBrush brush)
            {
                var dynamicColor = brush.ReadLocalValue(SolidColorBrush.ColorProperty);
                if (dynamicColor is not Color &&
                    converter.ConvertTo(dynamicColor, typeof(MarkupExtension)) is DynamicResourceExtension dynamicResource &&
                    brushes.ContainsKey((string)dynamicResource.ResourceKey))
                {
                    themeDictionary[entry.Key] = brushes[(string)dynamicResource.ResourceKey];
                }
            }
        }
    }

    private void ApplyTheme()
    {
        SelectTheme();

        UpdateIconResources();

        var uiTheme = UiThemeFrom(theme);

        if (SelectedTheme == Theme.System)
        {
            ApplicationThemeManager.ApplySystemTheme(updateAccent: UseSystemAccent);
        }
        else
        {
            ApplicationThemeManager.Apply(
                uiTheme, Wpf.Ui.Controls.WindowBackdropType.Mica,
                updateAccent: UseSystemAccent);
        }

        if (!UseSystemAccent)
        {
            var paletteColor = Application.Current.FindResource($"Palette{accentColor}Color");
            if (paletteColor != null)
            {
                ApplicationAccentColorManager.Apply((Color)paletteColor, uiTheme);
            }
            else
            {
                ApplicationAccentColorManager.ApplySystemAccent();
            }
        }

        ApplyAccentColorBrushFix();
    }
}
