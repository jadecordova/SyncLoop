using System.Windows;
using System.Windows.Input;

namespace SyncLoop
{
    public partial class TextEditor : Window
    {
        private void CheckLength_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = DOCUMENT_LOADED;
        }

        private void CheckLength_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CheckLength();
        }
    }
}
