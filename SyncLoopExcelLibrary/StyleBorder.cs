using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncLoopExcelLibrary
{
    /// <summary>
    /// Defines a cell border.
    /// </summary>
    public class StyleBorder
    {

        #region -----------------------------------------------------------------ENUMERATIONS

        public enum BorderType
        {
            Top,
            Right,
            Bottom,
            Left
        }

        public enum LineStyle
        {
            Continuous
        }

        #endregion

        #region -----------------------------------------------------------------PROPERTIES

        /// <summary>
        /// Defines where is the border located.
        /// </summary>
        public BorderType BorderPosition { get; set; }

        /// <summary>
        /// Type of line.
        /// </summary>
        public LineStyle BorderLineStyle { get; set; }

        /// <summary>
        /// Line weight.
        /// </summary>
        public string BorderLineWeight { get; set; }

        /// <summary>
        /// Border color.
        /// </summary>
        public string BorderColor { get; set; }

        #endregion

        #region -----------------------------------------------------------------CONSTRUCTORS

        public StyleBorder(BorderType borderType, LineStyle lineStyle, string borderWeight, string borderColor)
        {
            BorderPosition = borderType;
            BorderLineStyle = lineStyle;
            BorderLineWeight = borderWeight;
            BorderColor = borderColor;
        }

        #endregion

        #region -----------------------------------------------------------------METHODS

        public string WriteBorder()
        {
            // Result constructor
            StringBuilder border = new StringBuilder();
            // Write border
            border.Append(ExcelUtilities.Indent5 + 
                @"<Border ss:Position=" + ExcelUtilities.Quote + BorderPosition + ExcelUtilities.Quote +
                                        " ss:LineStyle=" + ExcelUtilities.Quote + BorderLineStyle + ExcelUtilities.Quote +
                                        " ss:Weight=" + ExcelUtilities.Quote + BorderLineWeight + ExcelUtilities.Quote +
                                        " ss:Color=" + ExcelUtilities.Quote + BorderColor + ExcelUtilities.Quote + "/>");

            return border.ToString();
        }

        #endregion
    }
}
