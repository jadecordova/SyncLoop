using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncLoopRTFLibrary
{
    public class RTFFontTable
    {

        #region --------------------------------------------------------------------------------< PROPERTIES >

        public string[] Fonts { get; set; }

        #endregion

        #region --------------------------------------------------------------------------------< CONSTRUCTORS >

        /// <summary>
        /// List of fonts.
        /// </summary>
        public RTFFontTable(string[] fonts)
        {
            Fonts = fonts;
        }

        #endregion

        #region --------------------------------------------------------------------------------< METHODS >

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
