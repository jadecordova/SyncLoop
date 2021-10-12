using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace SyncLoopLibrary
{
    public static partial class Utilities
    {
        /// <summary>
        /// Saves string to file asynchronously.
        /// </summary>
        /// <param name="text">Text to save.</param>
        /// <param name="fileName">Fully qualified path.</param>
        public static async Task<bool> SaveDocumentAsync(string text, string fileName)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(fileName))
                {
                    await writer.WriteAsync(text);

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
