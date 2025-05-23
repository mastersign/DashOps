﻿using System.ComponentModel;
using System.IO;
using System.Reflection;
using Mastersign.DashOps.ViewModel;
using Mastersign.WpfCodeEditor;
using Screen = System.Windows.Forms.Screen;
using UI = Wpf.Ui.Controls;

namespace Mastersign.DashOps.Windows;

public partial class ConfigEditorWindow : UI.FluentWindow
{
    private static bool IsWebView2Available()
    {
        try
        {
            Microsoft.Web.WebView2.Core.CoreWebView2Environment.GetAvailableBrowserVersionString();
            return true;
        }
        catch
        {
            return false;
        }
    }

    private const string WEBVIEW2_DOWNLOAD_URL = "https://developer.microsoft.com/de-de/microsoft-edge/webview2";

    private static void ShowWebView2NotFoundMessage()
    {
        var msgContent = new StackPanel();
        msgContent.Children.Add(new TextBlock
        {
            Text = Properties.Resources.Common.WebView2_NotFound,
        });
        msgContent.Children.Add(new UI.HyperlinkButton
        {
            Margin = new Thickness(0, 8, 0, 0),
            NavigateUri = WEBVIEW2_DOWNLOAD_URL,
            Content = WEBVIEW2_DOWNLOAD_URL,
        });
        UserInteraction.ShowMessage(
            title: Properties.Resources.Common.WebView2_NotFound_Title,
            message: msgContent,
            symbol: InteractionSymbol.Warning,
            showInTaskbar: true,
            owner: Application.Current.MainWindow);
    }

    private static ConfigEditorWindow instance;
    private static bool isExiting;

    public static Window Open(string title, string filename, string schemaName, bool lastTime = false)
    {
        if (!IsWebView2Available())
        {
            ShowWebView2NotFoundMessage();
            return null;
        }

        if (lastTime)
        {
            isExiting = true;
        }
        if (instance is null)
        {
            instance = new();
        }
        instance.Title = title;
        instance.filename = filename;
        instance.schemaName = schemaName;

        instance.SetWindowPosition();
        instance.Show();
        if (instance.WindowState == WindowState.Minimized)
        {
            instance.WindowState = WindowState.Normal;
        }
        return instance;
    }

    private ProjectView Project => App.Instance.ProjectLoader?.ProjectView;

    private void SetWindowPosition()
    {
        var ws = Project?.EditorWindow;
        if (ws is null) return;

        var screen = ws.ScreenNo.HasValue && ws.ScreenNo.Value >= 0 && ws.ScreenNo.Value < Screen.AllScreens.Length
            ? Screen.AllScreens[ws.ScreenNo.Value]
            : Screen.PrimaryScreen;

        var mainWindow = App.Instance.MainWindow;

        if (ws.Mode == WindowMode.Fixed)
        {
            var x = screen.WorkingArea.Left + ws.Left ?? 0;
            var y = screen.WorkingArea.Top + ws.Top ?? 0;
            Left = x;
            Top = y;
            if (ws.Width.HasValue) Width = Math.Min(screen.WorkingArea.Width - x, ws.Width.Value);
            if (ws.Height.HasValue) Height = Math.Min(screen.WorkingArea.Height - y, ws.Height.Value);
        }
        else if (ws.Mode == WindowMode.Auto)
        {
            var screen2 = Screen.FromPoint(new System.Drawing.Point(
                (int)mainWindow.Left + (int)mainWindow.Width, 
                (int)mainWindow.Top));
            var sx = screen2.WorkingArea.Left + screen2.WorkingArea.Width - (int)mainWindow.Left - (int)mainWindow.Width;
            var w = Math.Min(sx, ws.Width ?? 800);
            if (w >= 540)
            {
                Left = mainWindow.Left + mainWindow.Width;
                Top = mainWindow.Top;
                Width = w;
                Height = Math.Min(screen2.WorkingArea.Height - (int)mainWindow.Top, ws.Height ?? 960);
            }
            else
            {
                Left = screen.WorkingArea.Left;
                Top = screen.WorkingArea.Top;
                Width = Math.Min(screen.WorkingArea.Width, ws.Width ?? 800);
                Height = ws.Height.HasValue
                    ? Math.Min(screen.WorkingArea.Height, ws.Height.Value)
                    : screen.WorkingArea.Height switch
                        {
                            > 1280 => 960,
                            _ => screen.WorkingArea.Height,
                        };
            }
        }
        else
        {
            if (ws.Width.HasValue) Width = ws.Width.Value;
            if (ws.Height.HasValue) Height = ws.Height.Value;
        }
    }

    public static void CloseInstance()
    {
        isExiting = true;
        instance?.Close();
    }

    private string schemaName;
    private string filename;

    private static ColorScheme ColorSchemeFromAppTheme(WpfUiTools.Theme theme)
        => theme switch
        {
            WpfUiTools.Theme.Light => ColorScheme.Light,
            WpfUiTools.Theme.Dark => ColorScheme.Dark,
            _ => ColorScheme.Auto,
        };

    public ConfigEditorWindow()
    {
        InitializeComponent();
        editor.Visibility = Visibility.Hidden;
        editor.Configuration.Theme = ColorSchemeFromAppTheme(App.Instance.ThemeManager.SelectedTheme);
        App.Instance.ThemeManager.Register(this);
        Loaded += (sender, ea) =>
        {
            App.Instance.ThemeManager.SelectedThemeChanged += SelectedThemeChangedHandler;
            if (App.Instance.ThemeManager.MainWindow is null)
            {
                App.Instance.ThemeManager.MainWindow = this;
            }
        };
        Unloaded += (sender, ea) =>
        {
            App.Instance.ThemeManager.SelectedThemeChanged -= SelectedThemeChangedHandler;
        };
    }

    private void SelectedThemeChangedHandler(object sender, EventArgs e)
    {
        editor.Configuration.Theme = ColorSchemeFromAppTheme(App.Instance.ThemeManager.SelectedTheme);
        editor.UpdateColorScheme();
    }

    private static string GetTextResource(string path)
    {
        var ns = typeof(App).Namespace;
        var resPath = ns + "." + path.Replace('/', '.');
        using var s = Assembly.GetExecutingAssembly().GetManifestResourceStream(resPath);
        using var r = new StreamReader(s, Encoding.UTF8);
        return r.ReadToEnd();
    }

    private async void EditorReadyHandler(object sender, EventArgs e)
    {
        await editor.LoadJsonSchemaAsync(
            GetTextResource($"resources/{schemaName}.schema.json"),
            $"https://mastersign.de/{schemaName}.schema.json");

        await editor.LoadTextAsync(
            File.ReadAllText(filename, Encoding.UTF8),
            "yaml",
            "dashops.yaml");

        editor.Visibility = Visibility.Visible;
        editor.InvalidateVisual();
    }

    private async Task Save()
    {
        var text = await editor.GetTextAsync();
        var encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
        File.WriteAllText(filename, text, encoding);
    }

    private async void WindowClosingHandler(object sender, CancelEventArgs e)
    {
        if (!isExiting)
        {
            e.Cancel = true;
            Visibility = Visibility.Collapsed;
        }
        await Save();
    }

    private async void ApplicationCommandSaveHandler(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
    {
        await Save();
    }
}
