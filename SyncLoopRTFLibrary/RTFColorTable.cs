using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncLoopRTFLibrary
{
    public class RTFColorTable
    {

        #region --------------------------------------------------------------------------------< PROPERTIES >

        public List<RTFColor> Colors { get; set; }

        #endregion

        #region --------------------------------------------------------------------------------< CONSTRUCTORS >

        public RTFColorTable(List<RTFColor> colors = null)
        {
            Colors = colors;
        }

        #endregion

        #region --------------------------------------------------------------------------------< PROPERTIES >

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
