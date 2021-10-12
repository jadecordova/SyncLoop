using System;
using System.Windows.Documents;

namespace SyncLoopLibrary
{

    /// <summary>
    /// This class encapsulates the find and replace operations for FlowDocument.
    /// </summary>
    public sealed class FindAndReplaceManager
    {
        #region FIELDS

        private FlowDocument inputDocument;
        private TextPointer currentPosition;

        #endregion



        #region CONSTRUCTORS

        /// <summary>
        /// Initializes a new instance of the FindReplaceManager class given the specified FlowDocument instance.
        /// </summary>
        /// <param name="document">the input document</param>
        public FindAndReplaceManager(FlowDocument document)
        {
            inputDocument   = document ?? throw new ArgumentNullException("Document is null.");

            currentPosition = document.ContentStart;
        }

        #endregion


        #region METHODS

        /// <summary>
        /// Gets and sets the offset position for the FindReplaceManager/>
        /// </summary>
        public TextPointer CurrentPosition
        {
            get
            {
                return currentPosition;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                if (value.CompareTo(inputDocument.ContentStart) < 0 ||
                    value.CompareTo(inputDocument.ContentEnd) > 0)
                {
                    throw new ArgumentOutOfRangeException("value");
                }

                currentPosition = value;
            }
        }

        /// <summary>
        /// Find next match of the input string.
        /// </summary>
        /// <param name="input">The string to search for a match.</param>
        /// <param name="findOptions">the search options</param>
        /// <returns>The <see cref="TextRange"/> instance representing the input string.</returns>
        /// <remarks>
        /// This method will advance the <see cref="CurrentPosition"/> to next context position.
        /// </remarks>
        public TextRange FindNext(String input, FindOptions findOptions)
        {
            TextRange textRange = GetTextRangeFromPosition(input, findOptions);

            return textRange;
        }

        /// <summary>
        /// Within a specified input string, replaces the input string that
        /// match a regular expression pattern with a specified replacement string.
        /// </summary>
        /// <param name="input">The string to search for a match.</param>
        /// <param name="replacement">The replacement string.</param>
        /// <param name="findOptions"> the search options</param>
        /// <returns>The <see cref="TextRange"/> instance representing the replacement string.</returns>
        /// <remarks>
        /// This method will advance the <see cref="CurrentPosition"/> to next context position.
        /// </remarks>
        public TextRange Replace(String input, String replacement, FindOptions findOptions)
        {
            TextRange textRange = FindNext(input, findOptions);
            if (textRange != null)
            {
                textRange.Text = replacement;
            }

            return textRange;
        }

        /// <summary>
        /// Within a specified input string, replaces all the input strings that
        /// match a specified criteria with a specified replacement string.
        /// </summary>
        /// <param name="input">The string to search for a match.</param>
        /// <param name="replacement">The replacement string.</param>
        /// <param name="findOptions"> the search options</param>
        /// <param name="action">the action performed for each match of the input string.</param>
        /// <returns>The number of times the replacement can occur.</returns>
        /// <remarks>
        /// This method will advance the<see cref="CurrentPosition"/> to last context position.
        /// </remarks>
        public Int32 ReplaceAll(String input, String replacement, FindOptions findOptions, Action<TextRange> action)
        {
            Int32 count = 0;


            currentPosition = inputDocument.ContentStart;
            while (currentPosition.CompareTo(inputDocument.ContentEnd) < 0)
            {
                TextRange textRange = Replace(input, replacement, findOptions);

                if (textRange != null)
                {
                    CurrentPosition = textRange.End;
                    count++;
                    action?.Invoke(textRange);
                }
            }

            return count;
        }

