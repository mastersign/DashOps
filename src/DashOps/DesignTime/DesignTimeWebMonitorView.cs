using System.Text.RegularExpressions;
using Mastersign.DashOps.ViewModel;

namespace Mastersign.DashOps.DesignTime;

internal class DesignTimeWebMonitorView : WebMonitorView
{
    public DesignTimeWebMonitorView()
    {
        Title = "Demo Web Monitor";

        Interval = TimeSpan.FromSeconds(123456789);
        IsRunning = false;
        LastExecutionTime = DateTime.Now;
        HasLastExecutionResult = true;
        HasExecutionResultChanged = true;
        LastExecutionResult = true;

        Url = "https://my-demo-web-service.my-domain.com/alive-endpoint?m=dashops";
        Headers = new Dictionary<string, string>
        {
            { "User-Agent", "Custom Monitoring" },
            { "Accept", "text/html" },
        };
        Timeout = TimeSpan.FromSeconds(12.34);
        ServerCertificateHash = "096475e7ff5127e1381671e8af08203b49f145429d079a7e72a210de101e1e1b";
        NoTlsVerify = false;
        StatusCodes = [200, 201];
        RequiredPatterns = [
            new Regex(@"<div\s([^>]*\s)?class=[""'].*important.*[""']"),
            new Regex(@"My Monitored Application"),
        ];
        ForbiddenPatterns = [];

        Logs = @"D:\Massive Data Storage\Project XYZ\logs";
        NoLogs = false;
        CurrentLogFile =
            @"D:\Massive Data Storage\Project XYZ\logs\"
            + DateTime.Now.ToString("yyyyMMdd_HHmmss")
            + "_" + CommandId
            + "_OK_200.log";

        Tags = [];
    }
}
