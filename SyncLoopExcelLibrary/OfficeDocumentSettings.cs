using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncLoopExcelLibrary
{
    /// <summary>
    /// Office-specific settings.
    /// </summary>
    public class OfficeDocumentSettings
    {

        #region ----------------------------------------------------------------------PROPERTIES

        /// <summary>
        /// Collection of document colors.
        /// </summary>
        public Colors DocumentColors { get; set; }

        #endregion

        #region ----------------------------------------------------------------------CONSCTRUCTORS

        public OfficeDocumentSettings(Colors colors)
        {
            DocumentColors = colors;
        }
        #endregion

        #region ----------------------------------------------------------------------METHODS

        public string WriteOfficeDocumentSettings()
        {
            // Result constructor.
            StringBuilder settings = new StringBuilder();
            // Header
            settings.AppendLine(ExcelUtilities.Indent2 + @"<OfficeDocumentSettings xmlns=" + ExcelUtilities.Quote + "urn:schemas-microsoft-com:office:office" + ExcelUtilities.Quote + ">");
            // Colors.
            settings.Append(DocumentColors.WriteColors());
            // Footer.
            settings.AppendLine(ExcelUtilities.Indent2 + @"</OfficeDocumentSettings>");

            return settings.ToString();
        }

        #endregion
    }
}
