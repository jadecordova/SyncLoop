using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncLoopExcelLibrary
{
    public class CustomColor
    {

        #region ------------------------------------------------------------PROPERTIES

        /// <summary>
        /// Defines the color in hex.
        /// </summary>
        public string HexadecimalColorValue { get; set; }

        /// <summary>
        /// Index of color in Excel color table.
        /// </summary>
        public string Index { get; set; }

        #endregion

        #region ------------------------------------------------------------CONSTRUCTORS

        public CustomColor(string hexValue, int indexInTable)
        {
            HexadecimalColorValue = hexValue;
            Index = indexInTable.ToString();
        }

        #endregion

        #region ------------------------------------------------------------METHODS

        public string WriteColor()
        {
            // Result constructor.
            StringBuilder color = new StringBuilder();
            // Header.
            color.AppendLine(ExcelUtilities.Indent3 + @"<Color>");
            // Index.
            color.AppendLine(ExcelUtilities.Indent4 + @"<Index>" + Index + "</Index>");
            //Color.
            color.AppendLine(ExcelUtilities.Indent4 + @"<RGB>" + HexadecimalColorValue + "</RGB>");
            // Footer.
            color.AppendLine(ExcelUtilities.Indent3 + @"</Color>");

            return color.ToString();

        }

        #endregion
    }
}
