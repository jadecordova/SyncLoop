using System.Collections.Generic;
using System.Text;

namespace SyncLoopLibrary
{
    /// <summary>
    /// Colection of styles for document.
    /// </summary>
    public class DocumentStyles
    {

        #region PROPERTIES

        /// <summary>
        /// Colection of styles.
        /// </summary>
        public List<Style> Styles { get; set; }

        #endregion



        #region CONSTRUCTORS

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="stylesArray">Array of style objects.</param>
        public DocumentStyles(Style[] stylesArray)
        {
            // if the array exists...
            if (stylesArray != null)
            {
                Styles = new List<Style>();
                Styles.AddRange(stylesArray);
            }
        }

        #endregion



        #region METHODS

        /// <summary>
        /// Creates document styles.
        /// </summary>
        /// <returns>Styles string.</returns>
        public string WriteDocumentSytles()
        {
            // Result constructor.
            StringBuilder styles = new StringBuilder();

            // If the list is not empty...
            if (Styles != null)
            {
                // Header.
                styles.AppendLine(ExcelUtilities.Indent1 + @"<Styles>");
                // Write styles.
                foreach (Style style in Styles)
                {
                    styles.Append(style.WriteStyle());
                }
                // Footer
                styles.AppendLine(ExcelUtilities.Indent1 + @"</Styles>");
            }

            return styles.ToString();
        }

        #endregion
    }
}
