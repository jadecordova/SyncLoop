using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;

namespace SyncLoopLibrary
{
    /// <summary>
    /// Flow document extensions.
    /// </summary>
    public static class FlowDocumentExtensions
    {
        /// <summary>
        /// Gets all document paragraphs.
        /// </summary>
        /// <param name="doc">Flow document.</param>
        /// <returns>Document paragraphs.</returns>
        public static IEnumerable<Paragraph> Paragraphs(this FlowDocument doc)
        {
            return doc.Descendants().OfType<Paragraph>();
        }
    }
}
