using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Mastersign.DashOps
{
    partial class ActionView
    {
        private static MD5 md5 = MD5.Create();

        public bool HasFacette(string name) 
            => this.Facettes?.ContainsKey(name) ?? false;

        public string[] GetFacettes()
            => this.Facettes?.Keys.ToArray() ?? Array.Empty<string>();

        public bool HasFacetteValue(string name, string value)
            => this.Facettes?.ContainsKey(name) ?? false
                && string.Equals(this.Facettes[name], value);

        public string GetFacetteValue(string name)
            => this.Facettes != null
                ? this.Facettes.TryGetValue(name, out string value) ? value : null
                : null;

        public string CommandLabel => ExpandedCommand
            + (string.IsNullOrWhiteSpace(ExpandedArguments) 
                ? string.Empty 
                : " " + ExpandedArguments);

        public string ExpandedCommand => Environment.ExpandEnvironmentVariables(Command);

        public string ExpandedArguments => CommandLine.FormatArgumentList(Arguments.Select(Environment.ExpandEnvironmentVariables).ToArray());

        public string ActionId
        {
            get
            {
                var str = Command + " " + CommandLine.FormatArgumentList(Arguments);
                var data = Encoding.UTF8.GetBytes(str);
                var hash = md5.ComputeHash(data);
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }

        public override string ToString() => $"[{ActionId}] {Description}: {CommandLabel}";

        public string LogNamePattern => $"*_{ActionId}.log";

        public string CreateLogName() => $"{DateTime.Now:yyyyMMdd_HHmmss}_{ActionId}.log";
    }
}
