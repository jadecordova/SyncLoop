using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace SyncLoopLibrary
{
    public static partial class Utilities
    {
        /// <summary>
        /// Selects specified text and scrolls to show it.
        /// </summary>
        /// <param name="selection">Text to select.</param>
        /// <param name="editor">RichTextBox with the text to select.</param>
        public static void SelectText(TextRange selection, RichTextBox editor)
        {
            if (selection != null)
            {
                editor.Selection.Select(selection.Start, selection.End);
                // Rectangle corresponding to the coordinates of the selected text.
                Rect screenPos = editor.Selection.Start.GetCharacterRect(LogicalDirection.Forward);
                // Calculate offset to scroll to.
                double offset = screenPos.Top + editor.VerticalOffset;
                // The offset - half the size of the RichtextBox to keep the selection centered.
                editor.ScrollToVerticalOffset(offset - editor.ActualHeight / 2);
                // Set focus.
                editor.Focus();
            }
            else
            {
                MessageBox.Show("No more matches.");
            }
        }
    }
}
