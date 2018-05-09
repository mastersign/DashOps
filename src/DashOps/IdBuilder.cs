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
        public static string BuildId(string input)
        {
            var data = Encoding.UTF8.GetBytes(input);
            using (var md5 = new MD5CryptoServiceProvider())
            {
                var hash = md5.ComputeHash(data);
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }
    }
}
