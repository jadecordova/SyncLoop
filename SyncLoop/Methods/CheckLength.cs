using SyncLoopLibrary;
using System.Windows;
using System.Windows.Documents;

namespace SyncLoop
{
    public partial class TextEditor : Window
    {
        /// <summary>
        /// Checks current and previous paragraphs' lengths and sets the paragraph background color accordingly
        /// to alert user.
        /// </summary>
        public void CheckLength()
        {
            if (DOCUMENT_LOADED)
            {
                if (Editor.Document != null)
                {
                    Paragraph previousParagraph = GetPreviousParagraph();

                    Paragraph currentParagraph = Editor.CaretPosition.Paragraph;

                    if (previousParagraph != null)
                    {
                        previousParagraph.Background = Utilities.CheckParagraphLength(previousParagraph);
                    }

                    if (currentParagraph != null)
                    {
                        currentParagraph.Background = Utilities.CheckParagraphLength(currentParagraph);
                    }
                }

            }
        }
    }
}