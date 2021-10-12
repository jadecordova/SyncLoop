using System;
using System.Text;
using System.Text.RegularExpressions;

namespace SyncLoopLibrary
{
    /// <summary>
    /// Contains complete loop info.
    /// </summary>
    public class Loop
    {

        #region PROPERTIES

        /// <summary>
        /// Timecode string.
        /// </summary>
        public string Timecode { get; set; }

        /// <summary>
        /// In Timecode string.
        /// </summary>
        public string InTimecode { get; set; }

        /// <summary>
        /// Out Timecode string.
        /// </summary>
        public string OutTimecode { get; set; }

        /// <summary>
        /// Text spoken by character.
        /// </summary>
        public string CharacterDialog { get; set; }

        /// <summary>
        /// Number of lines in loop.
        /// </summary>
        public int LoopLines { get; set; }

        /// <summary>
        /// In Timecode string.
        /// </summary>
        public string InTimecodeSUB { get; set; }

        /// <summary>
        /// Out Timecode string.
        /// </summary>
        public string OutTimecodeSUB { get; set; }

        /// <summary>
        /// In Timecode string.
        /// </summary>
        public string InTimecodeSRT { get; set; }

        /// <summary>
        /// Out Timecode string.
        /// </summary>
        public string OutTimecodeSRT { get; set; }

        /// <summary>
        /// Subtitles (up to 2 lines).
        /// </summary>
        public string[] Subtitles { get; set; } = new string[2];

        /// <summary>
        /// Subtitle type.
        /// </summary>
        public DocumentMode Mode { get; set; }

        /// <summary>
        /// Loop character.
        /// </summary>
        public Character Character { get; set; }
        
        /// <summary>
        /// Stores the actual in video frame.
        /// </summary>
        public long InFrame { get; set; }

        /// <summary>
        /// Stores the actual out video frame.
        /// </summary>
        public long OutFrame { get; set; }

        /// <summary>
        /// Stores the frames block for subtitles.
        /// </summary>
        public string FramesBlock { get; set; } = "[ERROR]";

        #endregion



        #region CONSTRUCTORS

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Loop()
        {

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="timecode">Timecode string.</param>
        /// <param name="dialog">Text spoken by character.</param>
        /// <param name="lines">Number of lines spoken.</param>
        public Loop(string timecode, string dialog, int lines)
        {
            Timecode = timecode;

            CharacterDialog = dialog;

            LoopLines = lines;
        }

        #endregion



        #region METHODS

        /// <summary>
        /// Swaps last colon for a period to convert loop
        /// to .sub format.
        /// </summary>
        public void ToSubFormat()
        {
            InTimecodeSUB = Regex.Replace(InTimecode, @"(.*)[:](.*)", "$1.$2");

            OutTimecodeSUB = Regex.Replace(OutTimecode, @"(.*)[:](.*)", "$1.$2");
        }


        /// <summary>
        /// Swaps last colon for a period to convert loop
        /// to .sub format.
        /// </summary>
        public void ToSRTFormat()
        {
            SMPTE inSMPTE = new SMPTE((int)InFrame);

            SMPTE outSMPTE = new SMPTE((int)OutFrame);

            InTimecodeSRT = $"{inSMPTE.TimecodeTokens[0]:D2}:{inSMPTE.TimecodeTokens[1]:D2}:{inSMPTE.TimecodeTokens[2]:D2},{inSMPTE.ConvertFramesToMilliseconds(29.97):D3}";

            OutTimecodeSRT = $"{outSMPTE.TimecodeTokens[0]:D2}:{outSMPTE.TimecodeTokens[1]:D2}:{outSMPTE.TimecodeTokens[2]:D2},{outSMPTE.ConvertFramesToMilliseconds(29.97):D3}";
        }

        #endregion



        #region OVERRIDES

        /// <summary>
        /// Overrides ToString().
        /// </summary>
        /// <returns>Complete loop info.</returns>
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            string name = (Character == null) ? "NO NAME" : Character.Name;

            result.Append($"CHARACTER NAME: {name}"      + Environment.NewLine);
            result.Append($"TIMECODE: {Timecode}"        + Environment.NewLine);
            result.Append($"DIALOG: {CharacterDialog}"   + Environment.NewLine);
            result.Append($"LINES: {LoopLines}"          + Environment.NewLine);
            result.Append($"IN TIMECODE: {InTimecode}"   + Environment.NewLine);
            result.Append($"OUT TIMECODE: {OutTimecode}" + Environment.NewLine);
            result.Append($"SUBTITLES 1: {Subtitles[0]}" + Environment.NewLine);
            result.Append($"SUBTITLES 2: {Subtitles[1]}" + Environment.NewLine);
            result.Append($"MODE: {Mode.ToString()}");

            return result.ToString();
        }

        #endregion
    }
}
