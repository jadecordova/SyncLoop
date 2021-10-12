using System.Text;

namespace SyncLoopLibrary
{
    /// <summary>
    /// Merge attribute of cells
    /// </summary>
    public class CellMerge
    {

        #region PROPERTIES

        /// <summary>
        /// Number of cells to merge across to the right.
        /// </summary>
        public string MergeAcross { get; set; }

        /// <summary>
        /// Number of cells to merge down.
        /// </summary>
        public string MergeDown { get; set; }

        #endregion



        #region CONSTRUCTORS

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="across">Number of columns to merge.</param>
        /// <param name="down">Number of rows to merge.</param>
        public CellMerge(int across = 0, int down = 0)
        {
            MergeAcross = across.ToString();
            MergeDown = down.ToString();
        }

        #endregion



        #region METHODS

        /// <summary>
        /// Creates merge object.
        /// </summary>
        /// <returns>Merge object string.</returns>
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
