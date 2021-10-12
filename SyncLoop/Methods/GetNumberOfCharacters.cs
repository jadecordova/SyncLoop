using System.Windows;
using System.Windows.Documents;

namespace SyncLoop
{
    public partial class TextEditor : Window
    {
        /// <summary>
        /// Gets the total number of characters in document.
        /// Used to calculate percentage of completion.
        /// </summary>
        /// <returns>Number of characters.</returns>
        private int GetNumberOfCharacters()
        {
            return new TextRange(Editor.Document.ContentStart, Editor.Document.ContentEnd).Text.Length;
        }
    }
}
