using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Documents;

namespace SyncLoopLibrary
{
    public static partial class Utilities
    {

        /// <summary>
        /// Reads characters from current document in any mode.
        /// </summary>
        /// <param name="document">Flow document.</param>
        /// <returns>Observable collection of characters.</returns>
        public static ObservableCollection<Character> ReadCharactersFromDocument(FlowDocument document)
        {
            // Create return object.
            ObservableCollection<Character> result = new ObservableCollection<Character>();
            // General match for SMPTE string.
            Match generalMatch = null;
            // Working character.
            Character character;

            // Iterate.
            foreach (Paragraph paragraph in document.Paragraphs())
            {
                // Get paragraph content.
                string text = new TextRange(paragraph.Inlines.FirstInline.ContentStart, paragraph.Inlines.FirstInline.ContentEnd).Text;

                // If it is not empty...
                if (!String.IsNullOrWhiteSpace(text))
                {
                    // Search for general loop.
                    generalMatch = Regex.Match(text, Globals.GENERAL_SMPTE_PATTERN);
                
                    // If a match is found...
                    if (generalMatch.Success)
                    {
                        // We are in a loop definition paragraph. Let's find out what kind of loop it is.
                        if (text.StartsWith("SUB["))
                        {
                            // Do nothing, since those do not have characters.
                        }
                        else if (text.StartsWith("DUB["))
                        {

                            // Now we split the paragraph in its parts.
                            string[] parts = Regex.Split(text, Globals.RTF_SMPTE_PATTERN);
                            // The second index (1) is the character name plus blank spaces.
                            // Let's get the character name and clean it up by removing
                            // extra blank characters and converting it to uppercase (just in case).
                            string characterName = parts[1].Trim().ToUpper();
                            // Check if character is already added.
                            character = result.Where(i => i.Name.ToUpper() == characterName).FirstOrDefault();
                            // If no character is found...
                            if (character == null)
                            {
                                // And create a new one.
                                character = new Character()
                                {
                                    Name = parts[1].Trim().ToUpper()
                                };
                                // Add it to list.
                                result.Add(character);
                            }
                        }
                        else
                        {
                            // Since an Excel loop has the pattern 00:00:00:00 CHARACTER NAME
                            // The parts array will have to parts.
                            string[] parts = Regex.Split(text, Globals.GENERAL_SMPTE_PATTERN);
                            // Let's get the character name and clean it up by removing
                            // extra blank characters and converting it to uppercase (just in case).
                            string characterName = parts[1].Trim().ToUpper();
                            // Check if character is already added.
                            character = result.Where(i => i.Name.ToUpper() == characterName).FirstOrDefault();
                            // If no character is found...
                            if (character == null)
                            {
                                // And create a new one.
                                character = new Character()
                                {
                                    Name = parts[1].Trim().ToUpper()
                                };
                                // Add it to list.
                                result.Add(character);
                            }
                        }
                    }
                }
            }

            // Return result.
            return result;
        }

    }
}
