using System;
using System.Text;

namespace SyncLoopLibrary
{
    /// <summary>
    /// Defines RTF paragraph options.
    /// </summary>
    public class RTFParagraphOptions
    {

        #region PROPERTIES

        /// <summary>
        /// Paragraph language.
        /// </summary>
        public string Language { get; set; } = @"\lang8202";

        /// <summary>
        /// Paragraph justification.
        /// </summary>
        public RTFJustification Justification { get; set; } = RTFJustification.Justified;

        /// <summary>
        /// Paragraph font size.
        /// </summary>
        public int FontSize { get; set; } = 0;

        /// <summary>
        /// Page break before.
        /// </summary>
        public bool PageBreakBefore { get; set; } = false;

        /// <summary>
        /// Paragraph double spacing.
        /// </summary>
        public bool DoubleSpacing { get; set; } = false;

        /// <summary>
        /// Paragraph is bold.
        /// </summary>
        public bool IsBold { get; set; }

        /// <summary>
        /// Paragraph is italics.
        /// </summary>
        public bool IsItalics { get; set; }

        /// <summary>
        /// Paragraph should not be spell checked.
        /// </summary>
        public bool DoNotSpellCheck { get; set; } = false;

        /// <summary>
        /// Space before.
        /// </summary>
        public double SpaceBefore { get; set; } = 0;

        /// <summary>
        /// Space after.
        /// </summary>
        public double SpaceAfter { get; set; } = 0;

        /// <summary>
        /// Paragraph first line indentation.
        /// </summary>
        public double FirstLineIndentation { get; set; } = 0;

        /// <summary>
        /// Paragraph left indentation.
        /// </summary>
        public double LeftIndentation { get; set; } = 0;

        /// <summary>
        /// Paragraph right indentation.
        /// </summary>
        public double RightIndentation { get; set; } = 0;

        /// <summary>
        /// Paragraph color.
        /// </summary>
        public int ParagraphColor { get; set; } = 0;

        #endregion



        #region CONSTRUCTORS

        /// <summary>
        /// Default constructor.
        /// </summary>
        public RTFParagraphOptions() {}

        #endregion



        #region METHODS

        /// <summary>
        /// Creates paragraph.
        /// </summary>
        /// <returns>Paragraph string.</returns>
        public string WriteProperties()
        {
            // Result builder.
            StringBuilder result = new StringBuilder();
            // Bold.
            if (IsBold) result.Append(@"\b");
            // Italics.
            if (IsItalics) result.Append(@"\i");
            // Select justification.
            string justification = String.Empty;
            // Set justification.
            switch (Justification)
            {
                case RTFJustification.Left:
                    justification = @"\ql";
                    break;
                case RTFJustification.Center:
                    justification = @"\qc";
                    break;
                case RTFJustification.Right:
                    justification = @"\qr";
                    break;
                case RTFJustification.Justified:
                    justification = @"\qj";
                    break;
            }
            // Justification.
            result.Append(justification);
            // Font size.
            if (FontSize > 0)
            {
                result.Append(@"\fs" + (FontSize * 2));
            }
            // Page break.
            if (PageBreakBefore)
            {
                result.Append(@"\pagebb");
            }
            // Double space.
            if (DoubleSpacing)
            {
                result.Append(@"\sl480\");
            }
            // Space before.
            if (SpaceBefore > 0)
            {
                result.Append(@"\sb" + ((int)Math.Round(SpaceBefore * RTFUtilities.TwipsPerInch)).ToString());
            }
            // Space after.
            if (SpaceAfter > 0)
            {
                result.Append(@"\sa" + ((int)Math.Round(SpaceAfter * RTFUtilities.TwipsPerInch)).ToString());
            }
            // First line indentation.
            if (FirstLineIndentation > 0)
            {
                result.Append(@"\fi" + ((int)Math.Round(FirstLineIndentation * RTFUtilities.TwipsPerInch)).ToString());
            }
            // Left indentation.
            if (LeftIndentation > 0)
            {
                result.Append(@"\li" + ((int)Math.Round(LeftIndentation * RTFUtilities.TwipsPerInch)).ToString());
            }
            // Right indentation.
            if (RightIndentation > 0)
            {
                result.Append(@"\ri" + ((int)Math.Round(RightIndentation * RTFUtilities.TwipsPerInch)).ToString());
            }
            // Paragraph color.
            result.Append(@"\cf" + ParagraphColor);
            // Convert and return.
            return result.ToString();

        }

        #endregion
    }
}
