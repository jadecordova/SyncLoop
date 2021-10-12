using System.Windows;
using System.Windows.Documents;

namespace SyncLoop
{

    public partial class TextEditor : Window
    {

        /// <summary>
        /// Inserts specific shortcut into text editor.
        /// </summary>
        /// <param name="shortcut">Index of shortcut to insert.</param>
        public void InsertShortcut(int shortcut)
        {
            // Get current caret position.
            TextPointer insertionPosition = Editor.CaretPosition;
         
            // if it is not at an insertion point...
            if (!Editor.CaretPosition.IsAtInsertionPosition)
            {
                // Get next insertion point.
                insertionPosition = Editor.CaretPosition.GetInsertionPosition(LogicalDirection.Forward);
            }
            
            // Get insertion position.
            Editor.CaretPosition = Editor.CaretPosition.GetPositionAtOffset(0, LogicalDirection.Forward);

            // Insert text.
            Editor.CaretPosition.InsertTextInRun(shortcuts[shortcut]);
            
            // Focus editor.
            Editor.Focus();
        }
    }
}
