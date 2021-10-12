using System.Text;

namespace SyncLoopLibrary
{
    /// <summary>
    /// Defines background of cell.
    /// </summary>
    public class StyleInterior
    {

        #region ENUMERATIONS

        /// <summary>
        /// Background pattern of cell.
        /// </summary>
        public enum InteriorPattern
        {
            Solid
        }

        #endregion



        #region PROPERTIES

        /// <summary>
        /// Background color of cell.
        /// </summary>
        public string BackgroundColor { get; set; }

        /// <summary>
        /// Background pattern of cell.
        /// </summary>
        public InteriorPattern BackgroundPattern { get; set; }

        #endregion


        
        #region CONSTRUCTORS

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="backgroundColor">Background color.</param>
        /// <param name="backgroundPattern">Background pattern.</param>
        public StyleInterior(string backgroundColor, InteriorPattern backgroundPattern)
        {
            BackgroundColor = backgroundColor;
            BackgroundPattern = backgroundPattern;
        }

        #endregion



        #region METHODS

        /// <summary>
        /// Creates cell interior.
        /// </summary>
        /// <returns>Cell interior string.</returns>
        public string WriteInterior()
        {
            // Result constructor.
            StringBuilder interior = new StringBuilder();
            // Write.
            interior.Append(
                ExcelUtilities.Indent3 + 
                @"<Interior ss:Color=" + ExcelUtilities.Quote + BackgroundColor + ExcelUtilities.Quote + 
                " ss:Pattern=" + ExcelUtilities.Quote + BackgroundPattern.ToString() + ExcelUtilities.Quote + " />");

            return interior.ToString();
        }

        #endregion
    }
}
