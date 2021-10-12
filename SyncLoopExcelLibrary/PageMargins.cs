using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncLoopExcelLibrary
{
    /// <summary>
    /// Margins for the printed document.
    /// </summary>
    public class PageMargins
    {

        #region -----------------------------------------------------------------PROPERTIES

        /// <summary>
        /// REQUIRED. Bottom margin in inches.
        /// </summary>
        public double Bottom { get; set; }

        /// <summary>
        /// REQUIRED. Left margin in inches.
        /// </summary>
        public double Left { get; set; }

        /// <summary>
        /// REQUIRED. Right margin in inches.
        /// </summary>
        public double Right { get; set; }

        /// <summary>
        /// REQUIRED. Top margin in inches.
        /// </summary>
        public double Top { get; set; }

        #endregion

        #region -----------------------------------------------------------------CONSTRUCTROS

        public PageMargins(double bottomMargin, double leftMargin, double rightMargin, double topMargin)
        {
            Bottom = bottomMargin;
            Left = leftMargin;
            Right = rightMargin;
            Top = topMargin;
        }

        #endregion

        #region -----------------------------------------------------------------METHODS

        public string WriteMargins()
        {
            // Result constructor.
            StringBuilder margins = new StringBuilder();
            // Header.
            margins.Append(ExcelUtilities.Indent5 + @"<PageMargins");
            // Margins.
            margins.Append(@" x:Bottom=" + ExcelUtilities.Quote + Bottom.ToString() + ExcelUtilities.Quote);
            margins.Append(@" x:Left=" + ExcelUtilities.Quote + Left.ToString() + ExcelUtilities.Quote);
            margins.Append(@" x:Right=" + ExcelUtilities.Quote + Right.ToString() + ExcelUtilities.Quote);
            margins.Append(@" x:Top=" + ExcelUtilities.Quote + Top.ToString() + ExcelUtilities.Quote);
            // Footer.
            margins.AppendLine(@"/>");

            return margins.ToString();
        }

        #endregion
    }
}
