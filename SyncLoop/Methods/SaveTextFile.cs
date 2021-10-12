using System;
using System.IO;
using System.Windows;
using System.Windows.Documents;

namespace SyncLoop
{
    public partial class TextEditor : Window
    {
        /// <summary>
        /// Saves text in editor.
        /// </summary>
        /// <param name="filePath">File name.</param>
        private void SaveTextFile(string filePath)
        {
            // Get file extension.
            string extension = Path.GetExtension(filePath);

            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                TextRange range = new TextRange(Editor.Document.ContentStart, Editor.Document.ContentEnd);
            
                try
                {
                    switch (extension)
                    {
                        case ".txt":

                            range.Save(fs, DataFormats.Text);

                            break;

                        case ".xaml":

                            range.Save(fs, DataFormats.Xaml);

                            break;

                        case ".rtf":

                            range.Save(fs, DataFormats.Rtf);

                            break;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"There was an error saving the file: {ex.Message}",
                                    "SyncLoop",
                                    MessageBoxButton.OK, 
                                    MessageBoxImage.Warning);
                    return;
                }
            }
        }
    }
}
