using System.Windows;
using System.Windows.Input;

namespace SyncLoop
{
    public partial class TextEditor : Window
    {
        private void FindAndReplace_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = DOCUMENT_LOADED;
        }

        private void FindAndReplace_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // Get editor selection.
            string selection = Editor.Selection.Text;

            // Create dialog.
            FindAndReplace fr = new FindAndReplace(Editor, selection);

            fr.Show();
        }
    }
}
