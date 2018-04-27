using System;

namespace Mastersign.DashOps
{
    public class LogFileInfo
    {
        public readonly string FileName;
        public readonly DateTime Timestamp;
        public readonly string ActionId;
        public readonly bool HasExitCode;
        public readonly int ExitCode;

        public LogFileInfo(string fileName, DateTime timestamp, string actionId, bool hasExitCode, int exitCode)
        {
            FileName = fileName;
            Timestamp = timestamp;
            ActionId = actionId;
            HasExitCode = hasExitCode;
            ExitCode = exitCode;
        }

        public bool IsSuccess => HasExitCode && ExitCode == 0;
    }
}
