using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace SyncLoop
{
    public partial class TextEditor : Window
    {
        private void EditCharacter_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void EditCharacter_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (Editor.CaretPosition.Paragraph != null)
            {
                SelectCharacter(new TextRange(Editor.CaretPosition.Paragraph.ContentStart, Editor.CaretPosition.Paragraph.ContentEnd).Text);
            }
        }
    }
}
