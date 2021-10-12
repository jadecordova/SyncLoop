using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncLoopRTFLibrary
{
    public class RTFDocument
    {

        #region --------------------------------------------------------------------------------< MEMBERS >

        string documentOpen = @"{";
        string documentClose = @"}";

        #endregion

        #region --------------------------------------------------------------------------------< PROPERTIES >

        public RTFProperties Properties { get; set; }
        public RTFFontTable FontTable { get; set; }
        public RTFColorTable ColorTable { get; set; }
        public List<RTFParagraph> Paragraphs { get; set; }

        #endregion


        #region --------------------------------------------------------------------------------< CONSTRUCTORS >

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

        #region --------------------------------------------------------------------------------< METHODS >

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
