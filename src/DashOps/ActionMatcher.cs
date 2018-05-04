using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mastersign.DashOps.Model
{
    partial class ActionMatcher
    {
        private bool MatchString(string value)
            => !string.IsNullOrWhiteSpace(value) &&
               (Pattern != null
                   ? System.Text.RegularExpressions.Regex.IsMatch(value, Pattern)
                   : string.Equals(value, Value));

        private string NormalizedFacette
        {
            get
            {
                if (string.Equals(
                    Facette, nameof(CommandAction.Verb),
                    StringComparison.InvariantCultureIgnoreCase))
                {
                    return nameof(CommandAction.Verb);
                }
                if (string.Equals(
                    Facette, nameof(CommandAction.Service),
                    StringComparison.InvariantCultureIgnoreCase))
                {
                    return nameof(CommandAction.Service);
                }
                if (string.Equals(
                    Facette, nameof(CommandAction.Host),
                    StringComparison.InvariantCultureIgnoreCase))
                {
                    return nameof(CommandAction.Host);
                }
                return Facette;
            }
        }

        public bool Match(ActionView actionView)
        {
            switch (Mode)
            {
                case ActionMatchMode.Description:
                    return MatchString(actionView.Description);
                case ActionMatchMode.Command:
                    return MatchString(actionView.ExpandedCommand);
                case ActionMatchMode.Facette:
                    return MatchString(actionView.GetFacetteValue(NormalizedFacette));
                case ActionMatchMode.Tag:
                    return actionView.Tags.Any(MatchString);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
