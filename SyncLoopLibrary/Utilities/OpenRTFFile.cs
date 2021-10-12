using System;
using System.IO;
using System.Windows.Documents;

namespace SyncLoopLibrary
{
    public static partial class Utilities
    {
        /// <summary>
        /// Opens RTF file.
        /// </summary>
        /// <param name="path">File path.</param>
        /// <returns>Content string.</returns>
        public static string OpenRTFFile(string path)
        {
            try
            {
                // Create flow document.
                FlowDocument document = new FlowDocument();
                // create a TextRange around the entire document
                TextRange txtRange = new TextRange(document.ContentStart, document.ContentEnd);
                // Read file.
                using (var fStream = new FileStream(path, FileMode.OpenOrCreate))
                {
                    // load as RTF, text is formatted
                    txtRange.Load(fStream, System.Windows.DataFormats.Rtf);
                }

                TextRange range = new TextRange(document.ContentStart, document.ContentEnd);
                // Assign it.
                return range.Text;
            }
            catch (Exception ex)
            {
                // Log($"Error reading the RTF file: {e.Message}");

                return null;
            }
        }
    }
}
