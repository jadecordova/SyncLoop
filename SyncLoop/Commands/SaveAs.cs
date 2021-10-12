using System.Windows;
using System.Windows.Input;

namespace SyncLoop
{
    public partial class TextEditor : Window
    {
        private void SaveAs_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void SaveAs_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "Text File (*.txt)|*.txt|RTF File (*.rtf)|*.rtf|XAML File (*.xaml)|*.xaml|All files|*.*",
                RestoreDirectory = true
            };

            // If user accepts...
            if (dialog.ShowDialog() == true)
            {
                SaveTextFile(dialog.FileName);
            }

            // Set original file name.
            TextFileName = dialog.FileName;
        }
    }
}
