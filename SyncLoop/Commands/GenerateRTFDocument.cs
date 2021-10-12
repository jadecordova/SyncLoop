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
        private void GenerateRTFDocument_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = DOCUMENT_LOADED && (Settings.ApplicationSettings.DocumentType == DocumentMode.RTF);
        }


        private async void GenerateRTFDocument_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (DOCUMENT_LOADED)
            {
                // Get text from editor.
                string text = new TextRange(Editor.Document.ContentStart, Editor.Document.ContentEnd).Text;
                // Replace RTF control characters.
                text = text.Replace("{", @"\'7B");
                text = text.Replace("}", @"\'7D");

                // Create a fresh flow document to avoid errors.
                FlowDocument document = Utilities.CreateFlowDocumentFromText(text);
                
                // Flag for saving.
                bool SAVE_SUCCESS;
                
                // Dialog message.
                string message = null;


                if (document != null)
                {
                    // Generate loop list.
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
                            string fileName = Path.Combine(Settings.ApplicationSettings.Project.ProjectFolder,
                                                           Settings.ApplicationSettings.Project.DocumentName + ".rtf");
                            // Genereate document.
                            string RTF = await Task.Run(() => RTFUtilities.GenerateRTFDocument(loops.ProgramLoops, 
                                                                                               Characters, 
                                                                                               Settings.ApplicationSettings, 
                                                                                               programInfo, true));


                            if (!String.IsNullOrEmpty(RTF) && !String.IsNullOrWhiteSpace(RTF))
                            {
                                // Save document.
                                SAVE_SUCCESS = await Utilities.SaveDocumentAsync(RTF, fileName);

                                if (SAVE_SUCCESS)
                                {
                                    // Insert program into DB.
                                    AddProgramToDB(loops.FirstTimecode, loops.LastTimecode);

                                    message = "RTF document saved.";
                                }
                                else
                                {
                                    message = "Error saving RTF document.";
                                }
                            }
                            else
                            {
                                message = "RTF document has no content. No file was saved.";
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
