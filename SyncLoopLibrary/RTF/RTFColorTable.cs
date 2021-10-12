using System;
using System.Collections.Generic;
using System.Text;

namespace SyncLoopLibrary
{
    /// <summary>
    /// Defines a RTF color table.
    /// </summary>
    public class RTFColorTable
    {

        #region PROPERTIES

        /// <summary>
        /// List of RTF colors.
        /// </summary>
        public List<RTFColor> Colors { get; set; }

        #endregion



        #region CONSTRUCTORS

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="colors">List of colors.</param>
        public RTFColorTable(List<RTFColor> colors = null)
        {
            Colors = colors;
        }

        #endregion



        #region METHODS

        /// <summary>
        /// Creates color table.
        /// </summary>
        /// <returns>Color table string.</returns>
        public string WriteColorTable()
        {
            // Result builder.
            StringBuilder result = new StringBuilder();
            // New line.
            result.Append(Environment.NewLine);
            // Header.
            result.Append(@"{\colortbl");
            // New line.
            result.Append(Environment.NewLine);
            // Check if Colors array is set.
            if(Colors != null && Colors.Count > 0)
            {
                foreach(RTFColor color in Colors)
                {
                    // Write each color.
                    result.Append(color.WriteColor());
                    // New line.
                    result.Append(Environment.NewLine);
                }
            }
            // Footer.
            result.Append(@"}");
            // Convert and return.
            return result.ToString();
        }

        #endregion
    }
}
