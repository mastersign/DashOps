using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using UI = Wpf.Ui.Controls;

namespace Mastersign.DashOps;

internal static class UserInteraction
{
    public static void ShowMessage(
        string title,
        object message,
        InteractionSymbol symbol = InteractionSymbol.None,
        bool showInTaskbar = false
    ) {
        new UI.MessageBox
        {
            MaxWidth = 1024,
            Title = title,
            Content = BuildContent(message, symbol),
            CloseButtonText = "OK",
            Icon = new BitmapImage(new Uri("pack://application:,,,/DashOps;component/icon.ico")),
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner,
            ShowInTaskbar = showInTaskbar,
        }.ShowDialogAsync().Wait();
    }
    
    public static bool AskYesOrNoQuestion(
        string title,
        object question,
        InteractionSymbol symbol = InteractionSymbol.Question,
        bool showInTaskbar = false)
    {
        var msgBox = new UI.MessageBox
        {
            MaxWidth = 1024,
            Title = title,
            Content = BuildContent(question, symbol),
            PrimaryButtonText = "Yes",
            IsPrimaryButtonEnabled = true,
            CloseButtonText = "No",
            Icon = new BitmapImage(new Uri("pack://application:,,,/DashOps;component/icon.ico")),
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner,
            ShowInTaskbar = showInTaskbar,
        };
        return msgBox.ShowDialogAsync().Result == UI.MessageBoxResult.Primary;
    }

    private static object BuildContent(object content, InteractionSymbol symbol)
    {
        if (symbol == InteractionSymbol.None) return content;

        var grid = new Grid
        {
            Margin = new Thickness(12),
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = GridLength.Auto },
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
            },
        };
        grid.Children.Add(new ContentPresenter
        {
            Width = 64,
            Height = 64,
            Margin= new Thickness(0, 0, 20, 0),
            VerticalAlignment = VerticalAlignment.Top,
            Content = new UI.SymbolIcon()
            {
                FontSize = 48,
                Filled = true,
                Symbol = MapSymbol(symbol),
                Foreground = MapColor(symbol),
            },
        });
        if (content is string text)
        {
            var textBlock = new TextBlock
            {
                Text = text,
            };
            Grid.SetColumn(textBlock, 1);
            grid.Children.Add(textBlock);
        }
        else if (content is UIElement element)
        {
            Grid.SetRow(element, 0);
            Grid.SetColumn(element, 1);
            grid.Children.Add(element);
        }
        return grid;
    }

    private static UI.SymbolRegular MapSymbol(InteractionSymbol symbol)
        => symbol switch
        {
            InteractionSymbol.Info => UI.SymbolRegular.Info24,
            InteractionSymbol.Warning => UI.SymbolRegular.Warning24,
            InteractionSymbol.Error => UI.SymbolRegular.ErrorCircle24,
            InteractionSymbol.Success => UI.SymbolRegular.CheckmarkCircle24,
            InteractionSymbol.Question => UI.SymbolRegular.QuestionCircle24,
            InteractionSymbol.Reassurance => UI.SymbolRegular.ShieldQuestion24,
            _ => throw new NotSupportedException(),
        };

    private static Brush MapColor(InteractionSymbol symbol)
        => symbol switch
        {
            InteractionSymbol.Info => Res<Brush>("PaletteLightBlueBrush"),
            InteractionSymbol.Warning => Res<Brush>("PaletteOrangeBrush"),
            InteractionSymbol.Error => Res<Brush>("PaletteRedBrush"),
            InteractionSymbol.Success => Res<Brush>("PaletteGreenBrush"),
            InteractionSymbol.Question => Res<Brush>("PaletteLightBlueBrush"),
            InteractionSymbol.Reassurance => Res<Brush>("PaletteOrangeBrush"),
            _ => throw new NotSupportedException(),
        };

    private static T Res<T>(string name)
        where T : class
        => Application.Current.Resources[name] as T;
}

internal enum InteractionSymbol
{
    None,
    Info,
    Warning,
    Error,
    Success,
    Question,
    Reassurance,
}