using SyncLoopLibrary;
using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Documents;

namespace SyncLoop
{
    public partial class TextEditor : Window
    {

        /// <summary>
        /// This method processes text and RTF files to convert them into a flow document properly formatted, for
        /// display and edition in the text editor. The result is a homogeneous content on the text editor.
        /// </summary>
        private async void OpenTextFile(string path)
        {
            // Variable to store the file extension.
            string extension;

            // Variable to store the file content.
            string fileContent = String.Empty;

            // If no parameter wass passed, we open a file dialog.
            if (!String.IsNullOrEmpty(path))
            {
                // Get current directory.
                OriginalTextFileFolder = Path.GetDirectoryName(path);
            
                // Get file extension.
                extension = Path.GetExtension(path);

                // Select type of document.
                switch (extension)
                {
                    case ".txt":
                        
                        // If it is a plain text file, just read the content and place in the fileContent variable.
                        fileContent = await Utilities.OpenTextFileAsync(path);

                        break;

                    case ".xaml":
                
                        // If it is a XAML file, read it and assign it to the editor.
                        try
                        {
                            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
                            {
                                TextRange range = new TextRange(Editor.Document.ContentStart, Editor.Document.ContentEnd);

                                if (fs.Length > 0)
                                {
                                    range.Load(fs, System.Windows.DataFormats.Xaml);
                                
                                    // Set flag.
                                    DOCUMENT_LOADED = true;
                                    
                                    // Set file name.
                                    TextFileName = path;

                                    // Set task bar info.
                                    UpdateTaskBarLabel();
                                    
                                    // Set the file in the project file object.
                                    Settings.ApplicationSettings.Project.XamlFile = path;
                                    
                                    // Set windows title bar.
                                    Title += path;

                                    return;
                                }
                                else
                                {
                                    MessageBox.Show("The file is empty.",
                                                    "SyncLoop", 
                                                    MessageBoxButton.OK, MessageBoxImage.Warning);

                                    return;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"There was an error reading the file: {ex.Message}",
                                            "SyncLoop",
                                            MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }

                    case ".rtf":

                        // If it is a RTF file, read it, convert it to plain text which is stupidly complex, by the way) 
                        // and assign it to fileContent variable.
                        fileContent = await Utilities.OpenRTFFileAsync(path);

                        break;
                }

                if (fileContent != null)
                {
                    // Set the file in the project file object.
                    Settings.ApplicationSettings.Project.TextFile = path;

                    // Set editor title bar.
                    Title += path;
                }
                else
                {
                    MessageBox.Show("Cannot open document.", 
                                    "SyncLoop",
                                    MessageBoxButton.OK, MessageBoxImage.Exclamation);

                    return;
                }

                // Create flow document.
                FlowDocument flowDocument = Utilities.CreateFlowDocumentFromText(fileContent);

                // If the method returned a valid document...
                if (flowDocument != null)
                {
                    // Clear rich text box.
                    Editor.Document.Blocks.Clear();

                    // Set document.
                    Editor.Document = flowDocument; 
                    
                    // Set flag.
                    DOCUMENT_LOADED = true;
                    
                    // Set file name.
                    TextFileName = path;
                    
                    // Set task bar info.
                    UpdateTaskBarLabel();
                }
                else
                {
                    // Log("Invalid document");

                    MessageBox.Show("Could not create document.",
                                    "SyncLoop",
                                    MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }
    }
}