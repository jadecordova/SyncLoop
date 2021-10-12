using SyncLoopLibrary;
using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace SyncLoop
{

    public partial class TextEditor : Window
    {
        /// <summary>
        /// Inserts loop in document in Excel mode.
        /// </summary>
        /// <param name="timecode">Timecode string.</param>
        /// <param name="warning">Flag to indicate that this loop occurs before
        /// or at the same time than the previous, thus invalidating it.</param>
        /// <returns>Flag fo success</returns>
        public bool InsertLoop(string timecode, bool warning)
        {
            // Get character name.
            string characterName = GetCharacterName();

            // Check for character.
            if (!String.IsNullOrEmpty(characterName))
            {

                Run timecodeRun = new Run($"{timecode} {characterName.ToUpper()}");

                InsertParagraph(timecodeRun, warning);

                UpdateTaskBarLabel();

                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Inserts loop in document in RTF mode.
        /// </summary>
        /// <param name="cueIn">Cue in timecode.</param>
        /// <param name="cueOut">Cue out timecode.</param>
        /// <returns>Flag for success.</returns>
        public bool InsertLoop(string cueIn, string cueOut)
        {
            // Get character name.
            string characterName = GetCharacterName();

            // Check for character.
            if (!String.IsNullOrEmpty(characterName))
            {

                Run timecodeRun = new Run($"DUB[0 N {cueIn}>{cueOut}] {characterName.ToUpper()}");

                InsertParagraph(timecodeRun);

                UpdateTaskBarLabel();

                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Inserts loop in document for subtitles mode.
        /// </summary>
        /// <param name="cueIn">Cue in timecode.</param>
        /// <param name="cueOut">Cue out timecode</param>
        /// <param name="subtitle">Subtitle object.</param>
        /// <param name="focusTextEditor">Flag to indicate if focus should shift
        /// to text editor after inserting loop. Generally false, since subtitles
        /// are mainly entered in sequential mode, without interruption.</param>
        /// <param name="inFrame">Cue in frame.</param>
        /// <param name="outFrame">Cue out frame.</param>
        /// <returns>Flag for success.</returns>
        public bool InsertLoop(string cueIn, string cueOut, Subtitle subtitle, bool focusTextEditor, long inFrame = 0, long outFrame = 0)
        {
            Run timecodeRun = null;

            // Check if we are inserting a subtitle.
            if (subtitle != null)
            {
                // Header.
                string subtitleString = "SUB[";
            
                // Line.
                subtitleString += subtitle.Line.ToString();
                
                // Alignment.
                switch (subtitle.Alignment)
                {

                    case SubtitleAlignment.Center:

                        subtitleString += " ";

                        break;

                    case SubtitleAlignment.Left:

                        subtitleString += "L";

                        break;

                    case SubtitleAlignment.Right:

                        subtitleString += "R";

                        break;
                }

                // Italics.
                if (subtitle.FirstLineItalics && subtitle.SecondLineItalics)
                {
                    subtitleString += "I";
                }
                else if (subtitle.FirstLineItalics && !subtitle.SecondLineItalics)
                {
                    subtitleString += "1";
                }
                else if (!subtitle.FirstLineItalics && subtitle.SecondLineItalics)
                {
                    subtitleString += "2";
                }
                else
                {
                    subtitleString += "N";
                }
                
                // Close.
                subtitleString += $" {cueIn}>{cueOut}][{inFrame}:{outFrame}]";

                timecodeRun = new Run(subtitleString);
                
                InsertParagraph(timecodeRun, false, focusTextEditor);

                UpdateTaskBarLabel();

                return true;
            }

            return false;
        }


        /// <summary>
        /// Returns character name or null, if no charater was found.
        /// </summary>
        /// <returns>Character name or null.</returns>
        private string GetCharacterName()
        {
            string result = null;

            // Get current editor line for character look-up.
            string characterToLook = GetCurrentLineText();
            
            // Select character.
            Character selectedCharacter = SelectCharacter(characterToLook);
            
            if(selectedCharacter != null)
            {
                result = selectedCharacter.Name;
            }

            return result;
        }


        /// <summary>
        /// Inserts the actual paragraph in the document.
        /// </summary>
        /// <param name="run">The run object to be inserted.</param>
        /// <param name="warning">Flag to indicate invalid loop and change its color.</param>
        /// <param name="focusTextEditor">Flag to indicate if focus should shift
        /// to text editor after inserting loop</param>
        private void InsertParagraph(Run run, bool warning = false, bool focusTextEditor = true)
        {
            // Create final loop content.
            Paragraph loop = new Paragraph();

            // Set bold.
            loop.FontWeight = FontWeights.Bold;
            
            // Set color.
            if (warning == true) run.Foreground = Brushes.Red;
            
            // Add run to paragraph.
            loop.Inlines.Add(run);
            
            // Get current caret position.
            TextPointer insertionPosition = Editor.CaretPosition;
            
            // if it is not at an insertion point...
            if (!Editor.CaretPosition.IsAtInsertionPosition)
            {
                // Get next insertion point.
                insertionPosition = Editor.CaretPosition.GetInsertionPosition(LogicalDirection.Forward);
            }
            
            // Write it.
            // First we test for an empty document.
            // The insertion position should not be null.
            Editor.BeginChange();

            if (insertionPosition.Paragraph != null)
            {
                Editor.Document.Blocks.InsertBefore(insertionPosition.Paragraph, loop);
            }
            else
            {
                // If it is an empty document, we add the paragraph
                // instead of inserting it.
                Editor.Document.Blocks.Add(loop);
            }

            Editor.EndChange();

            // Set focus.
            if (focusTextEditor) Editor.Focus();

            // Auto save.
            if (LoopsSinceLastSave++ > Settings.ApplicationSettings.Autosave)
            {
                Save_Executed(null, null);
                // Reset counter.
                LoopsSinceLastSave = 0;
            }

        }
    }
}