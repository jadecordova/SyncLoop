using System;
using System.Windows;
using System.Windows.Documents;

namespace SyncLoop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class TextEditor : Window
    {
        /// <summary>
        /// Updates the percentage label in the task bar.
        /// </summary>
        private void UpdateTaskBarLabel()
        {
            // Get total number of characters.
            int totalCharacters = GetNumberOfCharacters();

            // Get length to caret.
            TextPointer start = Editor.Document.ContentStart;

            TextPointer caret = Editor.CaretPosition;

            TextRange range = new TextRange(start, caret);

            int indexInText = range.Text.Length;
            
            // Calculate percentage.
            int p = (int)Math.Round(indexInText * 100d / totalCharacters);
            
            // Update label.
            Percentage.Text = $"{p.ToString():D2}%";
        }
    }
}
