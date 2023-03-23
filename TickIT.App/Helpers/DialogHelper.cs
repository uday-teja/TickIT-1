using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.Controls;
using System.Threading.Tasks;
using System.Windows;

namespace TickIT.App.Helpers
{
    public class DialogHelper
    {
        public static async Task<bool> ShowMessageDialog(string title, string message, MessageDialogStyle style = MessageDialogStyle.Affirmative)
        {
            if (MessageDialogResult.Affirmative == await (Application.Current.MainWindow as MetroWindow).ShowMessageAsync(title, message, style))
                return true;
            return false;
        }
    }
}
