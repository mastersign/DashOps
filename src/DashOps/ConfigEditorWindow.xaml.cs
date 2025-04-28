using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;
using UI = Wpf.Ui.Controls;

namespace Mastersign.DashOps;

public partial class ConfigEditorWindow : UI.FluentWindow
{
    private static ConfigEditorWindow instance;

    public static void Open(string title, string filename, string schemaName)
    {
        if (instance is null) 
        { 
            instance = new(); 
        }
        instance.Title = title;
        instance.filename = filename;
        instance.schemaName = schemaName;

        instance.Show();
        if (instance.WindowState == WindowState.Minimized)
        {
            instance.WindowState = WindowState.Normal;
        }
    }

    public static void CloseInstance()
    {
        instance?.Close();
    }

    private string schemaName;
    private string filename;

    public ConfigEditorWindow()
    {
        InitializeComponent();
        editor.Visibility = Visibility.Hidden;
    }

    private static string GetTextResource(string path)
    {
        var ns = typeof(ConfigEditorWindow).Namespace;
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
            Path.GetFileName(filename));

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
        await Save();
    }

    private async void ApplicationCommandSaveHandler(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
    {
        await Save();
    }
}
