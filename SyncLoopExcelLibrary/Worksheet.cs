using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncLoopExcelLibrary
{
    /// <summary>
    /// Defines a worksheet.
    /// </summary>
    public class Worksheet
    {

        #region ------------------------------------------------------------PROPERTIES

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

        #region ------------------------------------------------------------CONSTRUCTORS

        public Worksheet(string worksheetName, Table documentTable)
        {
            WorksheetTable = documentTable;
            WorksheetName = worksheetName;
            Options = null;
        }

        public Worksheet(string worksheetName, Table documentTable, WorksheetOptions options)
        {
            WorksheetTable = documentTable;
            WorksheetName = worksheetName;
            Options = options;
        }

        #endregion

        #region ------------------------------------------------------------METHODS

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
