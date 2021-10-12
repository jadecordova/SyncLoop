using System.Windows;
using System.Windows.Input;

namespace SyncLoop
{
    public partial class TextEditor : Window
    {
        private void Shortcut1_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Shortcut1_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            InsertShortcut(0);
        }

        private void Shortcut2_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Shortcut2_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            InsertShortcut(1);
        }

        private void Shortcut3_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Shortcut3_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            InsertShortcut(2);
        }

        private void Shortcut4_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Shortcut4_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            InsertShortcut(3);
        }

        private void Shortcut5_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Shortcut5_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            InsertShortcut(4);
        }

        private void Shortcut6_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Shortcut6_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            InsertShortcut(5);
        }

        private void Shortcut7_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Shortcut7_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            InsertShortcut(6);
        }

        private void Shortcut8_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Shortcut8_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            InsertShortcut(7);
        }
    }
}
