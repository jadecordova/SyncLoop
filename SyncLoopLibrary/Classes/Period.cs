using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncLoopLibrary
{
    /// <summary>
    /// Identifies a period of invoicing.
    /// </summary>
    public class Period
    {
        #region PROPERTIES

        /// <summary>
        /// Period ID.
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// Period start date.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Period end date.
        /// </summary>
        public DateTime EndDate { get; set; }

        #endregion


        
        #region CONSTRUCTORS

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Period()
        {

        }

        /// <summary>
        /// Constructor for default period.
        /// Sets start and end date to current date.
        /// </summary>
        /// <param name="defaultPeriod">Default period flag.</param>
        public Period(bool defaultPeriod)
        {
            StartDate = DateTime.Now;

            EndDate = DateTime.Now;
        }

        #endregion
    }
}
