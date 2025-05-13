using UI = Wpf.Ui.Controls;

namespace Mastersign.DashOps;

partial class CopyButton : UI.Button
{
    public static readonly DependencyProperty CopyTextProperty =
        DependencyProperty.Register(nameof(CopyText), typeof(string), typeof(CopyButton),
        new PropertyMetadata(null, new PropertyChangedCallback(ChangeCopyText)));

    private string copyText;

    private static void ChangeCopyText(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var btn = (CopyButton)d;
        btn.copyText = e.NewValue as string;
        btn.IsEnabled = e.NewValue is not null;
    }

    public string CopyText
    {
        get => GetValue(CopyTextProperty) as string;
        set => SetValue(CopyTextProperty, value);
    }

    public CopyButton()
    {
        InitializeComponent();
    }

    protected override void OnClick()
    {
        if (copyText is not null)
        {
            Clipboard.SetText(copyText);
        }
        base.OnClick();
    }
}
