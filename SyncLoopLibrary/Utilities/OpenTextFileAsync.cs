using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SyncLoopLibrary
{
    public static partial class Utilities
    {

        /// <summary>
        /// Open text file asynchronously.
        /// </summary>
        /// <param name="path">File path.</param>
        /// <returns>Content string.</returns>
        public static async Task<string> OpenTextFileAsync(string path)
        {
            string result = string.Empty;

            try
            {
                using (StreamReader sr = new StreamReader(path, Encoding.UTF8))
                {
                    result = await sr.ReadToEndAsync();
                }

            }
            catch (Exception e)
            {
                // Log($"Error opening text file asynchronously: {e.Message}");

                return null;
            }

            return result;
        }
    }
}
