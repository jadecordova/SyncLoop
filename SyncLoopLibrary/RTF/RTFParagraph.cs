using System;
using System.Text;

namespace SyncLoopLibrary
{
    /// <summary>
    /// Defines a RTF paragraph.
    /// </summary>
    public class RTFParagraph
    {

        #region PROPERTIES

        /// <summary>
        /// Content string.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Paragraph options.
        /// </summary>
        public RTFParagraphOptions Options { get; set; }

        /// <summary>
        /// Content that should be rendered plain.
        /// </summary>
        public string ContentPlain { get; set; } = null;

        /// <summary>
        /// Content to be rendered in bold face.
        /// </summary>
        public string ContentBold { get; set; } = null;

        /// <summary>
        /// Indicates if the paragraph is a loop definition.
        /// </summary>
        public bool IsLoop { get; set; } = false;

        #endregion



        #region CONSTRUCTORS

        /// <summary>
        /// Default constructor.
        /// </summary>
        public RTFParagraph()
        {

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="content">Content string.</param>
        /// <param name="options">Paragraph options.</param>
        public RTFParagraph(string content, RTFParagraphOptions options)
        {
            Content = content;
            Options = options;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="contentPlain">Content to be rendered in plain face.</param>
        /// <param name="contentBold">Content to be rendered in bold face.</param>
        /// <param name="options">Paragraph options.</param>
        public RTFParagraph(string contentPlain, string contentBold, RTFParagraphOptions options)
        {
            ContentPlain = contentPlain;
            ContentBold = contentBold;
            Options = options;
        }

        #endregion



        #region METHODS

        /// <summary>
        /// Creates paragraph.
        /// </summary>
        /// <returns>Paragraph string.</returns>
        public string WriteParagraph()
        {
            // Result builder.
            StringBuilder result = new StringBuilder();
            // Begin paragraph.
            if(Options != null)
            {
                result.Append(Environment.NewLine + @"{\pard" + Options.WriteProperties() + Environment.NewLine);
            }
            // Is it a loop paragraph?
            if(!String.IsNullOrEmpty(ContentBold) && !String.IsNullOrEmpty(ContentPlain))
            {
                result.Append(ContentPlain + @" {\b " + ContentBold + @"}");
            }
            else
            {
                // Insert content.
                result.Append(Content);
            }
            // End paragraph.
            result.Append(Environment.NewLine + @"\par}");
            // Convert and return.
            return result.ToString();
        }

        #endregion
    }
}