        /// <summary>
        /// Finds the corresponding <see cref="TextRange"/> instance 
        /// representing the input string given a specified text pointer position.
        /// </summary>
        /// <param name="findOptions">Options.</param>
        /// <param name="input">Input string.</param>
        /// <returns>
        /// An <see cref="TextRange"/> instance represeneting the matching
        /// string withing the text container.
        /// </returns>
        public TextRange GetTextRangeFromPosition(String input, FindOptions findOptions)
        {
            Boolean matchCase = (findOptions & FindOptions.MatchCase) == FindOptions.MatchCase;
            Boolean matchWholeWord = (findOptions & FindOptions.MatchWholeWord) == FindOptions.MatchWholeWord;

            TextRange textRange = null;

            while (CurrentPosition != null)
            {
                if (CurrentPosition.CompareTo(inputDocument.ContentEnd) >= 0)
                {
                    break;
                }

                if (CurrentPosition.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                {
                    // GET TEXT FROM POSITION ONWARDS.
                    string textRun = CurrentPosition.GetTextInRun(LogicalDirection.Forward);
                    // SET THE COMPARISON FOR CASE.
                    StringComparison stringComparison = matchCase ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase;
                    // SEARCH FOR TEXT.
                    int indexInRun = textRun.IndexOf(input, stringComparison);
                    // IF FOUND...
                    if (indexInRun >= 0)
                    {
                        // GET A POINTER TO THE POSITION OF FIRST CHAR.
                        CurrentPosition = CurrentPosition.GetPositionAtOffset(indexInRun);
                        // GET A POINTER TO THE END OF THE SEARCH TEXT.
                        TextPointer nextPointer = CurrentPosition.GetPositionAtOffset(input.Length);
                        // CREATE A TEXT RANGE.
                        textRange = new TextRange(CurrentPosition, nextPointer);
                        // WHOLE WORD CHECK.
                        if (matchWholeWord)
                        {
                            // Test if the "textRange" represents a word.
                            if (IsWholeWord(textRange)) 
                            {
                                // If a WholeWord match is found, directly terminate the loop.
                                CurrentPosition = CurrentPosition.GetPositionAtOffset(input.Length);
                                break;
                            }
                            else
                            {
                                // If a WholeWord match is not found, go to next recursion to find it.
                                CurrentPosition = CurrentPosition.GetPositionAtOffset(input.Length);
                                return GetTextRangeFromPosition(input, findOptions);
                            }
                        }
                        else
                        {
                            // MOVE POINTER TO END OF STRING.
                            CurrentPosition = CurrentPosition.GetPositionAtOffset(input.Length);
                            // MOVE IT TO NEXT INSERTION POSITION IF IT IS NOT ALREADY THERE.
                            if (!CurrentPosition.IsAtInsertionPosition)
                            {
                                CurrentPosition.GetNextInsertionPosition(LogicalDirection.Forward);
                            }
                            break;
                        }
                    }
                    else
                    {
                        // If a match is not found, go over to the next context CurrentPosition after the "textRun".
                        CurrentPosition = CurrentPosition.GetNextContextPosition(LogicalDirection.Forward);
                    }
                }
                else
                {
                    //If the current CurrentPosition doesn't represent a text context CurrentPosition, go to the next context CurrentPosition.
                    // This can effectively ignore the formatting or embed element symbols.
                    CurrentPosition = CurrentPosition.GetNextContextPosition(LogicalDirection.Forward);
                }
            }

            return textRange;
        }

        /// <summary>
        /// determines if the specified character is a valid word character.
        /// here only underscores, letters, and digits are considered to be valid.
        /// </summary>
        /// <param name="character">character specified</param>
        /// <returns>Boolean value didicates if the specified character is a valid word character</returns>
        private Boolean IsWordChar(Char character)
        {
            return Char.IsLetterOrDigit(character) || character == '_';
        }

        /// <summary>
        /// Tests if the string within the specified <see cref="TextRange"/>instance is a word.
        /// </summary>
        /// <param name="textRange"> <see cref="TextRange"/> instance to test</param>
        /// <returns>test result</returns>
        private Boolean IsWholeWord(TextRange textRange)
        {
            Char[] chars = new Char[1];

            if (textRange.Start.CompareTo(inputDocument.ContentStart) == 0 || textRange.Start.IsAtLineStartPosition)
            {
                textRange.End.GetTextInRun(LogicalDirection.Forward, chars, 0, 1);
                return !IsWordChar(chars[0]);
            }
            else if (textRange.End.CompareTo(inputDocument.ContentEnd) == 0)
            {
                textRange.Start.GetTextInRun(LogicalDirection.Backward, chars, 0, 1);
                return !IsWordChar(chars[0]);
            }
            else
            {
                textRange.End.GetTextInRun(LogicalDirection.Forward, chars, 0, 1);
                if (!IsWordChar(chars[0]))
                {
                    textRange.Start.GetTextInRun(LogicalDirection.Backward, chars, 0, 1);
                    return !IsWordChar(chars[0]);
                }
            }

            return false;
        }

        #endregion
    }
}
