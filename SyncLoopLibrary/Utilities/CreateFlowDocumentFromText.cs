using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace SyncLoopLibrary
{
    public static partial class Utilities
    {

        /// <summary>
        /// Creates flow document from a text string.
        /// </summary>
        /// <returns>Flow document.</returns>
        public static FlowDocument CreateFlowDocumentFromText(string text)
        {
            // Create return document.
            FlowDocument document = null;

            // Check for valid string.
            if (!String.IsNullOrEmpty(text))
            {
                // Create document.
                document = new FlowDocument();
                // Convert units.
                text = UnitConverter.Convert(text);
                // Replace gestures.
                text = ReplaceGestures(text);
                // Split it in new lines and remove empty lines.
                string[] paragraphs = text.Split(new[] { "\r\n", "\n\r", "\r", "\n" }, StringSplitOptions.None);

                // For each paragraph.
                foreach (string paragraph in paragraphs)
                {
                    // Create paragraph object.
                    Paragraph paragraphContent = new Paragraph(new Run(paragraph));
                    // Check maximum length.
                    paragraphContent.Background = CheckParagraphLength(paragraphContent);
                    // Add paragraph to editor.
                    document.Blocks.Add(paragraphContent);
                }
            }
            return document;
        }
    }
}
