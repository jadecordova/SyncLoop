using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Documents;

namespace SyncLoopLibrary
{
    /// <summary>
    /// Program loops collection.
    /// </summary>
    public class Loops
    {
        #region FIELDS

        /// <summary>
        /// All program loops list.
        /// </summary>
        // All program loops.
        public List<Loop> ProgramLoops { get; set; } = new List<Loop>();

        /// <summary>
        /// First time code of document, for calculating duration.
        /// </summary>
        public string FirstTimecode { get; set; } = null;

        /// <summary>
        /// Last time code of document, for calculating duration.
        /// </summary>
        public string LastTimecode { get; set; } = null;

        #endregion



        #region METHODS

        /// <summary>
        /// Extracts loops from document.
        /// </summary>
        /// <param name="document">Flow document.</param>
        /// <param name="characters">Observable collection of program characters.</param>
        /// <param name="newLine">New line character for the specified document mode.</param>
        /// <param name="charactersPerLine">Maximum characters per line, for subtitles.</param>
        /// <returns>List of errors or null if everything went OK.</returns>
        public List<string> ExtractLoops(FlowDocument document,
                                         ObservableCollection<Character> characters,
                                         string newLine,
                                         int charactersPerLine)
        {
            // Result object.
            // This list will contain any errors found in the document's loops.
            List<string> errors = new List<string>();

            // General match for SMPTE string.
            Match generalMatch = null;

            // This flag signals if the actual document started. It is set after the first loop is found.
            // This prevents errors when creating the final document ecause before this, characters are null.
            bool CONTENT_STARTED = false;

            // Flag for multiple paragraphs per characters, as in the case of titles.
            bool ALREADY_HAS_CONTENT = false;

            // The Paragraphs() method is a custom extension method.
            foreach (Paragraph paragraph in document.Paragraphs())
            {
                // Get the paragraph text.
                string text = new TextRange(paragraph.ContentStart, paragraph.ContentEnd).Text;

                // Check for empty paragraph.
                if (!String.IsNullOrWhiteSpace(text))
                {
                    // Search for loop paragraph.
                    generalMatch = Regex.Match(text, Globals.GENERAL_SMPTE_PATTERN);

                    if (generalMatch.Success)
                    {
                        // Since there was a match, we know the content has started.
                        CONTENT_STARTED = true;

                        // We are in a loop definition paragraph. Let's find out what kind of loop it is.
                        // First, we test for subtitle loop by searching for the SUB[ string at the beginning of the paragraph.
                        if (text.StartsWith("SUB["))
                        {
                            Loop currentLoop = CreateLoop(text, DocumentMode.Subtitles, characters);

                            // If everything went right, we add it to the loops list.
                            if (currentLoop != null)
                            {
                                ProgramLoops.Add(currentLoop);
                            }

                            else errors.Add(text);
                        }

                        // Test for RTF paragraph.
                        else if (text.StartsWith("DUB["))
                        {
                            Loop currentLoop = CreateLoop(text, DocumentMode.RTF, characters);

                            // If everything went right, we add it to the loops list.
                            if (currentLoop != null)
                            {
                                ProgramLoops.Add(currentLoop);

                                // Reset the content flag, since this is a new loop.
                                ALREADY_HAS_CONTENT = false;
                            }

                            else errors.Add(text);
                        }

                        // Here, it must be an Excel loop.
                        else
                        {
                            Loop currentLoop = CreateLoop(text, DocumentMode.Excel, characters);

                            // If everything went right, we add it to the loops list.
                            if (currentLoop != null)
                            {
                                ProgramLoops.Add(currentLoop);

                                // Reset the content flag, since this is a new loop.
                                ALREADY_HAS_CONTENT = false;
                            }

                            else errors.Add(text);
                        }
                    }

                    // If not, treat it as a content paragraph.
                    else
                    {
                        // If there is at least one error, no loops will really be created.
                        // This method becomes an error finding one.
                        if (errors.Count == 0)
                        {
                            // Check if content has already started.
                            if (CONTENT_STARTED)
                            {

                                // If we are here, then we are not in a loop definition paragraph.
                                // Remove extra white spaces.
                                text = Utilities.RemoveWhiteSpaces(text);

                                // The mode of the loop should be defined in the loop creation routine above.
                                switch (ProgramLoops.Last().Mode)
                                {

                                    case DocumentMode.Excel:
                                    case DocumentMode.RTF:

                                        if (ALREADY_HAS_CONTENT)
                                        {
                                            ProgramLoops.Last().CharacterDialog += newLine;
                                        }

                                        // Set text.
                                        ProgramLoops.Last().CharacterDialog += text.Trim();

                                        // Count lines. This formula comes from the client.
                                        int lines = (int)Math.Ceiling((double)text.Length / charactersPerLine);

                                        // Add it the loop.
                                        ProgramLoops.Last().LoopLines += lines;

                                        // ...and add the lines.
                                        ProgramLoops.Last().Character.Lines += lines;

                                        // Set content flag.
                                        ALREADY_HAS_CONTENT = true;

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

                                        // Subtitles cannot have more than 2 lines.
                                        else
                                        {
                                            errors.Add(text);
                                        }

                                        break;
                                }
                            }
                        }
                    }
                }
            }

            if (errors.Count == 0)
            {
                // Set first and last timecodes. Is the same for all modes.
                FirstTimecode = ProgramLoops.First().InTimecode;

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


        /// <summary>
        /// Creates the actual loop.
        /// </summary>
        /// <param name="text">Text to extract loop info.</param>
        /// <param name="mode">Document mode.</param>
        /// <param name="characters">List of program characters.</param>
        /// <returns>Loop object or null if invalid.</returns>
        private Loop CreateLoop(string text, DocumentMode mode, ObservableCollection<Character> characters)
        {
            // Create frames array.
            long[] frames = null;
            // Create frames variables.
            long inFrame = 0, outFrame = 0;
            // Create search pattern based on mode.
            string pattern = string.Empty;
            // Temporary holder for the character name search.
            Character character = null;

            switch (mode)
            {
                case DocumentMode.Subtitles:

                    pattern = Globals.SUBTITLE_PATTERN;
                    break;

                case DocumentMode.RTF:

                    pattern = Globals.RTF_SMPTE_PATTERN;
                    break;

                case DocumentMode.Excel:

                    pattern = Globals.GENERAL_SMPTE_PATTERN;
                    break;
            }

            // Get complete timecode string.
            Match timecode = Regex.Match(text, pattern);

            // Check for complete timecode string.
            if (timecode.Success)
            {
                // Now we split the text in its parts.
                string[] parts = Regex.Split(text, pattern);

                // If there is a second part in the text, it is the character name in RTF or Excel modes, or the frames block
                // in subtitles mode.
                if (parts != null && parts.Length == 2 && !String.IsNullOrWhiteSpace(parts[1]))
                {
                    if (mode != DocumentMode.Subtitles)
                    {
                        // The first index is empty, so the second index (1) is the character name plus blank spaces.
                        character = GetCharacter(characters, parts[1]);
                    }
                    else
                    {
                        // Get the frames.
                        frames = GetFrames(parts[1]);
                    }
                }

                // Now we extract the in and out timecodes. First, we find the matches.
                MatchCollection timecodes = FindTimecodes(text);
                // Assign this value to avoid index out of range exceptions when working with Excel loops.
                string inTimecode = timecodes[0].Value;

                string outTimecode = (timecodes.Count > 1) ? timecodes[1].Value : null;

                // Get frames.
                if (frames != null && frames.Length == 2)
                {
                    inFrame = frames[0];
                    outFrame = frames[1];
                }

                // Just to be on the safe side, we check for null.
                if (!String.IsNullOrWhiteSpace(inTimecode))
                {
                    // Return loop.
                    return new Loop()
                    {
                        Timecode = timecode.Value,
                        InTimecode = inTimecode,
                        OutTimecode = outTimecode,
                        Character = character,
                        InFrame = inFrame,
                        OutFrame = outFrame,
                        FramesBlock = $"[{inFrame}:{outFrame}]",
                        // InTimeSpan = spans[0],
                        // OutTimeSpan = spans[1],
                        Mode = mode
                    };
                }

                else return null;
            }

            else return null;
        }


        /// <summary>
        /// Gets matching character from program's character list.
        /// </summary>
        /// <param name="characters">Program characters list.</param>
        /// <param name="name">Dirty name of character.</param>
        /// <returns>Character object if exists, or newly added character to list.</returns>
        private static Character GetCharacter(ObservableCollection<Character> characters, string name)
        {
            Character character;

            // Let's get the character name and clean it up by removing
            // extra blank characters and converting it to uppercase (just in case).
            string characterName = name.Trim().ToUpper();
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
                    Name = characterName
                };
            
                // Add it to list.
                characters.Add(character);
            }

            return character;
        }


