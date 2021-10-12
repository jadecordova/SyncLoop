using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncLoopRTFLibrary
{
    public class RTFParagraphOptions
    {

        #region --------------------------------------------------------------------------------< PROPERTIES >

        public string Language { get; set; } = @"\lang8202";
        public RTFJustification Justification { get; set; } = RTFJustification.Justified;
        public int FontSize { get; set; } = 0;
        public bool PageBreakBefore { get; set; } = false;
        public bool DoubleSpacing { get; set; } = false;
        public bool IsBold { get; set; }
        public bool IsItalics { get; set; }
        public bool DoNotSpellCheck { get; set; } = false;
        public double SpaceBefore { get; set; } = 0;
        public double SpaceAfter { get; set; } = 0;
        public double FirstLineIndentation { get; set; } = 0;
        public double LeftIndentation { get; set; } = 0;
        public double RightIndentation { get; set; } = 0;
        public int ParagraphColor { get; set; } = 0;

        #endregion

        #region --------------------------------------------------------------------------------< CONSTRUCTORS >

        public RTFParagraphOptions() {}

        #endregion

        #region --------------------------------------------------------------------------------< METHODS >

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
