using System;
using System.Text;

namespace SyncLoopLibrary
{
    /// <summary>
    /// Header for page setup class.
    /// </summary>
    public class Header
    {

        #region PROPERTIES

        /// <summary>
        /// REQUIRED. Header margin.
        /// </summary>
        public double HeaderMargin { get; set; }

        /// <summary>
        /// OPTIONAL. Data in CSS format for Excel 2000.
        /// </summary>
        public string HeaderData { get; set; }

        #endregion



        #region CONSTRUCTORS

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="margin">Margin.</param>
        public Header(double margin)
        {
            HeaderMargin = margin;
            HeaderData = String.Empty;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="margin">Margin.</param>
        /// <param name="data">Header data.</param>
        public Header(double margin, string data)
        {
            HeaderMargin = margin;
            HeaderData = data;
        }

        #endregion



        #region METHODS

        /// <summary>
        /// Creates header.
        /// </summary>
        /// <returns>Header string.</returns>
        public string WriteHeader()
        {
            // Result constructor.
            StringBuilder header = new StringBuilder();
            // Header.
            header.Append(ExcelUtilities.Indent5 + @"<Header");
            // Margin
            header.Append(@" x:Margin=" + ExcelUtilities.Quote + HeaderMargin + ExcelUtilities.Quote);
            // Data.
            if (!String.IsNullOrEmpty(HeaderData))
            {
                header.Append(@" x:Data=" + ExcelUtilities.Quote + HeaderData + ExcelUtilities.Quote);
            }
            // Footer
            header.AppendLine(@"/>");

            return header.ToString();
        }

        #endregion
    }
}
