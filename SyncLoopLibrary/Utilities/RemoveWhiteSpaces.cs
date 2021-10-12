using System.Text.RegularExpressions;

namespace SyncLoopLibrary
{
    public static partial class Utilities
    {
        /// <summary>
        /// Removes extra white space from document.
        /// </summary>
        /// <param name="text">Text to modify.</param>
        /// <returns>String with no double spaces.</returns>
        public static string RemoveWhiteSpaces(string text)
        {
            // Create regex.
            Regex regex = new Regex("[ ]{2,}", RegexOptions.None);
            // Replace.
            return regex.Replace(text, " ");
        }

    }
}
