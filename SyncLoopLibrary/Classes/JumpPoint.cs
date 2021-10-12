using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncLoopLibrary
{
    /// <summary>
    /// Jump point for subtitles playback.
    /// </summary>
    public class JumpPoint
    {
        #region PROPERTIES

        /// <summary>
        /// Frame from wich to jump.
        /// </summary>
        public long Frame { get; set; }

        /// <summary>
        /// TimeSpan to go to.
        /// </summary>
        public TimeSpan Span { get; set; }

        #endregion



        #region OVERRIDES

        /// <summary>
        /// Formated info.
        /// </summary>
        /// <returns>Frame and TimeSpan.</returns>
        public override string ToString()
        {
            return $"Frame: {Frame}; TimeSpan: {Span.ToString()}";
        }

        #endregion
    }
}
