using System.Text;

namespace SyncLoopLibrary
{
    /// <summary>
    /// Page setup
    /// </summary>
    public class PageSetup
    {

        #region PROPERTIES

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



        #region CONSTRUCTORS

        /// <summary>
        /// Default constructor.
        /// </summary>
        public PageSetup()
        {
            PageLayout = null;
            PageHeader = null;
            PageFooter = null;
            Margins = null;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="pageLayout">Page layout object.</param>
        public PageSetup(Layout pageLayout)
        {
            PageLayout = pageLayout;
            PageHeader = null;
            PageFooter = null;
            Margins = null;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="pageLayout">Page layout object.</param>
        /// <param name="pageHeader">Page header object.</param>
        public PageSetup(Layout pageLayout, Header pageHeader)
        {
            PageLayout = pageLayout;
            PageHeader = pageHeader;
            PageFooter = null;
            Margins = null;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="pageLayout">Page layout object.</param>
        /// <param name="pageHeader">Page header object.</param>
        /// <param name="pageFooter">Page footer object.</param>
        public PageSetup(Layout pageLayout, Header pageHeader, Footer pageFooter)
        {
            PageLayout = pageLayout;
            PageHeader = pageHeader;
            PageFooter = pageFooter;
            Margins = null;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="pageLayout">Page layout object.</param>
        /// <param name="pageHeader">Page header object.</param>
        /// <param name="pageFooter">Page footer object.</param>
        /// <param name="margins">Margins object.</param>
        public PageSetup(Layout pageLayout, Header pageHeader, Footer pageFooter, PageMargins margins)
        {
            PageLayout = pageLayout;
            PageHeader = pageHeader;
            PageFooter = pageFooter;
            Margins = margins;
        }

        #endregion



        #region METHODS

        /// <summary>
        /// Creates page setup.
        /// </summary>
        /// <returns>Page setup object.</returns>
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
