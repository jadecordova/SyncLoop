using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncLoopExcelLibrary
{
    /// <summary>
    ///  Header of document containing XML and application declarations.
    /// </summary>
    public class ExcelDocumentHeader
    {

        #region -----------------------------------------------------------------< PROPERTIES >

        /// <summary>
        /// XML declarations.
        /// </summary>
        public string XMLversion { get; set; }

        /// <summary>
        /// Type of application.
        /// </summary>
        public string Application { get; set; }

        #endregion

        #region -----------------------------------------------------------------< CONSTRUCTORS >

        public ExcelDocumentHeader()
        {
            XMLversion = @"<?xml version=" + ExcelUtilities.Quote + "1.0" + ExcelUtilities.Quote + "?>";
            Application = @"<?mso-application progid=" + ExcelUtilities.Quote + "Excel.Sheet" + ExcelUtilities.Quote + "?>";
        }

        #endregion

        #region -----------------------------------------------------------------METHODS

        public string WriteHeader()
        {
            // Result constructor.
            StringBuilder header = new StringBuilder();
            // Write it.
            header.AppendLine(XMLversion);
            header.AppendLine(Application);

            return header.ToString();
        }

        #endregion
    }
}
