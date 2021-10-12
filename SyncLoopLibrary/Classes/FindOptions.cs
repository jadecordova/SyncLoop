using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncLoopLibrary
{
    /// <summary>
    /// This class represents the possible options for search operation.
    /// </summary>
    [Flags]
    public enum FindOptions
    {
        /// <summary>
        /// Perform case-insensitive non-word search.
        /// </summary>
        None = 0x00000000,

        /// <summary>
        /// Perform case-sensitive search.
        /// </summary>
        MatchCase = 0x00000001,

        /// <summary>
        /// Perform the search against whole word.
        /// </summary>
        MatchWholeWord = 0x00000002,
    }
}
