using System.Collections.Generic;

namespace SyncLoopLibrary
{
    /// <summary>
    /// Report information.
    /// </summary>
    public class Report
    {
        #region PROPERTIES

        /// <summary>
        /// Translator name.
        /// </summary>
        public string Translator { get; set; } = "Glyphos, Servicios de Comunicación, C. A.";

        /// <summary>
        /// Report month.
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// Report year.
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Report IVA rate.
        /// </summary>
        public decimal IVA { get; set; }

        /// <summary>
        /// List of channel reports.
        /// </summary>
        public List<ChannelReport> Reports { get; set; }

        #endregion
    }
}
