using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncLoopExcelLibrary
{
    /// <summary>
    /// Layout definition for page setup class.
    /// </summary>
    public class Layout
    {

        #region ----------------------------------------------------------------------ENUMERATIONS

        public enum Orientation
        {
            Portrait,
            Landscape
        }

        #endregion

        #region ----------------------------------------------------------------------PROPERTIES

        /// <summary>
        /// Document centered horizontally on page.
        /// </summary>
        public bool CenterHorizontal { get; set; }

        /// <summary>
        /// Document centered vertically on page.
        /// </summary>
        public bool CenterVertical { get; set; }

        /// <summary>
        /// Portrait or Landscape.
        /// </summary>
        public Orientation DocumentOrientation { get; set; }

        /// <summary>
        /// Number to start page numbering.
        /// </summary>
        public int StartPageNumber { get; set; }

        #endregion

        #region ----------------------------------------------------------------------CONSTRUCTORS

        public Layout()
        {
            CenterHorizontal = false;
            CenterVertical = false;
            DocumentOrientation = Orientation.Portrait;
            StartPageNumber = -1;
        }

        public Layout(bool centerHorizontal)
        {
            CenterHorizontal = centerHorizontal;
            CenterVertical = false;
            DocumentOrientation = Orientation.Portrait;
            StartPageNumber = -1;
        }

        public Layout(bool centerHorizontal, bool centerVertical)
        {
            CenterHorizontal = centerHorizontal;
            CenterVertical = centerVertical;
            DocumentOrientation = Orientation.Portrait;
            StartPageNumber = -1;
        }

        public Layout(bool centerHorizontal, bool centerVertical, Orientation documentOrientation)
        {
            CenterHorizontal = centerHorizontal;
            CenterVertical = centerVertical;
            DocumentOrientation = documentOrientation;
            StartPageNumber = -1;
        }

        public Layout(bool centerHorizontal, bool centerVertical, Orientation documentOrientation, int startPageNumber)
        {
            CenterHorizontal = centerHorizontal;
            CenterVertical = centerVertical;
            DocumentOrientation = documentOrientation;
            StartPageNumber = startPageNumber;
        }

        #endregion

        #region ----------------------------------------------------------------------METHODS

        public string WriteLayout()
        {
            // Result constructor
            StringBuilder layout = new StringBuilder();
            // Header.
            layout.Append(ExcelUtilities.Indent5 + @"<Layout");
            // Center horizontal.
            if (CenterHorizontal == true)
            {
                layout.Append(@" x:CenterHorizontal=" + ExcelUtilities.Quote + 1 + ExcelUtilities.Quote);
            }
            // Center horizontal.
            if (CenterVertical == true)
            {
                layout.Append(@" x:CenterVertical=" + ExcelUtilities.Quote + 1 + ExcelUtilities.Quote);
            }
            // Orientation.
            layout.Append(@" x:Orientation=" + ExcelUtilities.Quote + DocumentOrientation.ToString() + ExcelUtilities.Quote);
            // Center horizontal.
            if (StartPageNumber != -1)
            {
                layout.Append(@" x:StartPageNumber=" + ExcelUtilities.Quote + StartPageNumber.ToString() + ExcelUtilities.Quote);
            }
            // Footer
            layout.AppendLine(@"/>");

            return layout.ToString();
        }

        #endregion
    }
}