        /// <summary>
        /// Gets the timecodes match collections.
        /// </summary>
        /// <param name="text"></param>
        /// <returns>Match collection of timecode string.</returns>
        private MatchCollection FindTimecodes(string text)
        {
            // Now we extract the in and out timecodes. First, we find the matches.
            MatchCollection timecodes = Regex.Matches(text, Globals.GENERAL_SMPTE_PATTERN);
            // The timecodes collection should contain two elements.
            return timecodes;
        }


        /// <summary>
        /// Checks every Excel or subtitle loop for sequentiality.
        /// </summary>
        public List<string> CheckSequentiality()
        {
            // The result object containing errors.
            List<string> result = new List<string>();

            // Declare holders for each type of loop.
            Loop lastExcelLoop = null;

            Loop lastSubtitleLoop = null;
            // Temporary substraction result holder.
            SMPTE resultSMPTE = null;

            foreach (Loop loop in ProgramLoops)
            {
                switch (loop.Mode)
                {
                    case DocumentMode.Excel:
            
                        // Check last Excel loop.
                        if (lastExcelLoop != null)
                        {
                            // Only null for first loop. Try to substract them to find out
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
                            // Only null for the first one. Try to substract them to find out
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


        /// <summary>
        /// Gets the actual video frames from timecode string
        /// </summary>
        /// <param name="text">Timecode string.</param>
        /// <returns>Array of 2 longs.</returns>
        private long[] GetFrames(string text)
        {
            // Result object.
            long[] result = new long[2];
            // Remove square brackets.
            text = Regex.Replace(text, @"\[|\]", "");
            // Get numbers.
            string[] numbers = text.Split(':');

            // Get in frame.
            if (long.TryParse(numbers[0], out long inFrame))
            {
                result[0] = inFrame;
            }
            else return null;
            
            // Get out frame.
            if (long.TryParse(numbers[1], out long outFrame))
            {
                result[1] = outFrame;
            }
            else return null;

            return result;
        }

        #endregion

    }
}
