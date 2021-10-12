using System.Text;

namespace SyncLoopLibrary
{
    /// <summary>
    ///  Header of document containing XML and application declarations.
    /// </summary>
    public class ExcelDocumentHeader
    {

        #region PROPERTIES

        /// <summary>
        /// XML declarations.
        /// </summary>
        public string XMLversion { get; set; }

        /// <summary>
        /// Type of application.
        /// </summary>
        public string Application { get; set; }

        #endregion



        #region CONSTRUCTORS

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ExcelDocumentHeader()
        {
            XMLversion = @"<?xml version=" + ExcelUtilities.Quote + "1.0" + ExcelUtilities.Quote + "?>";
            Application = @"<?mso-application progid=" + ExcelUtilities.Quote + "Excel.Sheet" + ExcelUtilities.Quote + "?>";
        }

        #endregion



        #region METHODS

        /// <summary>
        /// Creates header.
        /// </summary>
        /// <returns>Header string.</returns>
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
