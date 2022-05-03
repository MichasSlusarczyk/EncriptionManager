using System.Windows;

namespace EWMS.ViewModels
{
    internal class DisplayMessageBox
    {
        public static void DisplayMessage(string message)
        {
            MessageBox.Show(message, "Warning:", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
        }

        public static void DisplayMessage(Window window, string message)
        {
            MessageBox.Show(Window.GetWindow(window), message, "Warning:", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
        }
    }
}
