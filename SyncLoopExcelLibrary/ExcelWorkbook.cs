using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncLoopExcelLibrary
{
    /// <summary>
    /// Excel document properties.
    /// </summary>
    public class ExcelWorkbook
    {

        #region -----------------------------------------------------------------< PROPERTIES >

        /// <summary>
        /// Window height in twips (1440 per inch).
        /// </summary>
        public string WindowHeight { get; set; }

        /// <summary>
        /// Window width.
        /// </summary>
        public string WindowWidth { get; set; }

        /// <summary>
        /// Horizontal top position.
        /// </summary>
        public string WindowTopX { get; set; }

        /// <summary>
        /// Vertical top position.
        /// </summary>
        public string WindowTopY { get; set; }

        /// <summary>
        /// Active worksheet.
        /// </summary>
        public string ActiveSheet { get; set; }

        /// <summary>
        /// Protect sctructure.
        /// </summary>
        public string ProtectStructure { get; set; }

        /// <summary>
        /// Protect windows.
        /// </summary>
        public string ProtectWindows { get; set; }

        #endregion

        #region -----------------------------------------------------------------< CONSTRUCTORS >

        public ExcelWorkbook()
        {
            // Default values.
            WindowHeight = "9570";
            WindowWidth = "10365";
            WindowTopX = "0";
            WindowTopY = "0";
            ActiveSheet = "1";
            ProtectStructure = "False";
            ProtectWindows = "False";
        }

        #endregion

        #region -----------------------------------------------------------------< METHODS >

        public string WriteExcelDocument()
        {
            // Result constructor.
            StringBuilder excel = new StringBuilder();
            // Header.
            excel.AppendLine(ExcelUtilities.Indent1 + @"<ExcelWorkbook xmlns=" + "\"" + "urn:schemas-microsoft-com:office:excel" + "\"" + ">");
            // Window height.
            excel.AppendLine(ExcelUtilities.Indent2 + @"<WindowHeight>" + WindowHeight + "</WindowHeight>");
            // Window widht.
            excel.AppendLine(ExcelUtilities.Indent2 + @"<WindowWidth>" + WindowWidth + "</WindowWidth>");
            // Top X.
            excel.AppendLine(ExcelUtilities.Indent2 + @"<WindowTopX>" + WindowTopX + "</WindowTopX>");
            // Top Y.
            excel.AppendLine(ExcelUtilities.Indent2 + @"<WindowTopY>" + WindowTopY + "</WindowTopY>");
            // Active sheet.
            excel.AppendLine(ExcelUtilities.Indent2 + @"<ActiveSheet>" + ActiveSheet + "</ActiveSheet>");
            // Protected structure.
            excel.AppendLine(ExcelUtilities.Indent2 + @"<ProtectStructure>" + ProtectStructure + "</ProtectStructure>");
            // Protected windows.
            excel.AppendLine(ExcelUtilities.Indent2 + @"<ProtectWindows>" + ProtectWindows + "</ProtectWindows>");
            // Footer.
            excel.AppendLine(ExcelUtilities.Indent1 + @"</ExcelWorkbook>");

            return excel.ToString();
        }

        #endregion
    }
}
