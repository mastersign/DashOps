using Mastersign.DashOps.ViewModel;

namespace Mastersign.DashOps;

public partial class CommandMonitorInfoControl : UserControl
{
    public CommandMonitorInfoControl()
    {
        InitializeComponent();
    }

    private async void CheckMonitorHandler(object sender, EventArgs e)
    {
        var monitor = DataContext as MonitorView;
        if (monitor is null) return;
        if (monitor.IsRunning) return;
        await monitor.Check(DateTime.Now);
    }
}
