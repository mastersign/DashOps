using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mastersign.DashOps
{
    public interface IExecutable
    {
        string CommandId { get; }

        string Logs { get; }

        string Command { get; }

        string Arguments { get; }

        string WorkingDirectory { get; }

        string CurrentLogFile { get; set; }

        bool Visible { get; }

        void NotifyExecutionFinished();
    }
}
