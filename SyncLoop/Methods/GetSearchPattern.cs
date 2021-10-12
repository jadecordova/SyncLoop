using SyncLoopLibrary;
using System;
using System.Windows;

namespace SyncLoop
{
    public partial class TextEditor : Window
    {

        /// <summary>
        /// Creates regex search pattern based on document type.
        /// </summary>
        /// <returns>RegEx formatted string.</returns>
        [Obsolete]
        private string GetSearchPattern()
        {
            // Create result.
            string result = String.Empty;

            switch (Settings.ApplicationSettings.DocumentType)
            {
                case DocumentMode.Excel:
                case DocumentMode.Subtitles:

                    result = @"\d\d:\d\d:\d\d:\d\d";
                    break;

                case DocumentMode.RTF:

                    result = @"DUB\[0 N \d\d:\d\d:\d\d:\d\d>\d\d:\d\d:\d\d:\d\d\]";

                    break;
            }

            return result;
        }
    }
}
