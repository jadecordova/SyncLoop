using System.Text;

namespace SyncLoopLibrary
{
    /// <summary>
    /// Defines a cell border.
    /// </summary>
    public class StyleBorder
    {

        #region ENUMERATIONS

        /// <summary>
        /// Type of border.
        /// </summary>
        public enum BorderType
        {
            Top,
            Right,
            Bottom,
            Left
        }

        /// <summary>
        /// Line style.
        /// </summary>
        public enum LineStyle
        {
            Continuous
        }

        #endregion



        #region PROPERTIES

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



        #region CONSTRUCTORS

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="borderType">Borde type.</param>
        /// <param name="lineStyle">Line style.</param>
        /// <param name="borderWeight">Border weight.</param>
        /// <param name="borderColor">Border color.</param>
        public StyleBorder(BorderType borderType, LineStyle lineStyle, string borderWeight, string borderColor)
        {
            BorderPosition = borderType;
            BorderLineStyle = lineStyle;
            BorderLineWeight = borderWeight;
            BorderColor = borderColor;
        }

        #endregion



        #region METHODS

        /// <summary>
        /// Creates border.
        /// </summary>
        /// <returns>Border string.</returns>
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
