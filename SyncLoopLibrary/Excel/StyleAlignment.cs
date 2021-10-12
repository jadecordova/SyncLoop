using System.Text;

namespace SyncLoopLibrary
{
    /// <summary>
    /// Defines text alignment whitin cell.
    /// </summary>
    public class StyleAlignment
    {

        #region ENUMERATIONS

        /// <summary>
        /// Horizontal alignment.
        /// </summary>
        public enum HorizontalAlignment
        {
            Left,
            Center,
            Right
        }

        /// <summary>
        /// Vertical alignment.
        /// </summary>
        public enum VerticalAlignment
        {
            Top,
            Center,
            Bottom
        }

        /// <summary>
        /// Text wrap.
        /// </summary>
        public enum TextWrapping
        {
            No,
            Yes
        }

        #endregion



        #region PROPERTIES

        /// <summary>
        /// Horizontal alignment of cell.
        /// </summary>
        public HorizontalAlignment AlignmentHorizontal { get; set; }

        /// <summary>
        /// Vertical alignment of cell.
        /// </summary>
        public VerticalAlignment AlignmentVertical { get; set; }

        /// <summary>
        /// Text wrap.
        /// </summary>
        public TextWrapping TextWrap { get; set; }

        #endregion



        #region CONSTRUCTORS

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="horizontal">Horizontal alignment.</param>
        /// <param name="vertical">Vertical alignment.</param>
        public StyleAlignment(HorizontalAlignment horizontal, VerticalAlignment vertical)
        {
            AlignmentHorizontal = horizontal;
            AlignmentVertical = vertical;
            TextWrap = TextWrapping.No;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="horizontal">Horizontal alignment.</param>
        /// <param name="vertical">Vertical alignment.</param>
        /// <param name="wrapping">Text wrapping.</param>
        public StyleAlignment(HorizontalAlignment horizontal, VerticalAlignment vertical, TextWrapping wrapping)
        {
            AlignmentHorizontal = horizontal;
            AlignmentVertical = vertical;
            TextWrap = wrapping;
        }

        #endregion



        #region METHODS

        /// <summary>
        /// Creates alignment.
        /// </summary>
        /// <returns>Alignment string.</returns>
        public string WriteAlignment()
        {
            // Result constructor.
            StringBuilder alignment = new StringBuilder();
            // Write.
            alignment.Append(
                ExcelUtilities.Indent3 + 
                @"<Alignment ss:Horizontal=" + ExcelUtilities.Quote + AlignmentHorizontal.ToString() + ExcelUtilities.Quote + 
                @" ss:Vertical=" + ExcelUtilities.Quote + AlignmentVertical.ToString() + ExcelUtilities.Quote + 
                @" ss:WrapText=" + ExcelUtilities.Quote + ((int)TextWrap).ToString() + ExcelUtilities.Quote + " />"
                );

            return alignment.ToString();
        }

        #endregion
    }
}
