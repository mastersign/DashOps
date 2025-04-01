using System.Windows.Input;
using Mastersign.DashOps.Properties.Resources;

namespace Mastersign.DashOps
{
    public class UiCommands
    {
        public static RoutedUICommand EditProject { get; }
            = new RoutedUICommand(Common.Command_EditProject, nameof(EditProject), typeof(UiCommands));

        public static RoutedUICommand SwitchPerspective { get; }
            = new RoutedUICommand("Switch Perspective", nameof(SwitchPerspective), typeof(UiCommands));
    }
}
