using System.Text;

namespace SyncLoopLibrary
{
    /// <summary>
    /// Options for the worksheet.
    /// </summary>
    public class WorksheetOptions
    {

        #region PROPERTIES

        /// <summary>
        /// Page setup
        /// </summary>
        public PageSetup PageSettings { get; set; }

        #endregion



        #region CONSTRUCTORS

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="setup">Page setup.</param>
        public WorksheetOptions(PageSetup setup)
        {
            PageSettings = setup;
        }

        #endregion



        #region METHODS

        /// <summary>
        /// Creates worksheet options.
        /// </summary>
        /// <returns>Worksheet options string.</returns>
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
