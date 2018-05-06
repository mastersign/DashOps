using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Mastersign.DashOps
{
    public static class Core
    {
        public static void RefreshProject()
            => App.Instance.ProjectLoader.ReloadProjectAndProjectView();

        public static async Task ExecuteAction(ActionView action)
        {
            if (!action.Reassure || Reassure(action))
            {
                await action.ExecuteAsync();
                DashOpsCommands.ShowLastActionLog.RaiseCanExecuteChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private static bool Reassure(ActionView action)
        {
            var result = MessageBox.Show(
                "Are you sure you want to execute the following action?\n\n" + ActionInfo(action),
                "Execute Action", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            return result == MessageBoxResult.OK;
        }

        public static void ShowActionInfo(ActionView action)
        {
            MessageBox.Show(ActionInfo(action), "Action Info",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private static string ActionInfo(ActionView action)
        {
            var sb = new StringBuilder();
            sb.AppendLine(action.Description);
            sb.AppendLine();
            sb.Append("ID: ");
            sb.AppendLine(action.CommandId);
            sb.Append("Command: ");
            sb.AppendLine(action.Command);
            if (!string.IsNullOrWhiteSpace(action.Arguments))
            {
                sb.Append("Arguments: ");
                sb.AppendLine(action.Arguments);
            }
            sb.Append("Working Directory: ");
            sb.AppendLine(action.WorkingDirectory);
            return sb.ToString();
        }

        public static void ShowLastActionLog(ActionView action)
            => ShowLogFile(action.LastLogFile);

        public static void ShowLogFile(string filepath)
            => System.Diagnostics.Process.Start(filepath);
    }
}
