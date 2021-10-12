using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncLoopExcelLibrary
{
    /// <summary>
    /// Defines a document row.
    /// </summary>
    public class Row
    {

        #region -----------------------------------------------------------------PROPERTIES

        /// <summary>
        /// List of row cells.
        /// </summary>
        List<Cell> rowCells = new List<Cell>();

        #endregion

        #region -----------------------------------------------------------------CONSTRUCTORS

        public Row(List<Cell> cells)
        {
            rowCells = cells;
        }

        #endregion

        #region -----------------------------------------------------------------METHODS

        public string WriteRow()
        {
            // Result constructor.
            StringBuilder row = new StringBuilder();
            // Header.
            row.AppendLine(ExcelUtilities.Indent3 + @"<Row>");
            // Cells.
            foreach (Cell cell in rowCells)
            {
                row.Append(cell.WriteCell());
            }
            // Footer.
            row.AppendLine(ExcelUtilities.Indent3 + @"</Row>");

            return row.ToString();
        }

        #endregion
    }
}
