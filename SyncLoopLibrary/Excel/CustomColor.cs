using System.Text;

namespace SyncLoopLibrary
{
    /// <summary>
    /// Defines colors for each character, to help in character recognition at proof reading.
    /// </summary>
    public class CustomColor
    {

        #region PROPERTIES

        /// <summary>
        /// Defines the color in hex.
        /// </summary>
        public string HexadecimalColorValue { get; set; }

        /// <summary>
        /// Index of color in Excel color table.
        /// </summary>
        public string Index { get; set; }

        #endregion



        #region CONSTRUCTORS

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="hexValue">Color in hexadecimal format.</param>
        /// <param name="indexInTable">Index of color.</param>
        public CustomColor(string hexValue, int indexInTable)
        {
            HexadecimalColorValue = hexValue;
            Index = indexInTable.ToString();
        }

        #endregion


        
        #region METHODS

        /// <summary>
        /// Creates color.
        /// </summary>
        /// <returns>Color string.</returns>
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
