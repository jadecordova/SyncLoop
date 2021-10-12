using LiveCharts;
using System;

namespace SyncLoopLibrary
{
    /// <summary>
    /// Defines data for chart graphic.
    /// </summary>
    public class ChartData
    {
        /// <summary>
        /// Actual values for the the chart series.
        /// </summary>
        public SeriesCollection Data { get; set; } = new SeriesCollection();

        /// <summary>
        /// Labels for the X axis.
        /// </summary>
        public string[] Labels { get; set; }

        /// <summary>
        /// Formatting function.
        /// </summary>
        public Func<double, string> Formatter { get; set; } = value => value.ToString("N");
    }
}
