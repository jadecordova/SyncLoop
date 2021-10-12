using System;
using System.Windows;
using System.Windows.Input;

namespace SyncLoop
{
    public partial class TextEditor : Window
    {
        private void Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(TextFileName))
            {
                SaveTextFile(TextFileName);
            }
            else
            {
                SaveAs_Executed(null, null);
            }
        }
    }
}
