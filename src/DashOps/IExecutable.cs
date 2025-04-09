namespace Mastersign.DashOps
{
    public interface IExecutable : ILogged
    {
        bool UsePowerShellCore { get; }

        string PowerShellExe { get; }

        bool UsePowerShellProfile { get; }

        string PowerShellExecutionPolicy { get; }

        string Command { get; }

        string Arguments { get; }

        string WorkingDirectory { get; }

        Dictionary<string, string> Environment { get; }

        bool UseWindowsTerminal { get; }

        string WindowsTerminalArguments { get; }

        int[] ExitCodes { get; }

        string CurrentLogFile { get; set; }

        bool PrintExecutionInfo { get; }

        string Title { get; }

        bool Visible { get; }

        bool KeepOpen { get; }

        bool AlwaysClose { get; }

        Func<string, bool> SuccessCheck { get; }

        void NotifyExecutionFinished();
    }
}
