using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncLoopExcelLibrary
{
    /// <summary>
    /// Defines text alignment whitin cell.
    /// </summary>
    public class StyleAlignment
    {

        #region -----------------------------------------------------------------ENUMERATIONS

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

        #region --------------------------------------------------------------------------------PROPERTIES

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

        #region --------------------------------------------------------------------------------CONSTRUCTORS

        public StyleAlignment(HorizontalAlignment horizontal, VerticalAlignment vertical)
        {
            AlignmentHorizontal = horizontal;
            AlignmentVertical = vertical;
            TextWrap = TextWrapping.No;
        }

        public StyleAlignment(HorizontalAlignment horizontal, VerticalAlignment vertical, TextWrapping wrapping)
        {
            AlignmentHorizontal = horizontal;
            AlignmentVertical = vertical;
            TextWrap = wrapping;
        }

        #endregion

        #region --------------------------------------------------------------------------------METHODS

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
