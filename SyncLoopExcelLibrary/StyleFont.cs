using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncLoopExcelLibrary
{
    /// <summary>
    /// Defines a style font.
    /// </summary>
    public class StyleFont
    {

        #region --------------------------------------------------------------------------------ENUMERATIONS

        public enum FontWeight
        {
            Normal,
            Bold
        }

        #endregion

        #region --------------------------------------------------------------------------------PROPERTIES

        /// <summary>
        /// Name of font.
        /// </summary>
        public string FontName { get; set; }

        /// <summary>
        /// Family of font.
        /// </summary>
        public string FontFamily { get; set; }

        /// <summary>
        /// Size.
        /// </summary>
        public string FontSize { get; set; }

        /// <summary>
        /// Color in hex format.
        /// </summary>
        public string FontColor { get; set; }

        /// <summary>
        /// Color in hex format.
        /// </summary>
        public FontWeight Weight { get; set; }

        #endregion

        #region --------------------------------------------------------------------------------CONSTRUCTORS

        /// <summary>
        /// Default constructor.
        /// </summary>
        public StyleFont() { }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="fontName">Name of font.</param>
        /// <param name="fontSize">Size in points.</param>
        /// <param name="fontColor">Color in hex format.</param>
        public StyleFont(string fontName, string fontSize, string fontColor)
        {
            FontName = fontName;
            FontSize = fontSize;
            FontColor = fontColor;
            FontFamily = String.Empty;
            Weight = FontWeight.Normal;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="fontName">Name of font.</param>
        /// <param name="fontSize">Size in points.</param>
        /// <param name="fontColor">Color in hex format.</param>
        public StyleFont(string fontName, string fontSize, string fontColor, FontWeight weight)
        {
            FontName = fontName;
            FontSize = fontSize;
            FontColor = fontColor;
            FontFamily = String.Empty;
            Weight = weight;
        }

        #endregion

        #region --------------------------------------------------------------------------------METHODS

        public string WriteFont()
        {
            //Result constructor.
            StringBuilder font = new StringBuilder();
            // Write font.
            font.Append(ExcelUtilities.Indent3 + @"<Font ");

            if (!String.IsNullOrEmpty(FontName))
            {
                font.Append(@"ss:FontName=" + ExcelUtilities.Quote + FontName + ExcelUtilities.Quote + " ");
            }
            if (!String.IsNullOrEmpty(FontFamily))
            {
                font.Append(@"x:Family=" + ExcelUtilities.Quote + FontFamily + ExcelUtilities.Quote + " ");
            }
            if (!String.IsNullOrEmpty(FontSize))
            {
                font.Append(@"ss:Size=" + ExcelUtilities.Quote + FontSize + ExcelUtilities.Quote + " ");
            }
            if (!String.IsNullOrEmpty(FontColor))
            {
                font.Append(@"ss:Color=" + ExcelUtilities.Quote + FontColor + ExcelUtilities.Quote + " ");
            }
            if (Weight == FontWeight.Bold)
            {
                font.Append(@"ss:Bold=" + ExcelUtilities.Quote + ((int)Weight).ToString() + ExcelUtilities.Quote);
            }
            font.Append(@"/>");

            return font.ToString();
        }

        #endregion
    }
}
