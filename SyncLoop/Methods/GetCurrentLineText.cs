using System;
using System.Windows;
using System.Windows.Documents;

namespace SyncLoop
{
    public partial class TextEditor : Window
    {
        /// <summary>
        /// Gets the content of current line in editor. This will be used to potentially select
        /// the appropriate character automatically when opening the character selector dialog.
        /// </summary>
        /// <returns>Current line text.</returns>
        private string GetCurrentLineText()
        {
            if (Editor.CaretPosition.Paragraph != null)
            {
                return new TextRange(Editor.CaretPosition.Paragraph.ContentStart, Editor.CaretPosition.Paragraph.ContentEnd).Text;
            }

            return String.Empty;
        }
    }
}
