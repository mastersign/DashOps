using System.Text.RegularExpressions;

namespace Mastersign.DashOps;

internal class DesignTimeCommandMonitorView : CommandMonitorView
{
    public DesignTimeCommandMonitorView()
    {
        Title = "Demo Command Monitor";

        Interval = TimeSpan.FromSeconds(123456789);
        IsRunning = false;
        LastExecutionTime = DateTime.Now;
        HasLastExecutionResult = true;
        HasExecutionResultChanged = true;
        LastExecutionResult = true;

        Command = @"C:\Program Files (x86)\Some Vendor\My Application\bin\MyApplication.exe";
        Arguments = @"--verbose --input-file ""D:\Massive Data Storage\Project XYZ\resources\my-file.bin"" --option-a A --option-b B";
        WorkingDirectory = @"D:\Massive Data Storage\Project XYZ\assembly";
        Environment = new Dictionary<string, string>
        {
            { "SOME_IMPORTANT_VARIABLE", "And a rather lengthy value for demonstartive purposes" },
            { "EDITOR", "notepad.exe" },
        };

        ExePaths = [];
        ExitCodes = [0];

        UsePowerShellCore = false;
        PowerShellExe = @"C:\Program Files\PowerShell\7\pwsh.exe";
        UsePowerShellProfile = false;
        PowerShellExecutionPolicy = "RemoteSigned";

        RequiredPatterns = [];
        ForbiddenPatterns = [new Regex("err(or)?", RegexOptions.IgnoreCase)];

        Logs = @"D:\Massive Data Storage\Project XYZ\logs";
        NoLogs = false;
        CurrentLogFile =
            @"D:\Massive Data Storage\Project XYZ\logs\"
            + DateTime.Now.ToString("yyyyMMdd_HHmmss")
            + "_" + CommandId
            + ".log";

        Tags = ["development", "EXAMPLE"];
    }
}
