using System.Windows;
using System.Windows.Documents;

namespace SyncLoop
{
    public partial class TextEditor : Window
    {
        /// <summary>
        /// Gets the previous paragraph. This function is used to get the previous paragraph
        /// when spliting a paragraph due to excesive length. This way, it is possible to know if the previous
        /// paragraph is still too large.
        /// </summary>
        /// <returns>Previous paragraph object.</returns>
        public Paragraph GetNextParagraph()
        {
            if (Editor.CaretPosition != null)
            {
                // Return object.
                Paragraph result = null;

                // Get current paragraph.
                Paragraph position = Editor.CaretPosition.Paragraph;
                
                // Get next paragraph. NextBlock returns the next sibling, so in this case it should be the next paragraph.
                if(position != null)
                {
                    result = (Paragraph)position.NextBlock;
                }

                return result;

            }

            return null;
        }
    }
}
