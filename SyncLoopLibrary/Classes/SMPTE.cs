using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace SyncLoopLibrary
{
    /// <summary>
    /// Core class of the application.
    /// Defines SMPTE properties and methods.
    /// </summary>
    public class SMPTE
    {

        #region FIELDS

        /// <summary>
        /// Number of frames per hour at 30 FPS.
        /// </summary>
        public static int FramesPerHour   = 108000;

        /// <summary>
        /// Number of frames per minute at 30 FPS.
        /// </summary>
        public static int FramesPerMinute = 1800;

        /// <summary>
        /// Number of frames per second at 30 FPS.
        /// </summary>
        public static int FramesPerSecond = 30;

        #endregion



        #region PROPERTIES

        /// <summary>
        /// Original timecode string.
        /// </summary>
        public string Timecode { get; set; }

        /// <summary>
        /// Parsed timecode tokens for hours, minutes, seconds and frames.
        /// </summary>
        public int[] TimecodeTokens { get; set; }

        #endregion



        #region CONSTRUCTORS

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SMPTE() { }

        /// <summary>
        /// Loop constructor from string.
        /// </summary>
        /// <param name="timecode">SMPTE format timcode string.</param>
        public SMPTE(string timecode)
        {
            string[] stringTokens;
            // Check for colons.
            if (timecode.Contains(":"))
            {
                // Split a colons.
                stringTokens = timecode.Split(':');
            }
            else
            {
                if (timecode.Length < 8)
                {
                    // Pad with zeroes.
                    timecode += new string('0', (8 - timecode.Length));
                }
                else if (timecode.Length > 8)
                {
                    // Get first 8 characters.
                    timecode = timecode.Substring(0, 8);
                }
                // Get string tokens.
                stringTokens = new string[]
                {
                        timecode.Substring(0, 2),
                        timecode.Substring(2, 2),
                        timecode.Substring(4, 2),
                        timecode.Substring(6, 2)
                };
            }
            // Convert strings to ints.
            try
            {
                TimecodeTokens = new int[]
                {
                        Convert.ToInt32(stringTokens[0]),
                        Convert.ToInt32(stringTokens[1]),
                        Convert.ToInt32(stringTokens[2]),
                        Convert.ToInt32(stringTokens[3])
                };

                UpdateTimecode();
            }
            catch (Exception ex)
            {
                // Log($"Could not create loop from string: {ex.Message}. Default SMPTE 0 returnedS");

                // Default to 0 SMPTE.
                TimecodeTokens = new int[] { 0, 0, 0, 0 };

                UpdateTimecode();
            }
        }

        /// <summary>
        /// Loop constructor from int array.
        /// </summary>
        /// <param name="timecodeTokens"></param>
        public SMPTE(int[] timecodeTokens)
        {
            if (timecodeTokens != null && timecodeTokens.Length == 4)
            {
                // Assign member.
                TimecodeTokens = timecodeTokens;
            }
            else
            {
                // Log("Attempt to create a SMPTE object from invalid int[].");

                // Create default 0 SMPTE.
                TimecodeTokens = new int[4];
                // Use the provided ints.
                for (int i = 0; i < 4; i++)
                {
                    if (i < timecodeTokens.Length)
                        TimecodeTokens[i] = timecodeTokens[i];
                    else
                        TimecodeTokens[i] = 0;
                }
            }
            // Create timecode string.
            UpdateTimecode();
        }

        /// <summary>
        /// Constructor from number of frames.
        /// </summary>
        /// <param name="frames">Number of frames</param>
        public SMPTE(int frames)
        {
            TimecodeTokens = ConvertFromFrames(frames);
            // Create timecode string.
            UpdateTimecode();
        }

        /// <summary>
        /// Constructor from TimeSpan.
        /// </summary>
        /// <param name="timeSpan">TimeSpan object.</param>
        public SMPTE(TimeSpan timeSpan)
        {
            if (timeSpan != null)
            {
                // Convert TimeSpan.
                TimecodeTokens = ConvertFromTimeSpan(timeSpan);
            }
            else
            {
                // Log("Attempt to create SMPTE from invaid Timespan");

                //Default to 0.
                TimecodeTokens = new int[] { 0, 0, 0, 0 };
            }
            // Create timecode string.
            UpdateTimecode();
        }

        /// <summary>
        /// Constructo from milliseconds.
        /// </summary>
        /// <param name="milliseconds">Number of milliseconds.</param>
        public SMPTE(long milliseconds)
        {
            // Convert milliseconds to frames and then to timecode.
            TimecodeTokens = ConvertFromFrames(ConvertMillisecondsToFrames(milliseconds));
            // Create timecode string.
            UpdateTimecode();
        }

        #endregion



        #region OPERATOR OVERLOAD

        /// <summary>
        /// Binary add (+) operator overload.
        /// </summary>
        /// <param name="smpte1">Loop object to add.</param>
        /// <param name="smpte2">Loop object to add.</param>
        /// <returns>Loop object tha results from adding each corresponding token.
        /// No normalization is performed on the sum.</returns>
        public static SMPTE operator + (SMPTE smpte1, SMPTE smpte2)
        {
            // Sum tokens.
            int[] sums = new int[]
            {
                smpte1.TimecodeTokens[0] + smpte2.TimecodeTokens[0],
                smpte1.TimecodeTokens[1] + smpte2.TimecodeTokens[1],
                smpte1.TimecodeTokens[2] + smpte2.TimecodeTokens[2],
                smpte1.TimecodeTokens[3] + smpte2.TimecodeTokens[3]
            };

            return new SMPTE(sums).Normalize();
        }

        /// <summary>
        /// Binary add (-) operator overload.
        /// </summary>
        /// <param name="smpte1">Loop object to substract from.</param>
        /// <param name="smpte2">Loop object to substract to.</param>
        /// <returns>
        /// Loop object tha results from substracting total frames
        /// of smpte1 from those of smpte2.
        /// Performs check to see if smpte2 is actually later than smpte1.
        /// Otherwise, it throws an exception.
        /// </returns>
        public static SMPTE operator - (SMPTE smpte1, SMPTE smpte2)
        {
            // Check if smpte1 is later or equal than smpte1.
            int frames1 = smpte1.ConvertToFrames();
            int frames2 = smpte2.ConvertToFrames();

            if (frames2 > frames1)
            {
                throw new ArgumentException("SMPTE 2 occurs after SMPTE 1.", "Loop 2");
            }

            int resultFrames = frames1 - frames2;

            return new SMPTE(resultFrames).Normalize();
        }

        #endregion



        #region OVERRIDES

        /// <summary>
        /// Overrides ToString().
        /// </summary>
        /// <returns>Timecode string.</returns>
        public override string ToString()
        {
            return Timecode;
        }

        #endregion



        #region METHODS

        /// <summary>
        /// Creates tokens from total frames.
        /// </summary>
        /// <param name="frames">Number of frames.</param>
        /// <returns>Int[] timecode tokens.</returns>
        public int[] ConvertFromFrames(int frames)
        {
            // Number of frames divided by number of frames per hour.
            int _hours = frames / FramesPerHour;
            // Remaining frames.
            int _rest = frames % FramesPerHour;
            // Number of frames divided by number of frames per minute.
            int _minutes = _rest / FramesPerMinute;
            // Remaining frames.
            _rest %= FramesPerMinute;
            // Number of frames divided by number of frames per second.
            int _seconds = _rest / FramesPerSecond;
            // Remaining frames.
            _rest %= FramesPerSecond;

            int _frames = _rest;

            return new int[] { _hours, _minutes, _seconds, _frames };
        }

        /// <summary>
        /// Creates tokens from total milliseconds.
        /// </summary>
        /// <param name="timeSpan">TimeSpan object.</param>
        /// <returns>Int[] timecode tokens.</returns>
        private int[] ConvertFromTimeSpan(TimeSpan timeSpan)
        {
            int[] tokens = new int[4];

            tokens[0] = timeSpan.Hours;
            tokens[1] = timeSpan.Minutes;
            tokens[2] = timeSpan.Seconds;
            tokens[3] = ConvertMillisecondsToFrames(timeSpan.Milliseconds);

            return tokens;
        }

        /// <summary>
        /// Converts millseconds to frames.
        /// </summary>
        /// <param name="milliseconds">Number of milliseconds.</param>
        /// <returns>Number of frames.</returns>
        public int ConvertMillisecondsToFrames(long milliseconds)
        {
            // Divide milliseconds by 30 frames, as decimal
            // to conserve precision.
            decimal frames = (milliseconds * FramesPerSecond) / 1000M;

            // Round it and conver to int for return.
            return Convert.ToInt32(Math.Round(frames));
        }

        /// <summary>
        /// Converts TimeSpan to frames.
        /// </summary>
        /// <param name="timeSpan">TimeSpan object.</param>
        /// <returns>Number of frames.</returns>
        private int ConvertMillisecondsToFrames(TimeSpan timeSpan)
        {
            // Get total number of milliseconds.
            long milliseconds = Convert.ToInt64(timeSpan.TotalMilliseconds);

            // Convert to frames.
            return ConvertMillisecondsToFrames(milliseconds);
        }

        /// <summary>
        /// Converts from milliseconds to frames, using FFME PositionStep
        /// property to set the number of milliseconds per frame.
        /// This property is, strangely enough, expressed as a TimeSpan.
        /// </summary>
        /// <param name="milliseconds">Number of milliseconds.</param>
        /// <param name="timeSpan">Duration of a frame.</param>
        /// <returns></returns>
        private int ConvertMillisecondsToFrames(long milliseconds, TimeSpan timeSpan)
        {
            // Get number of frames.
            decimal frames = milliseconds / (decimal)timeSpan.TotalMilliseconds;

            // Round it and return.
            return Convert.ToInt32(Math.Round(frames));
        }

        /// <summary>
        /// Converts smpte to equivalent number of frames.
        /// </summary>
        /// <returns></returns>
        public int ConvertToFrames()
        {
            return   TimecodeTokens[3] +
                    (TimecodeTokens[2] * FramesPerSecond) +
                    (TimecodeTokens[1] * FramesPerMinute) +
                    (TimecodeTokens[0] * FramesPerHour);
        }

        /// <summary>
        /// Converts the frame portion of smpte (TimecodeTokens[3]) to milliseconds.
        /// </summary>
        /// <param name="frameRate">Frame rate of video file.</param>
        /// <returns>Number of milliseconds.</returns>
        public int ConvertFramesToMilliseconds(double frameRate)
        {
            return Convert.ToInt32(Math.Round((TimecodeTokens[3] * 1000) / frameRate));
        }

        /// <summary>
        /// Normalizes smpte to comply with SMPTE timecode standard.
        /// </summary>
        /// <returns>
        /// Normalized Loop object. This method modifies the smpte itself.
        /// </returns>
        public SMPTE Normalize()
        {
            if (TimecodeTokens[3] > 29)
            {
                int additionalSeconds = TimecodeTokens[3] / 30;
                TimecodeTokens[3] %= 30;
                TimecodeTokens[2] += additionalSeconds;
            }
            if (TimecodeTokens[2] > 59)
            {
                int additionalMinutes = TimecodeTokens[2] / 60;
                TimecodeTokens[2] %= 60;
                TimecodeTokens[1] += additionalMinutes;
            }
            if (TimecodeTokens[1] > 59)
            {
                int additionalHours = TimecodeTokens[1] / 60;
                TimecodeTokens[1] %= 60;
                TimecodeTokens[0] += additionalHours;
            }

            UpdateTimecode();

            return this;
        }

        /// <summary>
        /// Generates drop frame timecode array.
        /// This should be applied to the initial offset smpte to generate
        /// the SMPTE timecodes smpteup table to use for display and to insert
        /// in the text editor.
        /// </summary>
        /// <param name="totalMinutes">Total duration of program in minutes</param>
        /// <param name="videoMode">Video engine selected.</param>
        /// <returns>Array of drop frame timecodes.</returns>
        public List<SMPTE> GenerateDropFrameTimecode(int totalMinutes, VideoMode videoMode)
        {

            // Create list of timecodes.
            List<SMPTE> timecodes = new List<SMPTE>();

            // Store original tokens.
            int hours   = TimecodeTokens[0];
            int minutes = TimecodeTokens[1];
            int seconds = TimecodeTokens[2];
            int frames  = TimecodeTokens[3];

            // Add initial timecode.
            timecodes.Add(new SMPTE(new int[] { hours, minutes, seconds, frames }));

            // For the duration set by user in minutes...
            for (int i = 0; i <= totalMinutes; i++)
            {
                // Do this for every second of every minute...
                for (int j = 0; j < 60; j++)
                {
                    // And for every frame of every second.
                    for (int k = 0; k < 30; k++)
                    {
                        // If we are in frame 29 of second 59 of minute other than a 9th minute...
                        if (((TimecodeTokens[1] % 10) != 9) && (TimecodeTokens[2] == 59) && (TimecodeTokens[3] == 29))
                        {
                            switch (videoMode)
                            {
                                case VideoMode.FFME:
                                    // Add 3 frames, since the next minute is not divisible by 10.
                                    TimecodeTokens[3] += 3;
                                    break;
                                case VideoMode.QuickTime:
                                    // Add 3 frames, since the next minute is not divisible by 10.
                                    // And add it to the list 2 times (plus one in the normal smpte)
                                    // since we are calculating time based frames, and the simpler
                                    // method above works with frame information provided by FFME.
                                    // If we simply drop two frames, we are gonne be progressively
                                    // behind in timecode.
                                    TimecodeTokens[3] += 3;

                                    // Normalize it.
                                    Normalize();

                                    // Add it twice.
                                    timecodes.Add(new SMPTE(new int[]
                                    {
                                        TimecodeTokens[0],
                                        TimecodeTokens[1],
                                        TimecodeTokens[2],
                                        TimecodeTokens[3]
                                    }));
                                    timecodes.Add(new SMPTE(new int[]
                                    {
                                        TimecodeTokens[0],
                                        TimecodeTokens[1],
                                        TimecodeTokens[2],
                                        TimecodeTokens[3]
                                    }));
                                    break;
                            }
                        }
                        else
                        {
                            // Otherwise, add your run of the mill one frame.
                            TimecodeTokens[3] += 1;
                        }
                        // Normalize timecode.
                        Normalize();
                        // And add it to the list.
                        timecodes.Add(new SMPTE(new int[]
                        {
                            TimecodeTokens[0],
                            TimecodeTokens[1],
                            TimecodeTokens[2],
                            TimecodeTokens[3]
                        }));
                    }
                }
            }

            // Restore tokens.
            TimecodeTokens[0] = hours;
            TimecodeTokens[1] = minutes;
            TimecodeTokens[2] = seconds;
            TimecodeTokens[3] = frames;

            return timecodes;
        }

        /// <summary>
        /// Validates user input as a SMPTE string
        /// </summary>
        /// <param name="input">User input.</param>
        /// <returns>Valid SMPTE string.</returns>
        public static string ValidateSmpte(string input)
        {

            // If no string was passed, assume 0 time.
            if (string.IsNullOrEmpty(input)) return "00:00:00:00";

            // Maximum number of timecode significant digits.
            int maxChars = 8;
            // Total length of timecode string, includying colon chars.
            int lengthOfTimeCodeString = 11;
            // Valid string builder.
            StringBuilder validString = new StringBuilder();
            // Output builder.
            StringBuilder output = new StringBuilder();
            // Check for digits only.
            foreach (char c in input)
            {
                if (Char.IsDigit(c)) { validString.Append(c); }
            }

            // Pad to 8 digits for time code.
            while (validString.Length < maxChars)
            {
                validString.Append("0");
            }

            // Check if string is longer then 8 digits.
            if (validString.Length > maxChars)
                // Remove extra chars. -1 because is 0 based. 
                validString.Remove((maxChars - 1), (validString.Length - maxChars));

            // For counting the actual 8 digits.
            int y = 0;

            // Iterate through string.
            for (int i = 0; i < lengthOfTimeCodeString; i++)
            {
                // Every 2 chars...
                if ((i + 1) % 3 == 0)
                    // Place colon.
                    output.Append(":");
                else
                    output.Append(validString[y++]);
            }

            // Temporal variable to avoid converting the StringBuilder to string several times.
            string timeString = output.ToString();
            // Get minutes token.
            string minutes = timeString.Substring(3, 2);
            // Parse to int.
            int iMinutes = Int32.Parse(minutes);

            // Check if minutes is greater than 59.
            if (iMinutes > 59)
            {
                // Remove the token...
                output.Remove(3, 2);
                // And insert 59.
                output.Insert(3, "59");
            }

            // Get seconds token.
            string seconds = timeString.Substring(6, 2);
            // Parse to int.
            int iSeconds = Int32.Parse(seconds);

            // Check if seconds is greater than 59.
            if (iSeconds > 59)
            {
                // Remove token...
                output.Remove(6, 2);
                // And insert 59.
                output.Insert(6, "59");
            }

            // Get frames token.
            string frames = timeString.Substring(9, 2);
            // Parse to int.
            int iFrames = Int32.Parse(frames);

            //Check if frames is higher than 29.
            if (iFrames > 29)
            {
                // Remove token...
                output.Remove(9, 2);
                // And insert 29.
                output.Insert(9, "29");
            }
            
            return output.ToString();
        }

        /// <summary>
        /// Offset all program time loops.
        /// </summary>
        /// <param name="correctLoop">Loop to change to.</param>
        /// <param name="document">Flow document.</param>
        /// <param name="searchPattern">Pattern to search for.</param>
        public static Dictionary<string,string> OffsetLoops(FlowDocument document, SMPTE correctLoop, string searchPattern)
        {
            Regex regex = new Regex(searchPattern, RegexOptions.IgnoreCase);
            // Normalize.
            correctLoop.Normalize();
            // The starting point, as it should be.
            int correctFrames = correctLoop.ConvertToFrames();
            // Frame difference between the two.
            int offset = 0;
            // Frames of each loop.
            int thisFrames = 0;
            // New SMPTE
            SMPTE newSMPTE;
            // Create working loop.
            SMPTE workingSMPTE;
            // First match.
            bool isFirstMatch = true;
            // CREATE DICTIONARY TO STORE THE VALUES TO CHANGE AND THE VALUES TO CHANGE THEM TO.
            // THIS IS NECCESSARY BECAUSE A COLLECTION CANNOT BE CHANGED IN A LOOP.
            Dictionary<string, string> result = new Dictionary<string, string>();
            // ITERATE.
            foreach (Block block in document.Blocks)
            {
                // Convert block to paragraph.
                Paragraph paragraph = block as Paragraph;
                // If it indeed was a paragraph...
                if (paragraph != null)
                {
                    // Get paragraph content.
                    string paragraphText = new TextRange(paragraph.Inlines.FirstInline.ContentStart, paragraph.Inlines.FirstInline.ContentEnd).Text;
                    // If it is not empty...
                    if (!String.IsNullOrEmpty(paragraphText) && !String.IsNullOrWhiteSpace(paragraphText))
                    {
                        // Search for loop.
                        Match match = Regex.Match(paragraphText, searchPattern);
                        // If found...
                        if (match.Success)
                        {
                            // Repeat until no more loops exist.
                            while (match.Success)
                            {
                                // Create SMPTE.
                                workingSMPTE = new SMPTE(match.Value);
                                // Get frames.
                                thisFrames = workingSMPTE.ConvertToFrames();
                                // Is it the first loop?
                                if (isFirstMatch)
                                {
                                    // Calculate offset.
                                    offset = correctFrames - thisFrames;
                                    // Not first match anymore.
                                    isFirstMatch = false;
                                }
                                // Get frames.
                                newSMPTE = new SMPTE(thisFrames + offset);
                                // ADD STRING FOUND TO DICTIONARY.
                                result.Add(match.Value, newSMPTE.ToString());
                                // Next match.
                                match = regex.Match(paragraphText, match.Index + 1);
                            }
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Gets total time of program.
        /// </summary>
        /// <param name="inTimecode">First timecode.</param>
        /// <param name="outTimcode">Last timecode</param>
        /// <returns>Number of minutes</returns>
        public static int GetProgramTime(string inTimecode, string outTimcode)
        {
            int startSeconds = ConvertSMPTEtoSeconds(inTimecode);
            int endSeconds   = ConvertSMPTEtoSeconds(outTimcode);
            int totalSeconds = endSeconds - startSeconds;

            return (totalSeconds / 60) + (((totalSeconds % 60) > 0) ? 1 : 0);

        }

        /// <summary>
        /// Converts from SMPTE to seconds.
        /// </summary>
        /// <param name="SMPTE">SMPTE timcode</param>
        public static int ConvertSMPTEtoSeconds(string SMPTE)
        {
            // Array to hold de splitted times.
            string[] times;
            // Split the string at...
            string[] separator = { ":" };
            // Load the array.
            times = SMPTE.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            // Return value
            int seconds;

            seconds =  Int32.Parse(times[0]) * FramesPerHour;
            seconds += Int32.Parse(times[1]) * FramesPerMinute;
            seconds += Int32.Parse(times[2]) * FramesPerSecond;
            seconds += Int32.Parse(times[3]);

            seconds /= 30;

            return seconds;

        }

        /// <summary>
        /// Updates the timecode string after any modification to the timecode tokens.
        /// </summary>
        private void UpdateTimecode()
        {
            Timecode = $"{TimecodeTokens[0]:D2}:{TimecodeTokens[1]:D2}:{TimecodeTokens[2]:D2}:{TimecodeTokens[3]:D2}";
        }

        #endregion
    }
}
