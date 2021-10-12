using System;
using System.Text;

namespace SyncLoopLibrary
{
    /// <summary>
    /// Defines a RTF font table.
    /// </summary>
    public class RTFFontTable
    {

        #region PROPERTIES

        /// <summary>
        /// Array of fonts.
        /// </summary>
        public string[] Fonts { get; set; }

        #endregion



        #region CONSTRUCTORS

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="fonts">Array of fonts.</param>
        public RTFFontTable(string[] fonts)
        {
            Fonts = fonts;
        }

        #endregion



        #region METHODS

        /// <summary>
        /// Creates font table.
        /// </summary>
        /// <returns>Font table string.</returns>
        public string WriteFontTable()
        {
            // Result constructor.
            StringBuilder result = new StringBuilder();
            // New line.
            result.Append(Environment.NewLine);
            // Open font table.
            result.Append(@"{\fonttbl ");
            // New line.
            result.Append(Environment.NewLine);
            // Counter for font id.
            int fontNumber = 0;
            // Iterate.
            if (Fonts != null && Fonts.Length > 0)
            {
                // For each font...
                foreach(string font in Fonts)
                {
                    if (!String.IsNullOrEmpty(font))
                    {
                        // Add font.
                        result.Append(@"{\f" + fontNumber + @"\fnil " + font + ";}");
                        // New line.
                        result.Append(Environment.NewLine);
                        // Increase counter.
                        fontNumber++;
                    }
                }
                // Close fonts table.
                result.Append(@"}");
            }
            else
            {
                // New line.
                result.Append(Environment.NewLine);
                // Return a default font.
                result.Append(@"{\fonttbl{\f0\fnil Courier new}}");
            }
            // Convert to string and return.
            return result.ToString();
        }

        #endregion
    }
}
