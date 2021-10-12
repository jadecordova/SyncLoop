using System.Collections.Generic;
using System.Text;

namespace SyncLoopLibrary
{
    /// <summary>
    /// Defines a list of custom colors for the Excel document.
    /// </summary>
    public class Colors
    {

        #region PROPERTIES

        /// <summary>
        /// List of custom colors.
        /// </summary>
        public List<CustomColor> CustomColors { get; set; }

        #endregion



        #region CONSTRUCTORS

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="colors">List of custom colors.</param>
        public Colors(List<CustomColor> colors)
        {
            CustomColors = colors;
        }

        #endregion



        #region METHODS

        /// <summary>
        /// Creates colors string.
        /// </summary>
        /// <returns>Colors string.</returns>
        public string WriteColors()
        {
            // Result constructor.
            StringBuilder colors = new StringBuilder();
            // Header.
            colors.AppendLine(ExcelUtilities.Indent2 + @"<Colors>");
            // Colors.
            foreach (CustomColor color in CustomColors)
            {
                colors.Append(color.WriteColor());
            }
            // Footer.
            colors.AppendLine(ExcelUtilities.Indent2 + @"</Colors>");

            return colors.ToString();
        }

        #endregion

    }
}
