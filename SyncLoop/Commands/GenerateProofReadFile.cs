using SyncLoopLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace SyncLoop
{
    public partial class TextEditor : Window
    {
        private void GenerateProofReadFile_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = DOCUMENT_LOADED;
        }

        private async void GenerateProofReadFile_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // Check if there is a document in the editor.
            if (DOCUMENT_LOADED)
            {
                // Get text from editor.
                string text = new TextRange(Editor.Document.ContentStart, Editor.Document.ContentEnd).Text;
                // Replace RTF control characters.
                text = text.Replace("{", @"\'7B");
                text = text.Replace("}", @"\'7D");
                // Create a fresh flow document to avoid errors.
                FlowDocument document = Utilities.CreateFlowDocumentFromText(text);

                // Save flag.
                bool SAVE_SUCCESS;
                
                // Dialog message.
                string message = null;


                if(document != null)
                {
                    Loops loops = new Loops();

                    List<string> errors = loops.ExtractLoops(document,
                                                             Characters,
                                                             RTFUtilities.EmptyLine,
                                                             Settings.ApplicationSettings.CharactersPerLine);
                    if (errors != null)
                    {
                        ShowErrors(errors);
                    }
                    else
                    {
                        // Check sequentiality.
                        List<string> result = await Task.Run(() => loops.CheckSequentiality());

                        if (result != null)
                        {
                            ShowErrors(result);
                        }
                        else
                        {
                            // Variable to hold de resulting document string.
                            string documentContent = string.Empty;

                            // Generate content.
                            documentContent = await Task.Run(() => RTFUtilities.GenerateRTFDocument(loops.ProgramLoops, 
                                                                                                    Characters, 
                                                                                                    Settings.ApplicationSettings, 
                                                                                                    programInfo));

                            // Generate proof read file.
                            if (!String.IsNullOrEmpty(documentContent) && !String.IsNullOrWhiteSpace(documentContent))
                            {
                                // Set name.
                                string fileName = Path.Combine(Settings.ApplicationSettings.Project.ProjectFolder, Settings.ApplicationSettings.Project.DocumentName + " - PROOF.rtf");
                                
                                // Save file.
                                SAVE_SUCCESS = await Utilities.SaveDocumentAsync(documentContent, fileName);

                                if (SAVE_SUCCESS)
                                {
                                    // Save info on project object.
                                    Settings.ApplicationSettings.Project.ProofReadFile = fileName;

                                    message = "Proof read document saved.";
                                }
                                else
                                {
                                    message = "Error saving proof read document.";
                                }
                            }
                            else
                            {
                                message = "Proof read document has no content. No file was saved.";
                            }

                            MessageBox.Show(message, "SyncLoop");
                        }
                    }

                    // Reset characters lines.
                    // This is a bug fix where the characters kept the previous number of lines
                    // when generating the Excel document multiple times,
                    // and the number of lines was adding up.
                    foreach (Character character in Characters)
                    {
                        character.Lines = 0;
                    }
                }
            }
        }
    }
}
