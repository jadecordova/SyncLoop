using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncLoopRTFLibrary
{
    public class RTFProperties
    {

        #region --------------------------------------------------------------------------------< PROPERTIES >

        /// <summary>
        /// Zoom of the document.
        /// </summary>
        public string Zoom { get; set; } = @"\viewscale200";

        /// <summary>
        /// Document type RTF version 1.
        /// </summary>
        public string DocType { get; set; } = @"\rtf1";

        /// <summary>
        /// Character set.
        /// </summary>
        public string CharacterSet { get; set; } = @"\ansi";

        /// <summary>
        /// Default font of document as font 0 in fonts table.
        /// </summary>
        public string DefaultFont { get; set; } = @"\deff0";

        /// <summary>
        /// Fonts table.
        /// </summary>
        public RTFFontTable Fonts { get; set; }

        /// <summary>
        /// Color table.
        /// </summary>
        public RTFColorTable ColorTable { get; set; }

        // Info group properties.

        /// <summary>
        /// Document title in English.
        /// </summary>
        public string DocumentTitleEnglish { get; set; }

        /// <summary>
        /// Document title in Spanish.
        /// </summary>
        public string DocumentTitleSpanish { get; set; }

        /// <summary>
        /// Author.
        /// </summary>
        public string DocumentAuthor { get; set; }

        /// <summary>
        /// Company.
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// Creation date.
        /// </summary>
        public DateTime CreationDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Comments.
        /// </summary>
        public string Comment { get; set; }

        // Preliminaries.

        /// <summary>
        /// Language code.
        /// </summary>
        public RTFLanguages LanguageCode { get; set; } = RTFLanguages.Spanish_Venezuela;

        /// <summary>
        /// Content font size.
        /// </summary>
        public int ContentFontSize { get; set; } = 14;

        /// <summary>
        /// Headers and footers font size.
        /// </summary>
        public int HeadersAndFootersFontSize { get; set; } = 10;

        /// <summary>
        /// Margin top.
        /// </summary>
        public double MarginTop { get; set; } = 0.5d;

        /// <summary>
        /// Margin bottom.
        /// </summary>
        public double MarginBottom { get; set; } = 0.5d;

        /// <summary>
        /// Margin left.
        /// </summary>
        public double MarginLeft { get; set; } = 0.5d;

        /// <summary>
        /// Margin right.
        /// </summary>
        public double MarginRight { get; set; } = 0.5d;

        /// <summary>
        /// Paper height.
        /// </summary>
        public double PaperHeight { get; set; } = 11;

        /// <summary>
        /// Paper width.
        /// </summary>
        public double PaperWidht { get; set; } = 8.5;

        #endregion

        #region --------------------------------------------------------------------------------< CONSTRUCTORS >

        /// <summary>
        /// Default constructor.
        /// </summary>
        public RTFProperties()
        {

        }

        /// <summary>
        /// Constructor with font table and color table.
        /// </summary>
        /// <param name="fonts">Fonts.</param>
        public RTFProperties(RTFFontTable fonts, RTFColorTable colorTable = null)
        {
            Fonts = fonts;
            ColorTable = colorTable;
        }

        #endregion

        #region --------------------------------------------------------------------------------< METHODS >

        public string WriteProperties()
        {
            // Result constructor.
            StringBuilder result = new StringBuilder();
            // Add document type.
            result.Append(DocType);
            // Add character set.
            result.Append(CharacterSet);
            // Add default font.
            result.Append(DefaultFont);
            // Add zoom
            result.Append(Zoom);
            // Add fonts table.
            result.Append(Fonts.WriteFontTable());
            // Color table.
            if (ColorTable != null) result.Append(ColorTable.WriteColorTable());
            // New line.
            result.Append(Environment.NewLine);

            // ------------------------------------------< INFO GROUP >

            result.Append(@"{\info");
            // New line.
            result.Append(Environment.NewLine);
            // Check for info group values.
            if (!String.IsNullOrEmpty(DocumentTitleEnglish))
            {
                result.Append(@"{\title " + DocumentTitleEnglish + @"}");
                // New line.
                result.Append(Environment.NewLine);
            }
            if (!String.IsNullOrEmpty(DocumentAuthor))
            {
                result.Append(@"{\author " + DocumentAuthor + @"}");
                // New line.
                result.Append(Environment.NewLine);
            }
            if (!String.IsNullOrEmpty(Company))
            {
                result.Append(@"{\company " + Company + @"}");
                // New line.
                result.Append(Environment.NewLine);
            }
            // Creation date.
            result.Append(@"{\creatim\yr" + CreationDate.Year + 
                @"\mo" + CreationDate.Month + 
                @"\dy" + CreationDate.Day +
                @"\hr" + CreationDate.Hour +
                @"\min" + CreationDate.Minute + @"}");
            // New line.
            result.Append(Environment.NewLine);
            // Comment.
            if (!String.IsNullOrEmpty(Comment))
            {
                result.Append(@"{\doccomm " + Comment + @"}");
                // New line.
                result.Append(Environment.NewLine);
            }
            // Close info group.
            result.Append(@"}");
            // New line.
            result.Append(Environment.NewLine);

            // ------------------------------------------< PRELIMINARIES >

            // Language.
            result.Append(@"\deflang" + (int)LanguageCode);
            // Reset formattings.
            result.Append(@"\plain");
            // Default font size.
            result.Append(@"\fs" + (ContentFontSize * 2));
            // New line.
            result.Append(Environment.NewLine);
            // Paper width.
            result.Append(@"\paperw" + ((int)(PaperWidht * RTFUtilities.TwipsPerInch)).ToString());
            // Paper height.
            result.Append(@"\paperh" + ((int)(PaperHeight * RTFUtilities.TwipsPerInch)).ToString());
            // New line.
            result.Append(Environment.NewLine);
            // Margin top.
            result.Append(@"\margt" + ((int)(MarginTop * RTFUtilities.TwipsPerInch)).ToString());
            // Margin bottom.
            result.Append(@"\margb" + ((int)(MarginBottom * RTFUtilities.TwipsPerInch)).ToString());
            // Margin top.
            result.Append(@"\margl" + ((int)(MarginLeft * RTFUtilities.TwipsPerInch)).ToString());
            // Margin top.
            result.Append(@"\margr" + ((int)(MarginRight * RTFUtilities.TwipsPerInch)).ToString());
            // New line.
            result.Append(Environment.NewLine);
            // Header.
            result.Append(@"{\header\pard\ql\plain\f2\fs" + (HeadersAndFootersFontSize * 2).ToString());
            // Right flushed tab
            // Calculate tab position.
            int tabPosition = (int)Math.Round((PaperWidht - MarginRight - MarginLeft) * RTFUtilities.TwipsPerInch);
            result.Append(@"\tqr\tx" + tabPosition.ToString());
            result.Append(Environment.NewLine);
            result.Append(DocumentTitleEnglish + @" \tab " + DocumentTitleSpanish);
            result.Append(Environment.NewLine);
            result.Append(@"\par}");
            // New line.
            result.Append(Environment.NewLine);
            result.Append(@"{\footer\pard\ql\plain\f0\fs16");
            // Footer.
            // Right flushed tab
            result.Append(@"\tqr\tx" + tabPosition.ToString());
            result.Append(Environment.NewLine);
            result.Append(Company + @" \tab " + @" \chpgn");
            result.Append(Environment.NewLine);
            result.Append(@"\par}");
            // Convert and return.
            return result.ToString();
        }

        #endregion
    }
}
