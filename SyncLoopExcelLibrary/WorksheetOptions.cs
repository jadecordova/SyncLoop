using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncLoopExcelLibrary
{
    /// <summary>
    /// Options for the worksheet.
    /// </summary>
    public class WorksheetOptions
    {

        #region -----------------------------------------------------------------PROPERTIES

        /// <summary>
        /// Page setup
        /// </summary>
        public PageSetup PageSettings { get; set; }

        #endregion

        #region -----------------------------------------------------------------CONSTRUCTORS

        public WorksheetOptions(PageSetup setup)
        {
            PageSettings = setup;
        }

        #endregion

        #region -----------------------------------------------------------------METHODS

        public string WriteWorksheetOption()
        {
            // Result constructor.
            StringBuilder options = new StringBuilder();
            // Header.
            options.AppendLine(ExcelUtilities.Indent2 + @"<WorksheetOptions xmlns=" + ExcelUtilities.Quote + "urn:schemas-microsoft-com:office:excel" + ExcelUtilities.Quote + ">");
            // Write options.
            options.Append(PageSettings.WritePageSetup());
            // Footer.
            options.AppendLine(ExcelUtilities.Indent2 + @"</WorksheetOptions>");

            return options.ToString();
        }
        #endregion
    }
}
