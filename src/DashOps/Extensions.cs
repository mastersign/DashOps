using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Mastersign.DashOps
{
    internal static class Extensions
    {
        public static bool IsSuccessStatusCode(this HttpWebResponse httpWebResponse)
        {
            return ((int)httpWebResponse.StatusCode >= 200) && ((int)httpWebResponse.StatusCode <= 299);
        }
    }
}
