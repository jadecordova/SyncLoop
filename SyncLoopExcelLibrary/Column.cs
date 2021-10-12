using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncLoopExcelLibrary
{
    public class Column
    {

        #region ------------------------------------------------------------ENUMERATIONS

        public enum AutoFit
        {
            False,
            True
        }

        public enum ColumnHidden
        {
            False,
            True
        }

        #endregion

        #region ------------------------------------------------------------PROPERTIES

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

        #region ------------------------------------------------------------CONSTRUCTORS

        public Column()
        {
            // Defaults.
            ColumnWidth = null;
            ColumnStyleID = String.Empty;
            AutoFitWidth = AutoFit.False;
        }

        public Column(string width)
        {
            // Defaults.
            ColumnWidth = width;
            ColumnStyleID = String.Empty;
            AutoFitWidth = AutoFit.False;
        }

        public Column(string width, string styleID)
        {
            // Defaults.
            ColumnWidth = width;
            ColumnStyleID = styleID;
            AutoFitWidth = AutoFit.False;
        }

        public Column(string width, string styleID, AutoFit autoFitWidth)
        {
            // Defaults.
            ColumnWidth = width;
            ColumnStyleID = styleID;
            AutoFitWidth = autoFitWidth;
        }

        public Column(string width, string styleID, AutoFit autoFitWidth, ColumnHidden hidden)
        {
            // Defaults.
            ColumnWidth = width;
            ColumnStyleID = styleID;
            AutoFitWidth = autoFitWidth;
            Hidden = hidden;
        }

        #endregion

        #region ------------------------------------------------------------METHODS

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
