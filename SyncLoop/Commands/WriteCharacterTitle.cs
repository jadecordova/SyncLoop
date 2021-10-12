using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace SyncLoop
{
    public partial class TextEditor : Window
    {
        private void WriteCharacterTitle_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void WriteCharacterTitle_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (Editor.CaretPosition.Paragraph != null)
            {
                InsertTitle();
            }
        }
    }
}
