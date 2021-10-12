using System.Collections.Generic;
using System.Text;

namespace SyncLoopLibrary
{
    /// <summary>
    /// Defines a RTF document.
    /// </summary>
    public class RTFDocument
    {

        #region MEMBERS

        string documentOpen = @"{";
        string documentClose = @"}";

        #endregion



        #region PROPERTIES

        /// <summary>
        /// Document properties.
        /// </summary>
        public RTFProperties Properties { get; set; }

        /// <summary>
        /// Document font table.
        /// </summary>
        public RTFFontTable FontTable { get; set; }

        /// <summary>
        /// Document color table.
        /// </summary>
        public RTFColorTable ColorTable { get; set; }

        /// <summary>
        /// Document paragraphs.
        /// </summary>
        public List<RTFParagraph> Paragraphs { get; set; }

        #endregion



        #region CONSTRUCTORS

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="fonts">Fonts array.</param>
        /// <param name="paragraphs">List of RTF paragraphs.</param>
        /// <param name="colorTable">RTF color table.</param>
        public RTFDocument(
            string[] fonts,
            List<RTFParagraph> paragraphs,
            RTFColorTable colorTable = null)
        {
            FontTable = new RTFFontTable(fonts);
            Paragraphs = paragraphs;
            ColorTable = colorTable;
            Properties = new RTFProperties(FontTable, ColorTable);
        }

        #endregion



        #region METHODS

        /// <summary>
        /// Creates document.
        /// </summary>
        /// <returns>Document string.</returns>
        public string WriteDocument()
        {
            // Result constructor.
            StringBuilder result = new StringBuilder();
            // Open document.
            result.Append(documentOpen);
            // Add prolog.
            result.Append(Properties.WriteProperties());
            // Append content.
            foreach(RTFParagraph paragraph in Paragraphs)
            {
                result.Append(paragraph.WriteParagraph());
            }

            // Close document.
            result.Append(documentClose);
            // Convert and replace characters.
            string resultConverted = ReplaceCharacters(result.ToString());
            // return.
            return resultConverted;
        }

        private string ReplaceCharacters(string originalString)
        {
            // Result builder.
            StringBuilder result = new StringBuilder();

            string tmp;

            foreach (char c in originalString)
            {
                if (RTFCharacterMap.Map.TryGetValue(c, out tmp))
                    result.Append(tmp);
                else
                    result.Append(c);
            }

            return result.ToString();
        }

        #endregion
    }
}
