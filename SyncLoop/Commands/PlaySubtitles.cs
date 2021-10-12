using Newtonsoft.Json;
using SyncLoopLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace SyncLoop
{
    public partial class TextEditor : Window
    {
        private void PlaySubtitles_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void PlaySubtitles_Executed(object sender, ExecutedRoutedEventArgs e)
        {

            // Player window.
            SubtitlesPlayer SubtitlesPlayer;

            // Video file path.
            string video = Settings.ApplicationSettings.Project.VideoFile;

            switch (Settings.ApplicationSettings.SubtitlesMode)
            {

                #region INTERNAL

                case SubtitlesMode.Internal:

                    // Get text from editor.
                    string text = new TextRange(Editor.Document.ContentStart, Editor.Document.ContentEnd).Text;

                    // Create a fresh flow document to avoid errors.
                    FlowDocument document = Utilities.CreateFlowDocumentFromText(text);
                    
                    if (document != null)
                    {
                        // Generate loop list.
                        Loops loops = new Loops();

                        // Extract loops.
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
                            List<string> result = loops.CheckSequentiality();

                            if (result != null)
                            {
                                ShowErrors(result);
                            }
                            else
                            {
                                // Get subtitle loops.
                                List<Loop> subtitles = GetSubtitleLoops(loops);

                                SubtitlesPlayer = new SubtitlesPlayer(video, subtitles);

                                SubtitlesPlayer.Show();
                            }
                        }
                    }

                    break;

                #endregion



                #region FFME

                case SubtitlesMode.FFME:

                    // File name.
                    string jumpsFilePath = Path.ChangeExtension(video, ".jump");

                    // Jump object.
                    Queue<JumpPoint> jumps = null;
                    
                    // string to load file into.
                    string json = null;

                    // Deserialize the jumps.
                    try
                    {
                        using (StreamReader reader = new StreamReader(jumpsFilePath))
                        {
                            json = reader.ReadToEnd();
                        }
                        if (!String.IsNullOrEmpty(json))
                        {
                            try
                            {
                                jumps = JsonConvert.DeserializeObject<Queue<JumpPoint>>(json);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"There was an error reading the jump file contents: {ex.Message}", "SyncLoop");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Jump file is invalid.", "SyncLoop");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"There was an error reading the jump file: {ex.Message}", "SyncLoop");
                    }

                    SubtitlesPlayer = new SubtitlesPlayer(video, jumps);

                    SubtitlesPlayer.Show();

                    break;

                    #endregion

            }
        }

        
        /// <summary>
        /// Selects subtitle loops in document
        /// </summary>
        /// <param name="loops">Loops object with all document loops.</param>
        /// <returns>Listo of subtitle loop objects.</returns>
        private List<Loop> GetSubtitleLoops(Loops loops)
        {
            // Result.
            List<Loop> result = new List<Loop>();

            foreach (Loop loop in loops.ProgramLoops)
            {
                if (loop.Mode == DocumentMode.Subtitles)
                {
                    result.Add(loop);
                }
            }

            result.TrimExcess();

            return result;
        }
    }
}
