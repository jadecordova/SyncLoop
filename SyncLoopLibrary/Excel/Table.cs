using System.Collections.Generic;
using System.Text;

namespace SyncLoopLibrary
{
    /// <summary>
    /// Table containing document rows.
    /// </summary>
    public class Table
    {

        #region PROPERTIES

        /// <summary>
        /// List of rows for this table.
        /// </summary>
        public List<Row> DocumentRows { get; set; }

        /// <summary>
        /// List of rows.
        /// </summary>
        public List<Column> DocumentColumns { get; set; }

        #endregion



        #region CONSTRUCTORS

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Table(List<Row> documentRows, List<Column> documentColumns)
        {
            DocumentRows = documentRows;
            DocumentColumns = documentColumns;
        }

        #endregion



        #region METHODS

        /// <summary>
        /// Creates document table.
        /// </summary>
        /// <returns>Table string.</returns>
        public string WriteTable()
        {
            // Result constructor.
            StringBuilder table = new StringBuilder();
            // Header.
            table.AppendLine(ExcelUtilities.Indent2 + @"<Table>");
            // Columns.
            foreach (Column column in DocumentColumns)
            {
                table.Append(column.WriteColumn());
            }
            // Rows.
            foreach (Row row in DocumentRows)
            {
                table.Append(row.WriteRow());
            }
            // Footer.
            table.AppendLine(ExcelUtilities.Indent2 + @"</Table>");

            return table.ToString();
        }

        #endregion
    }
}
