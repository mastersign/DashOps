using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mastersign.DashOps
{
    partial class ActionView
    {
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

        public string Label => Command + " " + string.Join(" ", Arguments);
    }
}
