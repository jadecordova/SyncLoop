using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncLoopExcelLibrary
{
    /// <summary>
    /// Excel document properties.
    /// </summary>
    public class DocumentProperties
    {

        #region -----------------------------------------------------------------< PROPERTIES >

        /// <summary>
        /// Document title.
        /// </summary>
        public string DocumentTitle { get; set; }

        /// <summary>
        /// Document author
        /// </summary>
        public string DocumentAuthor { get; set; }

        /// <summary>
        /// Date of creation.
        /// </summary>
        public string DocumentDateCreated { get; set; }

        #endregion

        #region -----------------------------------------------------------------< CONSTRUCTORS >

        public DocumentProperties()
        {
            // Default author.
            DocumentAuthor = "Glyphos, Servicios de Comunicación";
        }

        #endregion

        #region -----------------------------------------------------------------< METHODS >

        public string WriteDocumentProperties()
        {
            // Result constructor.
            StringBuilder properties = new StringBuilder();
            // Header.
            properties.AppendLine(ExcelUtilities.Indent1 + @"<DocumentProperties xmlns=" + "\"" + @"urn:schemas-microsoft-com:office:office" + "\"" + ">");
            // Title
            properties.AppendLine(ExcelUtilities.Indent2 + @"<Title>" + DocumentTitle + @"</Title>");
            // Author.
            properties.AppendLine(ExcelUtilities.Indent2 + @"<Author>" + DocumentAuthor + @"</Author>");
            // Date created.
            properties.AppendLine(ExcelUtilities.Indent2 + @"<Created>" + DocumentDateCreated + "</Created>");
            // Footer
            properties.AppendLine(ExcelUtilities.Indent1 + @"</DocumentProperties>");

            return properties.ToString();
        }

        #endregion
    }
}
