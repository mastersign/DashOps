namespace Mastersign.DashOps;

/// <summary>
/// Interaktionslogik für CompactMonitorControl.xaml
/// </summary>
public partial class CompactMonitorControl : UserControl
{
    public CompactMonitorControl()
    {
        InitializeComponent();
    }

    private void ShowMonitorInfo()
    {
        var monitor = DataContext as MonitorView;
        if (monitor is null) return;
        DashOpsCommands.ShowMonitorInfo.Execute(monitor);
    }

    private async Task CheckMonitor()
    {
        var monitor = DataContext as MonitorView;
        if (monitor is null) return;
        if (monitor.IsRunning) return;
        await monitor.Check(DateTime.Now);
    }

    private void ClickHandler(object sender, EventArgs e)
    {
        ShowMonitorInfo();
    }

    private async void DoubleClickHandler(object sender, EventArgs e)
    {
        await CheckMonitor();
    }
}
