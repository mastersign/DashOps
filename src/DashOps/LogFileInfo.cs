using System;

namespace Mastersign.DashOps
{
    public class LogFileInfo
    {
        public readonly string FileName;
        public readonly DateTime Timestamp;
        public readonly string ActionId;
        public readonly bool HasResult;
        public readonly bool Success;
        public readonly int ExitCode;

        public LogFileInfo(string fileName, DateTime timestamp, string actionId, bool hasResult, bool success, int exitCode)
        {
            FileName = fileName;
            Timestamp = timestamp;
            ActionId = actionId;
            HasResult = hasResult;
            Success = success;
            ExitCode = exitCode;
        }
    }
}
