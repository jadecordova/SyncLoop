using SyncLoopLibrary;
using System;
using System.Windows;
using System.Windows.Documents;

namespace SyncLoop
{
    public partial class TextEditor : Window
    {
        /// <summary>
        /// Find the next valid loop paragraph in document.
        /// </summary>
        /// <returns>TextPointer to next loop</returns>
        public TextPointer FindNextLoop()
        {
            // Result position.
            TextPointer result = null;

            // Flag to find blank paragraph.
            bool SEPARATOR_FOUND = false;

            // If we have a text file without blanks, we must have an alternative method of ending the loop,
            // so we establish a maximum of two paragraphs for subtitles, since there can be only two lines per subtitle.
            int maximumParagraphsToCount = 2;

            // And we create a counter for that.
            int paragraphsFound = 0;

            if (Editor.CaretPosition != null)
            {
                // Loop thorugh paragraphs.
                while (!SEPARATOR_FOUND && paragraphsFound < maximumParagraphsToCount)
                {
                    // Get next paragaph.
                    Paragraph paragraph = GetNextParagraph();

                    if (paragraph != null)
                    {
                        // Move caret.
                        Editor.CaretPosition = paragraph.ContentStart;

                        // Get text.
                        string text = new TextRange(paragraph.ContentStart, paragraph.ContentEnd).Text;
                        
                        // Check for blank paragraph.
                        if (string.IsNullOrWhiteSpace(text) || string.IsNullOrEmpty(text))
                        {
                            SEPARATOR_FOUND = true;
                        }
                        
                        // Update the counter.
                        paragraphsFound++;
                    }
                    else
                    {
                        break;
                    }
                }

                if (SEPARATOR_FOUND || paragraphsFound > 0)
                {
                    Paragraph paragraph = GetNextParagraph();

                    if (paragraph != null)
                    {
                        result = paragraph.ContentStart;
                    }
                }
            }

            return result;
        }
    }
}
