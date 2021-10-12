using System.Text;

namespace SyncLoopLibrary
{
    /// <summary>
    /// Defines a worksheet.
    /// </summary>
    public class Worksheet
    {

        #region FIELDS

        /// <summary>
        /// Table containing rows and columns definitions.
        /// </summary>
        Table WorksheetTable;

        /// <summary>
        /// Worksheet options
        /// </summary>
        public WorksheetOptions Options { get; set; }

        string WorksheetName;

        #endregion



        #region CONSTRUCTORS

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="worksheetName">Worksheet name.</param>
        /// <param name="documentTable">Document table.</param>
        public Worksheet(string worksheetName, Table documentTable)
        {
            WorksheetTable = documentTable;
            WorksheetName = worksheetName;
            Options = null;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="worksheetName">Worksheet name.</param>
        /// <param name="documentTable">Document table.</param>
        /// <param name="options">Worksheet options.</param>
        public Worksheet(string worksheetName, Table documentTable, WorksheetOptions options)
        {
            WorksheetTable = documentTable;
            WorksheetName = worksheetName;
            Options = options;
        }

        #endregion



        #region METHODS

        /// <summary>
        /// Creates worksheet.
        /// </summary>
        /// <returns>Worksheet string.</returns>
        public string WriteWorksheet()
        {
            // Result constructor
            StringBuilder worksheet = new StringBuilder();
            // Header.
            worksheet.AppendLine(ExcelUtilities.Indent1 + @"<Worksheet ss:Name=" + ExcelUtilities.Quote + WorksheetName + ExcelUtilities.Quote + ">");
            // Table.
            worksheet.Append(WorksheetTable.WriteTable());
            // Options.
            if (Options != null)
            {
                worksheet.AppendLine(Options.WriteWorksheetOption());
            }
            // Footer.
            worksheet.AppendLine(ExcelUtilities.Indent1 + @"</Worksheet>");

            return worksheet.ToString();
        }

        #endregion
    }
}
