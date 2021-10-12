using Newtonsoft.Json;
using SyncLoopLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private void GenerateSubtitlesDocuments_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = DOCUMENT_LOADED && (Settings.ApplicationSettings.DocumentType == DocumentMode.Subtitles);
        }

        private async void GenerateSubtitlesDocuments_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (DOCUMENT_LOADED)
            {
                // Get text from editor.
                string text = new TextRange(Editor.Document.ContentStart, Editor.Document.ContentEnd).Text;

                // Create a fresh flow document to avoid errors.
                FlowDocument document = await Task.Run(() => Utilities.CreateFlowDocumentFromText(text));

                // Error message.
                string saveError = null;


                if (document != null)
                {

                    // Get loops list for all documents.
                    Loops loops = new Loops();

                    // Extract loops and look for errors.
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
                            // Create general file name.
                            string fileName = Path.Combine(Settings.ApplicationSettings.Project.ProjectFolder,
                                                           Settings.ApplicationSettings.Project.DocumentName);


                            bool saveResult;



                            #region EXCEL DOCUMENT

                            string excelDocument = await Task.Run(() => ExcelUtilities.CreateExcelDocument(Characters.ToList(),
                                                                                                           programInfo,
                                                                                                           loops.ProgramLoops));

                            if (!String.IsNullOrEmpty(excelDocument) && !String.IsNullOrWhiteSpace(excelDocument))
                            {
                                saveResult = await Utilities.SaveDocumentAsync(excelDocument, fileName + ".xls");

                                if (!saveResult)
                                {
                                    saveError = "Error saving Excel document." + Environment.NewLine;
                                }
                            }
                            else
                            {
                                saveError += "Excel document has no content. No file was saved." + Environment.NewLine;
                            }

                            #endregion



                            #region SYNCLOOP SUBTITLES

                            string RTF = await Task.Run(() => RTFUtilities.GenerateSyncLoopSubtitles(loops.ProgramLoops,
                                                                                                     Settings.ApplicationSettings,
                                                                                                     programInfo));


                            if (!String.IsNullOrEmpty(RTF) && !String.IsNullOrWhiteSpace(RTF))
                            {
                                saveResult = await Utilities.SaveDocumentAsync(RTF, Path.Combine(fileName + " - SUBTITLES.rtf"));

                                if (!saveResult)
                                {
                                    saveError += "Error saving SyncLoop document." + Environment.NewLine;
                                }
                            }
                            else
                            {
                                saveError += "SyncLoop format document has no content. No file was saved." + Environment.NewLine;
                            }

                            #endregion



                            #region SUB FORMAT SUBTITLES

                            byte[] subSubtitles = await Task.Run(() => Utilities.ExportSubtitlesSUB(loops.ProgramLoops));


                            if (subSubtitles != null && subSubtitles.Length > 0)
                            {
                                try
                                {
                                    File.WriteAllBytes(Path.Combine(fileName + ".sub"), subSubtitles);
                                }
                                catch (Exception ex)
                                {
                                    saveError += $"Error writing the sub file: {ex.Message}" + Environment.NewLine;
                                }
                            }
                            else
                            {
                                saveError += "Dub format document has no content. No file was saved." + Environment.NewLine;
                            }

                            #endregion



                            #region SRT SUBTITLES

                            SRT srtSubtitles = await Task.Run(() => Utilities.ExportSubtitlesSRT(loops.ProgramLoops));


                            string srtSubtitlesFileName = Path.ChangeExtension(Settings.ApplicationSettings.Project.VideoFile, "srt");

                            if (srtSubtitles.Subtitles != null && srtSubtitles.Subtitles.Length > 0)
                            {
                                if (!String.IsNullOrWhiteSpace(srtSubtitlesFileName))
                                {
                                    try
                                    {
                                        // Save it.
                                        saveResult = await Utilities.SaveDocumentAsync(srtSubtitles.Subtitles, srtSubtitlesFileName);

                                        if (!saveResult)
                                        {
                                            saveError += "Error saving SRT document." + Environment.NewLine;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        saveError += $"Error writing the SRT file: {ex.Message}" + Environment.NewLine;
                                    }
                                }
                                else
                                {
                                    saveError += "No video file specified." + Environment.NewLine;
                                }
                            }
                            else
                            {
                                saveError += "Dub format document has no content. No file was saved." + Environment.NewLine;
                            }

                            #endregion



                            #region JUMP LIST

                            // First, let's check a project or document was actually opened.
                            if (Settings.ApplicationSettings.Project.VideoFile != null)
                            {
                                if (srtSubtitles.JumpList != null)
                                {
                                    // Then, create the project file name and path.
                                    string path = Path.ChangeExtension(Settings.ApplicationSettings.Project.VideoFile, ".jump");

                                    if (!String.IsNullOrEmpty(path))
                                    {
                                        try
                                        {
                                            using (StreamWriter writer = new StreamWriter(path))
                                            {
                                                writer.Write(JsonConvert.SerializeObject(srtSubtitles.JumpList, Formatting.Indented));
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            saveError += $"There was an error writing the jump file: {ex.Message}";
                                        }
                                    }
                                }
                                else
                                {
                                    saveError += "Jump list is invalid.";
                                }
                            }
                            else
                            {
                                saveError += "No document has been opened.";
                            }

                            #endregion


                            if (saveError != null)
                            {
                                MessageBox.Show(saveError, "SyncLoop");
                            }
                            else
                            {
                                // Insert it into database.
                                AddProgramToDB(loops.FirstTimecode, loops.LastTimecode);

                                MessageBox.Show("Documents saved.", "SyncLoop");
                            }
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
