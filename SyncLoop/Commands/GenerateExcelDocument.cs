using SyncLoopLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace SyncLoop
{
    public partial class TextEditor : Window
    {

        private void GenerateExcelDocument_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = DOCUMENT_LOADED && (Settings.ApplicationSettings.DocumentType == DocumentMode.Excel);
        }


        private async void GenerateExcelDocument_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (DOCUMENT_LOADED)
            {
                // Get text from editor.
                string text = new TextRange(Editor.Document.ContentStart, Editor.Document.ContentEnd).Text;

                // Create a fresh flow document to avoid errors.
                FlowDocument document = Utilities.CreateFlowDocumentFromText(text);

                // Save error flag.
                bool SAVE_SUCCESS;

                // Dialog message.
                string message = null;

                if (document != null)
                {
                    Loops loops = new Loops();

                    // Get loops or errors.
                    List<string> errors = loops.ExtractLoops(document,
                                                             Characters, 
                                                             ExcelUtilities.NewLine, 
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
                            // Create document string.
                            string documentContent = await Task.Run(() => ExcelUtilities.CreateExcelDocument(Characters.ToList(), 
                                                                                                             programInfo, 
                                                                                                             loops.ProgramLoops));

                            if (!String.IsNullOrEmpty(documentContent) && !String.IsNullOrWhiteSpace(documentContent))
                            {
                                // Create file name.
                                string fileName = Path.Combine(Settings.ApplicationSettings.Project.ProjectFolder, 
                                                               Settings.ApplicationSettings.Project.DocumentName);
                                // Save it.
                                SAVE_SUCCESS = await Utilities.SaveDocumentAsync(documentContent, fileName + ".xls");

                                if (SAVE_SUCCESS)
                                {
                                    // Set it in the project file object.
                                    Settings.ApplicationSettings.Project.ExcelFile = fileName + ".xls";

                                    // Add the program to the DB.
                                    AddProgramToDB(loops.FirstTimecode, loops.LastTimecode);

                                    message = "Excel document saved.";
                                }
                                else
                                {
                                    message = "Error saving Excel document.";
                                }
                            }
                            else
                            {
                                message = "Excel document has no content. No file was saved.";
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
