using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace SyncLoopLibrary
{
    public static partial class Utilities
    {

        /// <summary>
        /// Returns background brush to be applied to paragraph
        /// based on the number of characters.
        /// </summary>
        /// <param name="paragraph">String to check.</param>
        /// <returns>Brush to be applied to current paragraph.</returns>
        public static SolidColorBrush CheckParagraphLength(Paragraph paragraph)
        {
            // Get lenght of paragraph.
            int contentLength = new TextRange(paragraph.ContentStart, paragraph.ContentEnd).Text.Length;
            // Set background color.
            if (contentLength > Settings.ApplicationSettings.LoopLength)
            {
                return new SolidColorBrush(Color.FromArgb(255, 135, 206, 250));
            }

            return new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
        }
    }
}
