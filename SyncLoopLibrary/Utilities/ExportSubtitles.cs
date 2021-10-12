using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace SyncLoopLibrary
{
    public static partial class Utilities
    {
        /// <summary>
        /// Export subtitles in SRT format.
        /// </summary>
        /// <param name="loops">List of loop objects.</param>
        /// <param name="frameRate">Video frame rate. Default> 29.97 fps.</param>
        /// <returns>Subtitle string in SRT format.</returns>
        public static SRT ExportSubtitlesSRT(List<Loop> loops, double frameRate = 29.97)
        {

            // Create drop frame SMPTE, to compensate for error in timing.
            SMPTE drops = new SMPTE(0);

            // Generate drop frames.
            // We'll use 120 minutos to cover our basis, but it would be nice to actually have
            // the real video length here.
            List<SMPTE> dropFrames = drops.GenerateDropFrameTimecode(120, VideoMode.FFME);

            // Result object for SRT file.
            StringBuilder subtitles = new StringBuilder();

            // For loops.
            int counter = 1;

            // Result object for Jump List.
            Queue<JumpPoint> list = new Queue<JumpPoint>();

            // Distance between loops in frames to create jump, in frames.
            long distance = 150;

            // Offset before the actual jump point.
            long offset = 1000;
            // Difference.
            long difference = 0;
            // Declare holders for last frame.
            long lastOutFrame = 0;

            // So we iterate through the loop list.
            foreach (Loop loop in loops)
            {

                // Check for subtitle loop.
                if (loop.Mode == DocumentMode.Subtitles)
                {
                    // Add number.
                    subtitles.Append(counter++);
                    subtitles.Append(Environment.NewLine);

                    // Create SRT timecodes.
                    SMPTE inSMPTE = dropFrames[(int)loop.InFrame];
                    SMPTE outSMPTE = dropFrames[(int)loop.OutFrame];

                    string inString = $"{inSMPTE.TimecodeTokens[0]:D2}:{inSMPTE.TimecodeTokens[1]:D2}:{inSMPTE.TimecodeTokens[2]:D2},{inSMPTE.ConvertFramesToMilliseconds(29.97):D3}";
                    string outString = $"{outSMPTE.TimecodeTokens[0]:D2}:{outSMPTE.TimecodeTokens[1]:D2}:{outSMPTE.TimecodeTokens[2]:D2},{outSMPTE.ConvertFramesToMilliseconds(29.97):D3}";

                    subtitles.AppendLine(String.Format(@"{0} --> {1}", inString, outString));

                    // We construct the subtitles.
                    for (int i = 0; i < loop.Subtitles.Length; i++)
                    {
                        if (!String.IsNullOrEmpty(loop.Subtitles[i]))
                        {
                            subtitles.AppendLine(loop.Subtitles[i]);
                        }
                    }

                    // Blank line.
                    subtitles.Append(Environment.NewLine);


                    // Jump list.

                    // Milliseconds to jump to.
                    double milliseconds = ((loop.InFrame * 1000) / frameRate) - offset;

                    // Now we check it against the last one.
                    difference = loop.InFrame - lastOutFrame;

                    if (difference >= distance)
                    {

                        JumpPoint jump = new JumpPoint()
                        {
                            Frame = lastOutFrame + 1,
                            Span = TimeSpan.FromMilliseconds(milliseconds)
                        };

                        list.Enqueue(jump);
                    }

                    lastOutFrame = loop.OutFrame;

                }
            }

            SRT result = new SRT
            {
                Subtitles = subtitles.ToString(),
                JumpList = list
            };

            return result;
        }


        /// <summary>
        /// Export subtitles in .sub format.
        /// This method returns a byte array, needed to generate an ANSI file.
        /// This file must be saved directly, without any encoding, to preserve the integrity and avoid the inclusion
        /// of Unicode aditional characters in the final output.
        /// </summary>
        /// <param name="loops">List of loop objects.</param>
        /// <returns>Raw byte array that shoud be saved directly to a text file.</returns>
        public static byte[] ExportSubtitlesSUB(List<Loop> loops)
        {
            // Create result constructor.
            StringBuilder result = new StringBuilder();
            // This flag indicates the lines in italics.
            // 0 = None, 1 = First, 2 = Second, 3 = both.
            int linesInItalics = 0;
            // This variable will contain the line number
            // coded as a variable number of "{".
            string height = String.Empty;
            // This variable will hold the last timecode.
            string lastTimecode = null;
            // This variable will hold the first subtitle loop.
            Loop firstLoop = new Loop();

            // Let's add the prolog, which is always the same.
            result.AppendLine("*PART 1*");
            // Iterate to find it.
            foreach (Loop loop in loops)
            {
                if (loop.Mode == DocumentMode.Subtitles)
                {
                    firstLoop = loop;
                    break;
                }
            }
            // Now we need to check the first subtitle to see
            // if it begins before or after the one hour mark,
            // to set the opening subtitle string of this format.
            // We get the first two characters of the timecode,
            // which indicate the hour.
            string beginning = firstLoop.InTimecode.Substring(0, 2);
            // We check if the first two characters are greater than one.
            // God only knows what this first timecode is for...
            if (int.TryParse(beginning, out int hour))
            {
                if (hour >= 1)
                {
                    result.AppendLine(@"01:00:00.00\01:00:00.00");
                }
                else
                {
                    result.AppendLine(@"00:00:00.00\00:00:00.00");
                }
            }
            else
            {
                // We inform the user...
                MessageBox.Show("Error reading the first subtitle", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                // ...and return null.
                // Must remember to check for null return value
                // in the calling function!
                return null;
            }
            // The prolog should be ready now.
            // It's time to generate the actual subtitles.
            // So we iterate through the loop list.
            foreach (Loop loop in loops)
            {
                // Check for subtitle loop.
                if (loop.Mode == DocumentMode.Subtitles)
                {
                    // First thing is check the full timecode string
                    // to extract information about the positioning
                    // and formatting of the text.
                    // This format seems to ignore left and right alignment.
                    // Let's check for the line number.
                    // It is the first character of the string, after the "SUB[" definition.
                    if (int.TryParse(loop.Timecode.Substring(4, 1), out int line))
                    {
                        // Now he have an int that tells us the line number.
                        // It should be between 0 and 9,
                        // so let's make sure.
                        if (line < 0 || line > 9)
                        {
                            // We inform the user...
                            MessageBox.Show("Error in subtitle format", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                            // ...and return null.
                            // Must remember to check for null return value
                            // in the calling function!
                            return null;
                        }
                        // We know we have a valid line number,
                        // so let's add the corrsponding number of characters.
                        // The character to raise the line is "{" up to line 8.
                        // Line 9 is the closing curly bracket "}".
                        // If it is 9, deal with the special case.
                        if (line == 9)
                        {
                            height = "}";
                        }
                        // Otherwise, add as many as needed
                        // if line is not 0.
                        else if (line > 0)
                        {
                            for (int i = 1; i <= line; i++)
                            {
                                // In the file produced by Transtation,
                                // only the first six lines are honored,
                                // so we check for that.
                                if (line < 7) height += "{";
                            }
                        }
                        else
                        {
                            height = String.Empty;
                        }
                    }
                    else
                    {
                        // We inform the user...
                        MessageBox.Show("Error in subtitle format", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        // ...and return null.
                        // Must remember to check for null return value
                        // in the calling function!
                        return null;
                    }
                    // At this point we should have the line information.
                    // Let's get the text formatting info.
                    // This is just about the text being in italics or not,
                    // and if both lines are or just one.
                    // So we get the info character, the 6th. (The 5th is used for alignment).
                    // This character can be a char or can be a int.
                    // Let's check for the "I" char first,
                    // which means both lines are italics.
                    if (loop.Timecode.Substring(6, 1) == "I")
                    {
                        linesInItalics = 3;
                    }
                    // If not, check for an integer indicating which of the two
                    // lines is in italics.
                    else if (int.TryParse(loop.Timecode.Substring(6, 1), out int italics))
                    {
                        // Let's check for range.
                        // Should be only 1 or 2.
                        if (italics != 1 && italics != 2)
                        {
                            // We inform the user...
                            MessageBox.Show("Error in subtitle format", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                            // ...and return null.
                            // Must remember to check for null return value
                            // in the calling function!
                            return null;
                        }
                        else
                        {
                            linesInItalics = italics;
                        }
                    }
                    // If we are here, there are no italics.
                    else
                    {
                        linesInItalics = 0;
                    }

                    // Now we have all the information needed to create the actual subtitle.
                    // We construct the subtitles.
                    for (int i = 0; i < loop.Subtitles.Length; i++)
                    {
                        if (!String.IsNullOrEmpty(loop.Subtitles[i]))
                        {
                            // We only have to add line number info
                            // to the first line of subtitles.
                            if (i == 0) result.Append(height);
                            // Set italics.
                            // If variable is 3, then both lines are italics.
                            // Otherwise, we add one to the index, since it is zero based
                            // and the italics varaible in one based.
                            if (linesInItalics == 3 || linesInItalics == i + 1)
                            {
                                result.Append("[");
                            }
                            // The prefix is done.
                            // Let's add the text.
                            // First, check maximum lenght.
                            if (loop.Subtitles[i].Length > Settings.ApplicationSettings.SubtitleLength)
                            {
                                // We inform the user...
                                MessageBox.Show($"Subtitle exceeded maximum length in: {loop.Subtitles[i]}", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                                // ...and return null.
                                // Must remember to check for null return value
                                // in the calling function!
                                return null;
                            }
                            else
                            {
                                if (Settings.ApplicationSettings.EncodeCharacters)
                                {
                                    result.AppendLine(EncodeCharacters(loop.Subtitles[i]));
                                }
                                else
                                {
                                    result.AppendLine(loop.Subtitles[i]);
                                }
                            }
                        }
                    }

                    loop.ToSubFormat();
                    // The timecode line is simple, since it does not contain
                    // any formatting info.
                    result.AppendLine(String.Format(@"{0}\{1}", loop.InTimecodeSUB, loop.OutTimecodeSUB));

                    // Set the last timecode.
                    // This, of course, will be overriden
                    // at each iteration.
                    lastTimecode = loop.OutTimecodeSUB;
                }
            }

            // All subtitles have been created.
            // Let's generate the mysterious file epilog.
            // First, the end flag.
            result.AppendLine("*END*");

            // Next, another mysterious timecode string,
            // consisting of the last timecode repited.
            // NOTE: IN THE EXAMPLE FILE, THESE TIMECODES ARE ONE FRAME
            // AFTER THE LAST.
            result.AppendLine(String.Format(@"{0}\{0}", lastTimecode));

            // Now the really mysterious things...

            #region Short version.

            result.AppendLine("*CODE*");
            result.AppendLine("0000000000000000");
            result.AppendLine("*CAST*");
            result.AppendLine(String.Empty);
            result.AppendLine(String.Empty);
            result.AppendLine("*GENERATOR*");
            result.AppendLine("*FONTS*");
            result.AppendLine("*READ*");
            result.AppendLine("0,300 14,000 130,000 100,000 25,000");
            result.AppendLine("*TIMING*");
            result.AppendLine("1 30 0");
            result.AppendLine("*TIMED BACKUP NAME*");
            result.AppendLine(@"C:\");
            result.AppendLine(String.Empty);
            result.AppendLine("*READ ADVANCED*");
            result.AppendLine("< > 1 1 1,000");
            result.AppendLine(String.Empty);
            result.AppendLine(String.Empty);
            result.AppendLine("*MARKERS*");

            #endregion

            #region Long version.

            //result.AppendLine("*CODE*");
            //result.AppendLine("");
            //result.AppendLine("*CAST*");
            //result.AppendLine("StyleName=");
            //result.AppendLine("Enabled=0");
            //result.AppendLine("*GENERATOR*");
            //result.AppendLine("");
            //result.AppendLine("*FONTS*");
            //result.AppendLine("*READ*");
            //result.AppendLine("0.300 14.000 130.000 100.000 25.000");
            //result.AppendLine("*TIMING*");
            //result.AppendLine("1 25 0 1 1");
            //result.AppendLine("*TIMED BACKUP NAME*");
            //result.AppendLine(@"C:\");
            //result.AppendLine(@"*FORMAT SAMPLE †‚Þåäê–˜*");
            //result.AppendLine(@"*READ ADVANCED*");
            //result.AppendLine(@"< > 1 1 1.000");
            //result.AppendLine(@"*METRICS*");
            //result.AppendLine(@"1 32 0 600 0 540 1 2 0 0 0");
            //result.AppendLine(@"*MARKERS*");
            //result.AppendLine(@"*PREVIEW FONT*");
            //result.AppendLine(@"Arial B 44 0:44 0");
            //result.AppendLine(@"0 3 18 14 8");
            //result.AppendLine(@"720 486 0 0 0 0");
            //result.AppendLine(@"*STYLE SETTINGS*");
            //result.AppendLine(@"<GenParam");
            //result.AppendLine(@"Center=ctLeft");
            //result.AppendLine(@"BaseLy=18");
            //result.AppendLine(@"SingLy=14");
            //result.AppendLine(@"LineH=8");
            //result.AppendLine(@"LineV=8");
            //result.AppendLine(@"XLeft=10");
            //result.AppendLine(@"XCent=50");
            //result.AppendLine(@"XRight=95");
            //result.AppendLine(@"YTop=88.0699996948242");
            //result.AppendLine(@"YBottom=13");
            //result.AppendLine(@"txtColor=0");
            //result.AppendLine(@"Border=5");
            //result.AppendLine(@"bkColor=7");
            //result.AppendLine(@"scColor=10");
            //result.AppendLine(@"DefaultFont=0");
            //result.AppendLine(@"DvdColor=0");
            //result.AppendLine(@"GLColor=0");
            //result.AppendLine(@"AAStyle=aa4x");
            //result.AppendLine(@"AAColor=11");
            //result.AppendLine(@"BoxColor=7");
            //result.AppendLine(@"BoxTrans=10");
            //result.AppendLine(@"Box=False");
            //result.AppendLine(@"StripeColor=7");
            //result.AppendLine(@"Stripe=False");
            //result.AppendLine(@"StripeTop=75");
            //result.AppendLine(@"StripeSize=20");
            //result.AppendLine("Line=\"\"");
            //result.AppendLine(@"RubySeparation=1");
            //result.AppendLine(@"BoutenSeparation=1");
            //result.AppendLine(@"PersistentPos=True");
            //result.AppendLine(@"AlignAfterQuotation=False");
            //result.AppendLine(@"WordKerning=0");
            //result.AppendLine(@"CharKerning=0");
            //result.AppendLine(@"ShadowStyle=0");
            //result.AppendLine(@"ShadowWidth=0");
            //result.AppendLine(@"ShadowDir=0");
            //result.AppendLine(@"ImageFormat=NTSC");
            //result.AppendLine("StylePassword=\"\"");
            //result.AppendLine(@"ActivePassword=False");
            //result.AppendLine(@">");
            //result.AppendLine(@"<Colors");
            //result.AppendLine(@"Values=#FFFFFF;#00FFFF;#FF00FF;#0000FF;#FFFF00;#00FC00;#FF0000;#000000;#0056E3;#9D7800;#880009;#969696;");
            //result.AppendLine(@"/>");
            //result.AppendLine(@"<Fonts");
            //result.AppendLine(@"BorderSize=5");
            //result.AppendLine(@"ShadowWidth=0");
            //result.AppendLine(@"ShadowStyle=0");
            //result.AppendLine(@"ShadowDir=0");
            //result.AppendLine(@"CharKerning=0");
            //result.AppendLine(@"WordKerning=0");
            //result.AppendLine(@"FontPath=.\DnLoad");
            //result.AppendLine(@">");
            //result.AppendLine(@"<Arial_B_44_0_44_0");
            //result.AppendLine(@"Name=Arial&#x20;B&#x20;44&#x20;0:44&#x20;0");
            //result.AppendLine(@"/>");
            //result.AppendLine(@"<Comic_Sans_MS_40");
            //result.AppendLine(@"Name=Comic&#x20;Sans&#x20;MS&#x20;40");
            //result.AppendLine(@"/>");
            //result.AppendLine(@"<Tahoma_35");
            //result.AppendLine(@"Name=Tahoma&#x20;35");
            //result.AppendLine(@"/>");
            //result.AppendLine(@"<MS_Gothic_30");
            //result.AppendLine(@"Name=MS&#x20;Gothic&#x20;30");
            //result.AppendLine(@"/>");
            //result.AppendLine(@"</Fonts>");
            //result.AppendLine(@"</GenParam>");
            //result.AppendLine(@"*IMAGE FORMAT*");
            //result.AppendLine(@"<BaseImageFormat");
            //result.AppendLine(@"Name=NTSC");
            //result.AppendLine(@"TopDown=True");
            //result.AppendLine(@"ImgWidth=720");
            //result.AppendLine(@"ImgHeight=486");
            //result.AppendLine(@"ImgHOfs=0");
            //result.AppendLine(@"ImgHExt=0");
            //result.AppendLine(@"ImgVOfs=0");
            //result.AppendLine(@"ImgVExt=0");
            //result.AppendLine(@"/>");

            #endregion

            // get the correct encodings 
            var srcEncoding = Encoding.UTF8; // utf-8
            var destEncoding = Encoding.GetEncoding(1252); // windows-1252

            string test = result.ToString();
            // convert the source bytes to the destination bytes
            var destBytes = Encoding.Convert(srcEncoding, destEncoding, srcEncoding.GetBytes(test));

            return destBytes;
        }


        /// <summary>
        /// Encodes character to sub coding.
        /// This is done manually since I could not find an encoder to do this automatically.
        /// </summary>
        /// <param name="original">Original string.</param>
        /// <returns>Encoded string</returns>
        private static string EncodeCharacters(string original)
        {
            // Create a dictionary to store the keys and values.
            // The keys are gonna be the original accented character
            // and the value the replacement character.
            Dictionary<char, char> table = new Dictionary<char, char>();

            table.Add('¿', (char)168);
            table.Add('¡', (char)173);

            table.Add('á', (char)160);
            table.Add('é', (char)8218);
            table.Add('í', (char)161);
            table.Add('ó', (char)162);
            table.Add('ú', (char)163);

            table.Add('Á', (char)181);
            table.Add('É', (char)144);
            table.Add('Í', (char)214);
            table.Add('Ó', (char)224);
            table.Add('Ú', (char)233);

            table.Add('ñ', (char)164);
            table.Add('Ñ', (char)165);

            table.Add('ü', (char)129);
            table.Add('Ü', (char)353);

            table.Add('ö', (char)148);
            table.Add('Ö', (char)153);

            table.Add('ä', (char)132);
            table.Add('Ä', (char)142);

            table.Add('ª', (char)166);
            table.Add('º', (char)167);

            table.Add('ç', (char)135);
            table.Add('Ç', (char)128);

            table.Add('²', (char)253);

            table.Add('´', (char)239);
            table.Add('“', (char)34);
            table.Add('”', (char)34);
            table.Add('…', (char)46);
            table.Add('°', (char)248);
            table.Add('’', (char)39);

            // Create a variable to store the converted string.
            StringBuilder result = new StringBuilder(original);
            // Replace the characters using regex.
            for (int i = 0; i < result.Length; i++)
            {
                if (table.ContainsKey(result[i])) result[i] = table[result[i]];
            }

            return result.ToString();
        }

    }
}
