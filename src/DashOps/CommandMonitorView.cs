using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mastersign.DashOps
{
    partial class CommandMonitorView
    {
        public override Task<bool> Check()
        {
            var t = new Task<bool>(() => {
                throw new NotImplementedException();
            });
            t.Start();
            return t;
        }
    }
}
