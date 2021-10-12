using System;
using System.Text;

namespace SyncLoopLibrary
{
    /// <summary>
    /// Represents a worksheet column.
    /// </summary>
    public class Column
    {

        #region ENUMERATIONS

        /// <summary>
        /// Options for content autofit.
        /// </summary>
        public enum AutoFit
        {
            /// <summary>
            /// No autofit.
            /// </summary>
            False,
            /// <summary>
            /// Autofit content.
            /// </summary>
            True
        }

        /// <summary>
        /// Options for column hidden.
        /// </summary>
        public enum ColumnHidden
        {
            /// <summary>
            /// Hidden.
            /// </summary>
            False,
            /// <summary>
            /// Visible.
            /// </summary>
            True
        }

        #endregion



        #region PROPERTIES

        /// <summary>
        /// Width.
        /// </summary>
        public string ColumnWidth { get; set; }

        /// <summary>
        /// Style ID for Column
        /// </summary>
        public string ColumnStyleID { get; set; }

        /// <summary>
        /// Auto fit column width (numbers only).
        /// </summary>
        public AutoFit AutoFitWidth { get; set; }

        /// <summary>
        /// Is the column hidde?
        /// </summary>
        public ColumnHidden Hidden { get; set; }

        #endregion



        #region CONSTRUCTORS

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Column()
        {
            // Defaults.
            ColumnWidth = null;
            ColumnStyleID = String.Empty;
            AutoFitWidth = AutoFit.False;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="width">Column width.</param>
        public Column(string width)
        {
            // Defaults.
            ColumnWidth = width;
            ColumnStyleID = String.Empty;
            AutoFitWidth = AutoFit.False;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="width">Column width.</param>
        /// <param name="styleID">Style ID of column.</param>
        public Column(string width, string styleID)
        {
            // Defaults.
            ColumnWidth = width;
            ColumnStyleID = styleID;
            AutoFitWidth = AutoFit.False;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="width">Column width.</param>
        /// <param name="styleID">Style ID of column.</param>
        /// <param name="autoFitWidth">Autofit options.</param>
        public Column(string width, string styleID, AutoFit autoFitWidth)
        {
            // Defaults.
            ColumnWidth = width;
            ColumnStyleID = styleID;
            AutoFitWidth = autoFitWidth;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="width">Column width.</param>
        /// <param name="styleID">Style ID of column.</param>
        /// <param name="autoFitWidth">Autofit options.</param>
        /// <param name="hidden">Hidden options.</param>
        public Column(string width, string styleID, AutoFit autoFitWidth, ColumnHidden hidden)
        {
            // Defaults.
            ColumnWidth = width;
            ColumnStyleID = styleID;
            AutoFitWidth = autoFitWidth;
            Hidden = hidden;
        }

        #endregion



        #region METHODS

        /// <summary>
        /// Creates column string.
        /// </summary>
        /// <returns>Column string.</returns>
        public string WriteColumn()
        {
            // Result constructor.
            StringBuilder column = new StringBuilder();
            // Header.
            column.Append(ExcelUtilities.Indent3 + @"<Column");
            // Width.
            if (ColumnWidth != null)
            {
                column.Append(@" ss:Width =" + ExcelUtilities.Quote + ColumnWidth + ExcelUtilities.Quote);
            }
            // Style.
            if (!String.IsNullOrEmpty(ColumnStyleID))
            {
                column.Append(@" ss:StyleID=" + ExcelUtilities.Quote + ColumnStyleID + ExcelUtilities.Quote + " ");
            }
            // Auto fit.
            if (AutoFitWidth == AutoFit.True)
            {
                column.Append(@" ss:AutoFitWidth=" + ExcelUtilities.Quote + ((int)AutoFitWidth).ToString() + ExcelUtilities.Quote + " ");
            }
            // Hidden.
            if (Hidden == ColumnHidden.True)
            {
                column.Append(@" ss:Hidden=" + ExcelUtilities.Quote + ((int)Hidden).ToString() + ExcelUtilities.Quote + " ");
            }
            // Footer.
            column.AppendLine(@"/>");

            return column.ToString();
        }

        #endregion
    }
}
