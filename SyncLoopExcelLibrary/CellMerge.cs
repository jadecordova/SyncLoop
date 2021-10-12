using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncLoopExcelLibrary
{
    /// <summary>
    /// Merge attribute of cells
    /// </summary>
    public class CellMerge
    {

        #region ------------------------------------------------------------PROPERTIES

        /// <summary>
        /// Number of cells to merge across to the right.
        /// </summary>
        public string MergeAcross { get; set; }

        /// <summary>
        /// Number of cells to merge down.
        /// </summary>
        public string MergeDown { get; set; }

        #endregion

        #region ------------------------------------------------------------CONSTRUCTORS

        public CellMerge(int across = 0, int down = 0)
        {
            MergeAcross = across.ToString();
            MergeDown = down.ToString();
        }

        #endregion

        #region ------------------------------------------------------------METHODS

        public string WriteMerge()
        {
            // Result constructor.
            StringBuilder merge = new StringBuilder();
            // Write.
            if (MergeAcross != "0")
            {
                merge.Append(@"ss:MergeAcross=" + ExcelUtilities.Quote + MergeAcross + ExcelUtilities.Quote + " ");
            }
            if (MergeDown != "0")
            {
                merge.Append(@"ss:MergeDown=" + ExcelUtilities.Quote + MergeDown + ExcelUtilities.Quote + " ");
            }

            return merge.ToString();
        }

        #endregion
    }
}
