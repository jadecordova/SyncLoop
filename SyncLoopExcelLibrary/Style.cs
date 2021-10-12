using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncLoopExcelLibrary
{
    /// <summary>
    /// Defines a cell style.
    /// </summary>
    public class Style
    {

        #region -----------------------------------------------------------------PROPERTIES

        /// <summary>
        /// Style ID.
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Horizontal alignment of cell.
        /// </summary>
        public StyleAlignment Alignment { get; set; }

        /// <summary>
        /// Cell font.
        /// </summary>
        public StyleFont Font { get; set; }

        /// <summary>
        /// Background of cell.
        /// </summary>
        public StyleInterior Background { get; set; }

        /// <summary>
        /// Borders list.
        /// </summary>
        public List<StyleBorder> Borders { get; set; }

        /// <summary>
        /// Format for cells.
        /// </summary>
        public NumberFormat CellNumberFormat { get; set; }

        #endregion PROPERTIES

        #region -----------------------------------------------------------------CONSTRUCTORS

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Style()
        {
            // Default ID.
            ID = "Default";
            // Default alignment.
            Alignment = new StyleAlignment(StyleAlignment.HorizontalAlignment.Center, StyleAlignment.VerticalAlignment.Center);
            // Default font.
            Font = new StyleFont("Calibri", "12", "#000000");
            // Default background.
            Background = new StyleInterior("", StyleInterior.InteriorPattern.Solid);
            // Borders list.
            Borders = new List<StyleBorder>();
            // Cell number format.
            CellNumberFormat = null;
        }

        #endregion

        #region -----------------------------------------------------------------METHODS

        public string WriteStyle()
        {
            // Content accumulator.
            StringBuilder style = new StringBuilder();
            // Header.
            style.AppendLine(
                ExcelUtilities.Indent2 + 
                @"<Style ss:ID =" + ExcelUtilities.Quote + ID + ExcelUtilities.Quote + ">");
            // Font.
            style.AppendLine(Font.WriteFont());
            // Alignment.
            style.AppendLine(Alignment.WriteAlignment());
            // Background (if it was defined).
            if (!String.IsNullOrEmpty(Background.BackgroundColor))
            {
                style.AppendLine(Background.WriteInterior());
            }
            // If has borders...
            if (Borders.Count > 0)
            {
                //Header.
                style.AppendLine(ExcelUtilities.Indent4 + @"<Borders>");
                // Write borders.
                foreach (StyleBorder border in Borders)
                {
                    style.AppendLine(border.WriteBorder());
                }
                //Footer.
                style.AppendLine(ExcelUtilities.Indent4 + @"</Borders>");
            }
            // Number format.
            if (CellNumberFormat != null)
            {
                style.Append(CellNumberFormat.WriteNumberFormat());
            }
            // Footer.
            style.AppendLine(ExcelUtilities.Indent2 + @"</Style>");

            return style.ToString();
        }

        #endregion

    }
}
