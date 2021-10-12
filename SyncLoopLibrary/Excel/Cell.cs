using System;
using System.Text;

namespace SyncLoopLibrary
{
    /// <summary>
    /// Defines a document cell.
    /// </summary>
    public class Cell
    {

        #region ENUMERATIONS

        /// <summary>
        /// Type of data in cell.
        /// </summary>
        public enum DataType
        {
            /// <summary>
            /// Numeric data.
            /// </summary>
            Number,
            /// <summary>
            /// Text data.
            /// </summary>
            String
        }

        #endregion



        #region PROPERTIES

        /// <summary>
        /// ID of cell's style.
        /// </summary>
        public string CellStyleID { get; set; }

        /// <summary>
        /// Type of data (String or Number)
        /// </summary>
        public DataType CellDataType { get; set; }

        /// <summary>
        /// Data of the cell.
        /// </summary>
        public string CellData { get; set; }

        /// <summary>
        /// Index of cell in row.
        /// </summary>
        public string CellIndex { get; set; }

        /// <summary>
        /// For cell merging.
        /// </summary>
        public CellMerge Merge { get; set; }

        /// <summary>
        /// Defines the cell's formula.
        /// </summary>
        public string CellFormula { get; set; }

        #endregion



        #region CONSTRUCTORS

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Cell()
        {
            Merge = new CellMerge();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="id">Cell ID.</param>
        /// <param name="type">Type of data in cell.</param>
        /// <param name="data">Actual cell data.</param>
        /// <param name="merge">Merge info.</param>
        /// <param name="index">Index. Defaults to empty string.</param>
        public Cell(string id, DataType type, string data, CellMerge merge, string index = "")
        {
            // Defaults.
            CellStyleID = id;
            CellDataType = type;
            CellData = data;
            CellIndex = index;
            Merge = merge;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="id">Cell ID.</param>
        /// <param name="type">Type of data in cell.</param>
        /// <param name="data">Actual cell data.</param>
        /// <param name="index">Index. Defaults to empty string.</param>
        public Cell(string id, DataType type, string data, string index = "")
        {
            // Defaults.
            CellStyleID = id;
            CellDataType = type;
            CellData = data;
            CellIndex = index;
            Merge = new CellMerge();
        }

        #endregion



        #region METHODS

        /// <summary>
        /// Creates cell.
        /// </summary>
        /// <returns>Cell string.</returns>
        public string WriteCell()
        {
            // Result constructor.
            StringBuilder cell = new StringBuilder();
            // Header.
            cell.Append(ExcelUtilities.Indent4 + @"<Cell ");

            // Merge
            if ((Merge.MergeAcross != "0") || (Merge.MergeDown != "0"))
            {
                cell.Append(Merge.WriteMerge());
            }

            cell.AppendLine((
                String.IsNullOrEmpty(CellStyleID) ? "" : (@"ss:StyleID=" + ExcelUtilities.Quote + CellStyleID + ExcelUtilities.Quote)) +
                (String.IsNullOrEmpty(CellIndex) ? "" : (" ss:Index=" + ExcelUtilities.Quote + CellIndex + ExcelUtilities.Quote)) +
                (String.IsNullOrEmpty(CellFormula) ? "" : (" ss:Formula=" + ExcelUtilities.Quote + CellFormula + ExcelUtilities.Quote)) + @">");
            
            // Data.
            cell.AppendLine(ExcelUtilities.Indent5 + @"<Data ss:Type=" + ExcelUtilities.Quote + CellDataType.ToString() + ExcelUtilities.Quote + @">" + CellData + @"</Data>");
            // Footer.
            cell.AppendLine(ExcelUtilities.Indent4 + @"</Cell>");

            return cell.ToString();
        }

        #endregion
    }
}
