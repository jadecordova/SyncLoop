using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncLoopLibrary
{
    /// <summary>
    /// SRT subtitle format info.
    /// </summary>
    public class SRT
    {
        #region PROPERTIES

        /// <summary>
        /// String of SRT format subtitles.
        /// </summary>
        public string Subtitles { get; set; }

        /// <summary>
        /// Queue of JumpPoint objects for video control.
        /// </summary>
        public Queue<JumpPoint> JumpList { get; set; }

        #endregion
    }
}
