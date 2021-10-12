using System.Text;

namespace SyncLoopLibrary
{
    /// <summary>
    /// Layout definition for page setup class.
    /// </summary>
    public class Layout
    {

        #region ENUMERATIONS

        /// <summary>
        /// Page orientation options.
        /// </summary>
        public enum Orientation
        {
            /// <summary>
            /// Vertical.
            /// </summary>
            Portrait,
            /// <summary>
            /// Horizontal.
            /// </summary>
            Landscape
        }

        #endregion



        #region PROPERTIES

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



        #region CONSTRUCTORS

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Layout()
        {
            CenterHorizontal = false;
            CenterVertical = false;
            DocumentOrientation = Orientation.Portrait;
            StartPageNumber = -1;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="centerHorizontal">Document is centered horizontally.</param>
        public Layout(bool centerHorizontal)
        {
            CenterHorizontal = centerHorizontal;
            CenterVertical = false;
            DocumentOrientation = Orientation.Portrait;
            StartPageNumber = -1;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="centerHorizontal">Document is centered horizontally.</param>
        /// <param name="centerVertical">Document is centered vertically.</param>
        public Layout(bool centerHorizontal, bool centerVertical)
        {
            CenterHorizontal = centerHorizontal;
            CenterVertical = centerVertical;
            DocumentOrientation = Orientation.Portrait;
            StartPageNumber = -1;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="centerHorizontal">Document is centered horizontally.</param>
        /// <param name="centerVertical">Document is centered vertically.</param>
        /// <param name="documentOrientation">Document orientation.</param>
        public Layout(bool centerHorizontal, bool centerVertical, Orientation documentOrientation)
        {
            CenterHorizontal = centerHorizontal;
            CenterVertical = centerVertical;
            DocumentOrientation = documentOrientation;
            StartPageNumber = -1;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="centerHorizontal">Document is centered horizontally.</param>
        /// <param name="centerVertical">Document is centered vertically.</param>
        /// <param name="documentOrientation">Document orientation.</param>
        /// <param name="startPageNumber">Initial page number.</param>
        public Layout(bool centerHorizontal, bool centerVertical, Orientation documentOrientation, int startPageNumber)
        {
            CenterHorizontal = centerHorizontal;
            CenterVertical = centerVertical;
            DocumentOrientation = documentOrientation;
            StartPageNumber = startPageNumber;
        }

        #endregion



        #region METHODS

        /// <summary>
        /// Creates layout.
        /// </summary>
        /// <returns>Layout string.</returns>
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
