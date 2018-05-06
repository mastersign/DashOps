using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Mastersign.DashOps
{
    static class IdBuilder
    {
        private static readonly MD5 Md5 = MD5.Create();

        public static string BuildId(string input)
        {
            var data = Encoding.UTF8.GetBytes(input);
            var hash = Md5.ComputeHash(data);
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }
    }
}
