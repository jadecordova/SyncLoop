using System.Windows;
using System.Windows.Input;

namespace SyncLoop
{
    public partial class TextEditor : Window
    {
        private void LoadCharacters_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void LoadCharacters_Executed(object sender, ExecutedRoutedEventArgs e)
        {            
            // Select file.
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Json Files (*.json)|*.json|All files|*.*",
                RestoreDirectory = true
            };

            if(dialog.ShowDialog() == true)
            {
                // Load characters from disk.
                LoadCharacters(dialog.FileName);
            }
        }
    }
}
