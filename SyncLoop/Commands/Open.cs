using System.Windows;
using System.Windows.Input;

namespace SyncLoop
{
    public partial class TextEditor : Window
    {
        private void Open_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Content Files (*.txt,*.rtf,*.xaml)|*.txt;*.rtf;*.xaml|All files|*.*",
                RestoreDirectory = true
            };

            // If the user selected a file, we set the path variable.
            if (dialog.ShowDialog() == true)
            {
                OpenTextFile(dialog.FileName);
            }
            
            // Open program info dialog.
            GetProgramInfo();
        }
    }
}
