using SyncLoopLibrary;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace SyncLoop
{
    public partial class TextEditor : Window
    {
        private void ScrollToLine_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ScrollToLine_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // Rectangle corresponding to the coordinates of the selected text.
            Rect screenPos = Editor.Selection.Start.GetCharacterRect(LogicalDirection.Forward);

            // Set offset.
            double offset = screenPos.Top + Editor.VerticalOffset - Settings.ApplicationSettings.SubtitlesScrollOffset; 

            // The offset - half the size of the RichtextBox to keep the selection centered.
            Editor.ScrollToVerticalOffset(offset);
        }
    }
}
