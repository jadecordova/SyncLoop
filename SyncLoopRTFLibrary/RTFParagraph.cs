using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncLoopRTFLibrary
{
    public class RTFParagraph
    {

        #region --------------------------------------------------------------------------------< PROPERTIES >

        public string Content { get; set; }
        public RTFParagraphOptions Options { get; set; }
        public string ContentPlain { get; set; } = null;
        public string ContentBold { get; set; } = null;
        public bool IsLoop { get; set; } = false;

        #endregion

        #region --------------------------------------------------------------------------------< CONSTRUCTORS >

        public RTFParagraph()
        {

        }

        public RTFParagraph(string content, RTFParagraphOptions options)
        {
            Content = content;
            Options = options;
        }

        public RTFParagraph(string contentPlain, string contentBold, RTFParagraphOptions options)
        {
            ContentPlain = contentPlain;
            ContentBold = contentBold;
            Options = options;
        }

        #endregion

        #region --------------------------------------------------------------------------------< METHODS >

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
