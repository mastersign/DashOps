using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mastersign.DashOps
{
    partial class MonitorView
    {
        public virtual Task<bool> Check()
        {
            throw new NotImplementedException();
        }

        protected virtual void NotifyExecutionBegin()
        {
            LastExecutionTime = DateTime.Now;
            IsRunning = true;
        }

        protected virtual void NotifyExecutionFinished(bool success)
        {
            LastExecutionResult = success;
            HasLastExecutionResult = true;
            IsRunning = false;
        }
    }
}
