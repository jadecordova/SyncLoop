namespace SyncLoopLibrary
{
    /// <summary>
    /// Aligment of subtitle on screen.
    /// </summary>
    public enum SubtitleAlignment
    {
        /// <summary>
        /// Left aligned.
        /// </summary>
        Left,
        /// <summary>
        /// Center aligned.
        /// </summary>
        Center,
        /// <summary>
        /// Right aligned.
        /// </summary>
        Right
    }

    /// <summary>
    /// Csontructor.
    /// </summary>
    public class Subtitle
    {
        /// <summary>
        /// Line to place subtitle.
        /// </summary>
        public int Line { get; set; } = 0;

        /// <summary>
        /// Flag for 1st line in italics.
        /// </summary>
        public bool FirstLineItalics { get; set; } = false;

        /// <summary>
        /// Flag for 2nd line in italics.
        /// </summary>
        public bool SecondLineItalics { get; set; } = false;

        /// <summary>
        /// Subtitle aligment.
        /// </summary>
        public SubtitleAlignment Alignment { get; set; } = SubtitleAlignment.Center;

    }
}
