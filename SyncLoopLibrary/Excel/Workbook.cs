using System;
using System.Collections.Generic;
using System.Text;

namespace SyncLoopLibrary
{
    /// <summary>
    /// Defines the complete Excel document.
    /// </summary>
    public class Workbook
    {

        #region FIELDS

        /// <summary>
        /// Document styles
        /// </summary>
        DocumentStyles BookStyles;

        /// <summary>
        /// Document properties.
        /// </summary>
        DocumentProperties BookProperties;

        /// <summary>
        /// Excel workbook properties.
        /// </summary>
        ExcelWorkbook ExcelBook;

        /// <summary>
        /// Book worksheets.
        /// </summary>
        List<Worksheet> BookWorksheets;

        #endregion



        #region PROPERTIES

        /// <summary>
        /// XML version
        /// </summary>
        public string XMLversion { get; set; }

        /// <summary>
        /// Microsoft code for Excel app.
        /// </summary>
        public string ApplicationDefinition { get; set; }

        /// <summary>
        /// Microsoft schema.
        /// </summary>
        public string WorkbookDefinition { get; set; }

        /// <summary>
        /// Defines Excel-specific settings.
        /// </summary>
        public OfficeDocumentSettings OfficeSettings { get; set; }

        #endregion



        #region CONSTRUCTORS

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="documentStyles">Document styles.</param>
        /// <param name="documentProperties">Document properties.</param>
        /// <param name="excelWorkbook">Workbook.</param>
        /// <param name="documentWorksheets">Document worksheet.</param>
        /// <param name="officeDocumentSettings">Office document settings.</param>
        public Workbook(DocumentStyles documentStyles,
                        DocumentProperties documentProperties,
                        ExcelWorkbook excelWorkbook,
                        List<Worksheet> documentWorksheets,
                        OfficeDocumentSettings officeDocumentSettings = null)
        {
            XMLversion = @"<?xml version=" + ExcelUtilities.Quote + "1.0" + ExcelUtilities.Quote + "?>";
            ApplicationDefinition = @"<?mso-application progid='Excel.Sheet'?>";
            WorkbookDefinition = @"<Workbook xmlns=" + ExcelUtilities.Quote + "urn:schemas-microsoft-com:office:spreadsheet" + ExcelUtilities.Quote + Environment.NewLine +
                                    @" xmlns:o=" + ExcelUtilities.Quote + "urn:schemas-microsoft-com:office:office" + ExcelUtilities.Quote + Environment.NewLine +
                                    @" xmlns:x=" + ExcelUtilities.Quote + "urn:schemas-microsoft-com:office:excel" + ExcelUtilities.Quote + Environment.NewLine +
                                    @" xmlns:ss=" + ExcelUtilities.Quote + "urn:schemas-microsoft-com:office:spreadsheet" + ExcelUtilities.Quote + Environment.NewLine +
                                    @" xmlns:html=" + ExcelUtilities.Quote + @"http://www.w3.org/TR/REC-html40" + ExcelUtilities.Quote + ">";
            BookStyles = documentStyles;
            BookProperties = documentProperties;
            ExcelBook = excelWorkbook;
            BookWorksheets = documentWorksheets;
            OfficeSettings = officeDocumentSettings;
        }

        #endregion



        #region METHODS

        /// <summary>
        /// Writes header of document
        /// </summary>
        /// <returns>Header string.</returns>
        private string WriteHeader()
        {
            // Result constructor.
            StringBuilder header = new StringBuilder();
            // Create header.
            header.AppendLine(XMLversion);
            header.AppendLine(ApplicationDefinition);
            header.AppendLine(WorkbookDefinition);

            return header.ToString();
        }

        /// <summary>
        /// Writes complete Excel document.
        /// </summary>
        /// <returns></returns>
        public string WriteBook()
        {
            // Result constructor.
            StringBuilder book = new StringBuilder();
            // Create book.
            book.Append(WriteHeader());
            // Document properties.
            book.Append(BookProperties.WriteDocumentProperties());
            // Office document properties.
            if (OfficeSettings != null) book.Append(OfficeSettings.WriteOfficeDocumentSettings());
            // Excel document properties.
            book.Append(ExcelBook.WriteExcelDocument());
            // Include styles.
            book.Append(BookStyles.WriteDocumentSytles());
            // Worksheets.
            foreach (Worksheet sheet in BookWorksheets)
            {
                book.Append(sheet.WriteWorksheet());
            }
            // Footer.
            book.AppendLine(@"</Workbook>");

            return book.ToString();
        }

        #endregion
    }
}
