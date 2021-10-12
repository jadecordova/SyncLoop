using System.Threading.Tasks;

namespace SyncLoopLibrary
{
    public static partial class Utilities
    {
        /// <summary>
        /// Opens RTF file asynchronously.
        /// </summary>
        /// <param name="path">File path.</param>
        /// <returns>Content string.</returns>
        public static async Task<string> OpenRTFFileAsync(string path)
        {

            string result = await Task.Run(() => OpenRTFFile(path));

            return result;
        }
    }
}
