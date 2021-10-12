using System.Text;

namespace SyncLoopLibrary
{
    /// <summary>
    /// Defines a RTF color.
    /// </summary>
    public class RTFColor
    {

        #region PROPERTIES

        /// <summary>
        /// Red component.
        /// </summary>
        public byte Red { get; set; }

        /// <summary>
        /// Green component.
        /// </summary>
        public byte Green { get; set; }

        /// <summary>
        /// Blue component.
        /// </summary>
        public byte Blue { get; set; }

        #endregion



        #region CONSTRUCTORS

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="red">Red value.</param>
        /// <param name="green">Green value.</param>
        /// <param name="blue">Blue value.</param>
        public RTFColor(byte red, byte green, byte blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }

        #endregion



        #region METHODS

        /// <summary>
        /// Creates color.
        /// </summary>
        /// <returns>Color string.</returns>
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
