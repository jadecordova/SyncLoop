using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncLoopExcelLibrary
{
    /// <summary>
    /// Defines a list of custom colors for the Excel document.
    /// </summary>
    public class Colors
    {

        #region -------------------------------------------------------PROPERTIES

        public List<CustomColor> CustomColors { get; set; }

        #endregion

        #region -------------------------------------------------------CONSTRUCTORS

        public Colors(List<CustomColor> colors)
        {
            CustomColors = colors;
        }
        #endregion

        #region -------------------------------------------------------METHODS

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
