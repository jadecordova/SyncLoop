using System.Collections.Generic;
using System.Text;

namespace SyncLoopLibrary
{
    /// <summary>
    /// Defines a document row.
    /// </summary>
    public class Row
    {

        #region PROPERTIES

        /// <summary>
        /// List of row cells.
        /// </summary>
        List<Cell> rowCells = new List<Cell>();

        #endregion



        #region CONSTRUCTORS

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="cells">List of cells.</param>
        public Row(List<Cell> cells)
        {
            rowCells = cells;
        }

        #endregion



        #region METHODS

        /// <summary>
        /// Creates row.
        /// </summary>
        /// <returns>Row string.</returns>
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
