using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncLoopExcelLibrary
{
    /// <summary>
    /// Footer for page setup class.
    /// </summary>
    public class Footer
    {

        #region ------------------------------------------------------------PROPERTIES

        /// <summary>
        /// REQUIRED. Footer margin.
        /// </summary>
        public double FooterMargin { get; set; }

        /// <summary>
        /// OPTIONAL. Data in CSS format for Excel 2000.
        /// </summary>
        public string FooterData { get; set; }

        #endregion

        #region ------------------------------------------------------------CONSTRUCTORS

        public Footer(double margin)
        {
            FooterMargin = margin;
            FooterData = String.Empty;
        }

        public Footer(double margin, string data)
        {
            FooterMargin = margin;
            FooterData = data;
        }

        #endregion

        #region ------------------------------------------------------------METHODS

        public string WriteFooter()
        {
            // Result constructor.
            StringBuilder footer = new StringBuilder();
            // Footer.
            footer.Append(ExcelUtilities.Indent5 + @"<Footer");
            // Margin
            footer.Append(@" x:Margin=" + ExcelUtilities.Quote + FooterMargin + ExcelUtilities.Quote);
            // Data.
            if (!String.IsNullOrEmpty(FooterData))
            {
                footer.Append(@" x:Data=" + ExcelUtilities.Quote + FooterData + ExcelUtilities.Quote);
            }
            // Footer
            footer.AppendLine(@"/>");

            return footer.ToString();
        }

        #endregion
    }
}
