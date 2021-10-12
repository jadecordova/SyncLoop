using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncLoopLibrary
{
    /// <summary>
    /// Global variables.
    /// </summary>
    public static class Globals
    {
        /// <summary>
        /// Regular expression pattern for frames block.
        /// </summary>
        public static string FRAMES_PATTERN { get; set; } = @"\[\d+:\d+\]";

        /// <summary>
        /// Regular expression pattern for SMPTE code.
        /// </summary>
        public static string GENERAL_SMPTE_PATTERN { get; } =   @"\d\d:\d\d:\d\d:\d\d";

        /// <summary>
        /// Regular expression pattern for RTF loop.
        /// </summary>
        public static string RTF_SMPTE_PATTERN { get; } =       @"DUB\[0 N \d\d:\d\d:\d\d:\d\d>\d\d:\d\d:\d\d:\d\d\]";

        /// <summary>
        /// Regular expression pattern for subtitle loop.
        /// </summary>
        public static string SUBTITLE_PATTERN { get; } =  @"SUB\[... \d\d:\d\d:\d\d:\d\d>\d\d:\d\d:\d\d:\d\d\]";

        /// <summary>
        /// Regular expression pattern for subtitle plus frames block.
        /// </summary>
        public static string SUBTITLE_SMPTE_PATTERN { get; } =  SUBTITLE_PATTERN + FRAMES_PATTERN;
    }
}
