using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncLoopExcelLibrary
{
    /// <summary>
    /// Page setup
    /// </summary>
    public class PageSetup
    {

        #region ----------------------------------------------------------------------PROPERTIES

        /// <summary>
        /// Layout object.
        /// </summary>
        public Layout PageLayout { get; set; }

        /// <summary>
        /// Page header.
        /// </summary>
        public Header PageHeader { get; set; }

        /// <summary>
        /// Page footer.
        /// </summary>
        public Footer PageFooter { get; set; }

        /// <summary>
        /// Page margins
        /// </summary>
        public PageMargins Margins { get; set; }

        #endregion

        #region ----------------------------------------------------------------------CONSTRUCTORS

        public PageSetup()
        {
            PageLayout = null;
            PageHeader = null;
            PageFooter = null;
            Margins = null;
        }

        public PageSetup(Layout pageLayout)
        {
            PageLayout = pageLayout;
            PageHeader = null;
            PageFooter = null;
            Margins = null;
        }

        public PageSetup(Layout pageLayout, Header pageHeader)
        {
            PageLayout = pageLayout;
            PageHeader = pageHeader;
            PageFooter = null;
            Margins = null;
        }

        public PageSetup(Layout pageLayout, Header pageHeader, Footer pageFooter)
        {
            PageLayout = pageLayout;
            PageHeader = pageHeader;
            PageFooter = pageFooter;
            Margins = null;
        }

        public PageSetup(Layout pageLayout, Header pageHeader, Footer pageFooter, PageMargins margins)
        {
            PageLayout = pageLayout;
            PageHeader = pageHeader;
            PageFooter = pageFooter;
            Margins = margins;
        }

        #endregion

        #region ----------------------------------------------------------------------METHODS

        public string WritePageSetup()
        {
            // Result constructor.
            StringBuilder pagesetup = new StringBuilder();
            // Header.
            pagesetup.AppendLine(ExcelUtilities.Indent4 + @"<PageSetup>");
            // Attributes.
            if (PageLayout != null) pagesetup.Append(PageLayout.WriteLayout());
            if (PageHeader != null) pagesetup.Append(PageHeader.WriteHeader());
            if (PageFooter != null) pagesetup.Append(PageFooter.WriteFooter());
            if (Margins != null) pagesetup.Append(Margins.WriteMargins());
            // Footer.
            pagesetup.AppendLine(ExcelUtilities.Indent4 + @"</PageSetup>");

            return pagesetup.ToString();
        }

        #endregion
    }
}
