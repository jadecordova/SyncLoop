using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncLoopRTFLibrary
{
    public class RTFColor
    {

        #region --------------------------------------------------------------------------------< PROPERTIES >

        public byte Red { get; set; }
        public byte Green { get; set; }
        public byte Blue { get; set; }

        #endregion

        #region --------------------------------------------------------------------------------< CONSTRUCTORS >

        public RTFColor(byte red, byte green, byte blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }

        #endregion

        #region --------------------------------------------------------------------------------< METHODS >

        public string WriteColor()
        {
            // Result constructor.
            StringBuilder result = new StringBuilder();
            // Add colors.
            result.Append(@"\red" + Red.ToString());
            result.Append(@"\green" + Green.ToString());
            result.Append(@"\blue" + Blue.ToString());
            // Add semicolon at the end.
            result.Append(@";");
            // Convert and return.
            return result.ToString();
        }

        #endregion
    }
}
