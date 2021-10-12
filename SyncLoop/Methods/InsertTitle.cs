using SyncLoopLibrary;
using System.Windows;
using System.Windows.Documents;

namespace SyncLoop
{

    public partial class TextEditor : Window
    {

        /// <summary>
        /// Inserts character title information into document.
        /// </summary>
        public void InsertTitle()
        {
            // Get current editor line.
            string characterToLook = GetCurrentLineText();

            // Select character.
            Character selectedCharacter = SelectCharacter(characterToLook);

            // If character is valid...
            if (selectedCharacter != null)
            {
                // Get title name. Might be differente from character name.
                string characterTitleName = selectedCharacter.TitleName;

                // Get title string.
                string characterTitle = selectedCharacter.Title;
                
                // Get current caret position.
                TextPointer insertionPosition = Editor.CaretPosition;
                
                // if it is not at an insertion point...
                if (!Editor.CaretPosition.IsAtInsertionPosition)
                {
                    // Get next insertion point.
                    insertionPosition = Editor.CaretPosition.GetInsertionPosition(LogicalDirection.Forward);
                }
                
                // Create title name.
                Paragraph nameLoop = new Paragraph();

                Run titleNameRun = new Run(characterTitleName);

                nameLoop.Inlines.Add(titleNameRun);
                
                // Create title.
                Paragraph titleLoop = new Paragraph();

                Run titleRun = new Run(characterTitle);

                titleLoop.Inlines.Add(titleRun);

                Editor.Document.Blocks.InsertBefore(insertionPosition.Paragraph, nameLoop);
                
                // Reset caret position.
                insertionPosition = Editor.CaretPosition;

                Editor.Document.Blocks.InsertBefore(insertionPosition.Paragraph, titleLoop);

                Editor.Focus();
            }
        }
    }
}