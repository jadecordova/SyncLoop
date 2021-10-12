using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Documents;

namespace SyncLoopLibrary
{
    public class LoopsOLD
    {
        /*
        #region MEMBERS

        // All program loops.
        public List<Loop> ProgramLoops { get; set; } = new List<Loop>();

        // First time code of document, for calculating duration.
        public string FirstTimecode { get; set; } = null;

        // Last time code of document, for calculating duration.
        public string LastTimecode { get; set; } = null;

        #endregion



        #region METHODS

        /// <summary>
        /// Creates a list of loops for generating the final document in any mode.
        /// </summary>
        /// <returns>List of loops.</returns>
        public List<string> ExtractLoops(FlowDocument document,
                                         ObservableCollection<Character> characters,
                                         string newLine,
                                         int charactersPerLine)
        {

            // Holds possible timecode errors.
            List<string> errors = new List<string>();
            // General match for SMPTE string.
            Match generalMatch = null;
            // Flag for multiple paragraphs per characters, as in the case of titles.
            bool IS_CONTENT_SET = false;
            // This flag signals if the actual document started.
            // It is set after the first loop is found.
            // This prevents error when creating the final document
            // because before this, characters are null.
            bool DOCUMENT_STARTED = false;
            // Working character.
            Character character;

            // Iterate.
            foreach (Block block in document.Blocks)
            {
                // Convert block to paragraph.
                Paragraph paragraph = block as Paragraph;
                // If it indeed was a paragraph...
                if (paragraph != null)
                {
                    // Get paragraph content.
                    string text = new TextRange(paragraph.ContentStart, paragraph.ContentEnd).Text;
                    // If it is not empty...
                    if (!String.IsNullOrEmpty(text) && !String.IsNullOrWhiteSpace(text))
                    {
                        // Search for general loop.
                        // All 3 modes (Excel, RTF and subtitles) will match this.
                        generalMatch = Regex.Match(text, Globals.GENERAL_SMPTE_PATTERN);
                        // If a match is found...
                        if (generalMatch.Success)
                        {
                            // Since there was a match, we know the document has started.
                            DOCUMENT_STARTED = true;
                            // We are in a loop definition paragraph.
                            // Let's find out what kind of loop it is.
                            // First, we test for subtitle loop by searching for the SUB[ string
                            // at the beginning of the paragraph.
                            if (text.StartsWith("SUB["))
                            {
                                // Now we extract the in and out timecodes.
                                // First, we find the matches.
                                MatchCollection timecodes = FindTimecodes(text);

                                if (timecodes != null)
                                {
                                    // Get complete timecode string.
                                    Match timecode = Regex.Match(text, Globals.SUBTITLE_SMPTE_PATTERN);
                                    // Check for success.
                                    if (timecode.Success)
                                    {
                                        // Create working subtitle loop.
                                        ProgramLoops.Add(new Loop()
                                        {
                                            Timecode = timecode.Value,
                                            InTimecode = timecodes[0].Value,
                                            OutTimecode = timecodes[1].Value,
                                            Mode = DocumentMode.Subtitles
                                        });
                                    }
                                    else
                                    {
                                        errors.Add(text);
                                    }
                                }
                                else
                                {
                                    errors.Add(text);
                                }
                            }
                            else if (text.StartsWith("DUB["))
                            {
                                MatchCollection timecodes = FindTimecodes(text);
                                // If timecodes were found...
                                if (timecodes != null)
                                {
                                    // Get the match for the whole timecode string.
                                    Match rtfMatch = Regex.Match(text, Globals.RTF_SMPTE_PATTERN);
                                    // Check for success.
                                    if (rtfMatch.Success)
                                    {
                                        // Since we are in a new loop, no content has been set yet,
                                        // so we reset the content flag.
                                        // This flag is used to allow for multiple-paragraph loop content,
                                        // as in the case of titles.
                                        IS_CONTENT_SET = false;
                                        // Now we split the paragraph in its parts.
                                        // Since an Excel loop has the pattern 00:00:00:00 CHARACTER NAME
                                        // The parts array will have to parts.
                                        string[] parts = Regex.Split(text, Globals.RTF_SMPTE_PATTERN);
                                        // We must check for correct parts.
                                        if (parts != null && parts.Length == 2 && !String.IsNullOrEmpty(parts[1]))
                                        {
                                            // The first index is empty,
                                            // So the second index (1) is the character name plus blank spaces.
                                            character = GetCharacter(characters, parts);
                                            // Now the we have a character, we create a new loop.
                                            // We set the loop timecode to the matched string, using
                                            ProgramLoops.Add(new Loop()
                                            {
                                                InTimecode = timecodes[0].Value,
                                                OutTimecode = timecodes[1].Value,
                                                Timecode = rtfMatch.Value.Trim(),
                                                CharacterName = character.Name,
                                                Mode = DocumentMode.RTF
                                            });
                                        }
                                        else
                                        {
                                            errors.Add(text);
                                        }
                                    }
                                    else
                                    {
                                        errors.Add(text);
                                    }
                                }
                                else
                                {
                                    errors.Add(text);
                                }
                            }
                            else
                            {
                                // We are inside an Excel loop.
                                // Since we are in a new loop, no content has been set yet,
                                // so we reset the content flag.
                                // This flag is used to allow for multiple-paragraph loop content,
                                // as in the case of titles.
                                IS_CONTENT_SET = false;
                                // Now we split the paragraph in its parts.
                                // Since an Excel loop has the pattern 00:00:00:00 CHARACTER NAME
                                // The parts array will have to parts.
                                string[] parts = Regex.Split(text, Globals.GENERAL_SMPTE_PATTERN);
                                // We must check for correct parts.
                                if (parts != null && parts.Length == 2 && !String.IsNullOrEmpty(parts[1]))
                                {
                                    // Get character.
                                    character = GetCharacter(characters, parts);
                                    // Now the we have a character, we create a new loop.
                                    // We set the loop timecode to the matched string, using
                                    ProgramLoops.Add(new Loop()
                                    {
                                        Timecode = generalMatch.Value.Trim(),
                                        InTimecode = generalMatch.Value.Trim(),
                                        OutTimecode = null,
                                        CharacterName = character.Name,
                                        Mode = DocumentMode.Excel
                                    });
                                }
                                else
                                {
                                    errors.Add(text);
                                }
                            }
                        }
                        else
                        {
                            // If there is at least one error, no loops will really be created.
                            // This method becomes an error finding one.
                            if (errors.Count == 0)
                            {
                                if (DOCUMENT_STARTED)
                                {
                                    // If we are here, then we are not in a loop definition paragraph.
                                    // The mode of the loop should be defined in the loop creation routine above.
                                    switch (ProgramLoops.Last().Mode)
                                    {
                                        case DocumentMode.Excel:
                                        case DocumentMode.RTF:

                                            if (IS_CONTENT_SET)
                                            {
                                                ProgramLoops.Last().CharacterDialog += newLine;
                                            }

                                            // Remove extra white spaces.
                                            text = Utilities.RemoveWhiteSpaces(text);

                                            // Set text.
                                            ProgramLoops.Last().CharacterDialog += text.Trim();

                                            // Count lines. This formula comes from the client.
                                            int lines = (int)Math.Ceiling((double)text.Length / charactersPerLine);

                                            // Add it the loop.
                                            ProgramLoops.Last().LoopLines += lines;

                                            // We get the character...
                                            string characterName = String.Empty;

                                            characterName = ProgramLoops.Last().CharacterName.Trim().ToUpper();

                                            // ...find it in the collection...
                                            character = characters.Where(i => i.Name.ToUpper() == characterName).FirstOrDefault();

                                            // ...and add the lines.
                                            character.Lines += lines;

                                            // Set content flag.
                                            IS_CONTENT_SET = true;

                                            break;

                                        case DocumentMode.Subtitles:

                                            // If first subtitle is empty, assgin it.
                                            if (String.IsNullOrEmpty(ProgramLoops.Last().Subtitles[0]))
                                            {
                                                ProgramLoops.Last().Subtitles[0] = text;
                                            }
                                            // If not, assign it to the second.
                                            else if (String.IsNullOrEmpty(ProgramLoops.Last().Subtitles[1]))
                                            {
                                                ProgramLoops.Last().Subtitles[1] = text;
                                            }
                                            else
                                            {
                                                MessageBox.Show($"More than two lines of subtitles were found at {ProgramLoops.Last().InTimecode}.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                                            }

                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (errors.Count == 0)
            {
                // Set first and last timecodes.
                switch (ProgramLoops.First().Mode)
                {
                    case DocumentMode.Subtitles:
                    case DocumentMode.RTF:
                        FirstTimecode = ProgramLoops.First().InTimecode;
                        break;
                    case DocumentMode.Excel:
                        FirstTimecode = ProgramLoops.First().InTimecode;
                        break;

                }

                switch (ProgramLoops.Last().Mode)
                {
                    case DocumentMode.Subtitles:
                    case DocumentMode.RTF:
                        LastTimecode = ProgramLoops.Last().OutTimecode;
                        break;
                    case DocumentMode.Excel:
                        LastTimecode = ProgramLoops.Last().InTimecode;
                        break;

                }
            }

            return (errors.Count > 0) ? errors : null;
        }

        private static Character GetCharacter(ObservableCollection<Character> characters, string[] parts)
        {
            Character character;
            // Let's get the character name and clean it up by removing
            // extra blank characters and converting it to uppercase (just in case).
            string characterName = parts[1].Trim().ToUpper();
            // Using the characters collection passed as parameter,
            // we select the corresponding character object from its name, using LINQ.
            character = characters.Where(i => i.Name.ToUpper() == characterName).FirstOrDefault();
            // Since the FirstOrDefault method can return null
            // if no character is found, we check for a null character.
            if (character == null)
            {
                // And create a new one.
                character = new Character()
                {
                    Name = parts[1].Trim().ToUpper()
                };
                // Add it to list.
                characters.Add(character);
            }

            return character;
        }

        /// <summary>
        /// Extracts timecodes from a string.
        /// </summary>
        /// <param name="text">String to search.</param>
        /// <returns>Collection of Regex mateches.</returns>
        private MatchCollection FindTimecodes(string text)
        {
            // Now we extract the in and out timecodes.
            // First, we find the matches.
            MatchCollection timecodes = Regex.Matches(text, Globals.GENERAL_SMPTE_PATTERN);

            // The timecodes collection should contain two elements,
            // But let's check anyway.
            if (timecodes.Count < 2 || timecodes.Count > 2)
            {
                return null;
            }

            return timecodes;
        }

        /// <summary>
        /// Checks every Excel or subtitle loop for sequentiality.
        /// </summary>
        /// <param name="documentMode">The type of loop to check. Values are Excel and Subtitle.</param>
        public List<string> CheckSequentiality()
        {
            // Declare holders for each type of loop.
            Loop lastExcelLoop = null;
            Loop lastSubtitleLoop = null;
            // Temporary substraction result holder.
            SMPTE resultSMPTE = null;
            // The result object.
            List<string> result = new List<string>();

            foreach (Loop loop in ProgramLoops)
            {
                switch (loop.Mode)
                {
                    case DocumentMode.Excel:
                        // Check last Excel loop.
                        if (lastExcelLoop != null)
                        {
                            // Only null for first loop.
                            // Try to substract them to find out
                            // if the 2nd occurs after the first.
                            try
                            {
                                resultSMPTE = new SMPTE(loop.InTimecode) - new SMPTE(lastExcelLoop.InTimecode);
                                // Check for the same time.
                                if (resultSMPTE.ConvertToFrames() == 0)
                                {
                                    // They occur at the same time.
                                    // We should add this loop to result as an invalid one.
                                    result.Add(loop.InTimecode);
                                }
                            }
                            catch
                            {
                                // Last loop occurs after current loop.
                                // We should add this loop to result as an invalid one.
                                result.Add(loop.InTimecode);
                            }
                        }
                        // Finally, we set this loop as the last one.
                        lastExcelLoop = loop;

                        break;

                    case DocumentMode.Subtitles:
                        // First, we check if the loop itself is valid.
                        try
                        {
                            resultSMPTE = new SMPTE(loop.OutTimecode) - new SMPTE(loop.InTimecode);
                            // Check for the same time.
                            if (resultSMPTE.ConvertToFrames() == 0)
                            {
                                // They occur at the same time.
                                // We should add this loop to result as an invalid one.
                                result.Add(loop.InTimecode);
                            }
                        }
                        catch
                        {
                            // Out loop occurs after first loop.
                            // We should add this loop to result as an invalid one.
                            result.Add(loop.InTimecode);
                        }

                        // Now we check it against the last one.
                        if (lastSubtitleLoop != null)
                        {
                            // Only null for the first one.
                            // Try to substract them to find out
                            // if the 2nd occurs after the first.
                            try
                            {
                                resultSMPTE = new SMPTE(loop.InTimecode) - new SMPTE(lastSubtitleLoop.OutTimecode);
                                // Check for the same time.
                                if (resultSMPTE.ConvertToFrames() == 0)
                                {
                                    // They occur at the same time.
                                    // We should add this loop to result as an invalid one.
                                    result.Add(loop.InTimecode);
                                }
                            }
                            catch
                            {
                                // Last loop occurs after current loop.
                                // We should add this loop to result as an invalid one.
                                result.Add(loop.InTimecode);
                            }
                        }
                        // Finally, we set this loop as the last one.
                        lastSubtitleLoop = loop;

                        break;

                    case DocumentMode.RTF:
                        // We check if the loop itself is valid.
                        try
                        {
                            resultSMPTE = new SMPTE(loop.OutTimecode) - new SMPTE(loop.InTimecode);
                        }
                        catch
                        {
                            // Out loop occurs after first loop.
                            // We should add this loop to result as an invalid one.
                            result.Add(loop.InTimecode);
                        }

                        break;
                }
            }
            // Return the list if it has elements.
            return result.Count > 0 ? result : null;
        }


        #endregion
    */
    }
}
