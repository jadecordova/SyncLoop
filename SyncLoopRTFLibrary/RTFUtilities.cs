using SyncLoopLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace SyncLoopRTFLibrary
{
    public static class RTFUtilities
    {
        #region --------------------------------------------------------------------------------< MEMBERS >

        private static List<Color> characterColors = new List<Color>
        {
                    Colors.Brown,
                    Colors.DarkGreen,
                    Colors.DarkSlateGray,
                    Colors.DarkSlateBlue,
                    Colors.SaddleBrown,
                    Colors.DarkOliveGreen,
                    Colors.Indigo,
                    Colors.OrangeRed,
                    Colors.Olive,
                    Colors.RoyalBlue,
                    Colors.Red,
                    Colors.Teal,
                    Colors.DimGray,
                    Colors.SlateBlue,
                    Colors.SteelBlue,
                    Colors.Crimson,
                    Colors.DarkBlue
        };

        #endregion

        #region --------------------------------------------------------------------------------< PROPERTIES >

        /// <summary>
        /// Value of twips per inch.
        /// </summary>
        public static int TwipsPerInch { get; set; } = 1440;

        /// <summary>
        /// RTF code empty line token.
        /// </summary>
        public static string EmptyLine { get; set; } = @"\line ";

        /// <summary>
        /// List of character colors.
        /// </summary>
        public static List<Color> CharacterColors
        {
            get { return characterColors; }
            set { characterColors = value; }
        }

        #endregion

        #region --------------------------------------------------------------------------------< METHODS >

        /// <summary>
        /// Generates RTF document title page based on info
        /// gathered by the program info dialog.
        /// </summary>
        public static List<RTFParagraph> GenerateTitlePage(ProgramInfo programInfo)
        {
            List<RTFParagraph> result = new List<RTFParagraph>();

            // Blank paragraph.
            result.Add(new RTFParagraph(String.Empty, new RTFParagraphOptions()
            {
                FontSize = 24,
                IsBold = true,
                Justification = RTFJustification.Center
            }));

            // Series name English.
            result.Add(new RTFParagraph(programInfo.EpisodeSeries.NameEnglish.ToUpper(), new RTFParagraphOptions()
            {
                FontSize = 24,
                IsBold = true,
                Justification = RTFJustification.Center
            }));

            // Episode name English
            result.Add(new RTFParagraph(programInfo.EpisodeNameEnglish.ToUpper(), new RTFParagraphOptions()
            {
                FontSize = 24,
                IsBold = true,
                Justification = RTFJustification.Center
            }));

            // Blank paragraph.
            result.Add(new RTFParagraph(String.Empty, new RTFParagraphOptions()
            {
                FontSize = 24,
                IsBold = true,
                Justification = RTFJustification.Center
            }));

            // Series name Spanish.
            result.Add(new RTFParagraph(programInfo.EpisodeSeries.NameSpanish.ToUpper(), new RTFParagraphOptions()
            {
                FontSize = 24,
                IsBold = true,
                Justification = RTFJustification.Center
            }));

            // Episode name Spanish
            result.Add(new RTFParagraph(programInfo.EpisodeNameSpanish.ToUpper(), new RTFParagraphOptions()
            {
                FontSize = 24,
                IsBold = true,
                Justification = RTFJustification.Center
            }));

            // Blank paragraph.
            result.Add(new RTFParagraph(String.Empty, new RTFParagraphOptions()
            {
                FontSize = 24,
                IsBold = true,
                Justification = RTFJustification.Center
            }));

            // Episode number.
            result.Add(new RTFParagraph(programInfo.EpisodeNumber, new RTFParagraphOptions()
            {
                FontSize = 24,
                IsBold = true,
                Justification = RTFJustification.Center
            }));

            // Episode code.
            result.Add(new RTFParagraph(programInfo.EpisodeCode, new RTFParagraphOptions()
            {
                FontSize = 24,
                IsBold = true,
                Justification = RTFJustification.Center
            }));

            return result;

        }

        /// <summary>
        /// Generates RTF file for proof reading or final delivery.
        /// </summary>
        /// <param name="isFinalDocument">Flag for type of document.</param>
        public static string GenerateRTFDocument(List<Loop> loops,
                                                 ObservableCollection<Character> characters,
                                                 Settings settings,
                                                 ProgramInfo programInfo,
                                                 bool isFinalDocument = false)
        {
            // List of document content as RTFParagraphs.
            List<RTFParagraph> documentRTFParagraphs = new List<RTFParagraph>();

            // Flag to know if this is the first content paragraph, to set page break.
            bool IS_FIRST_LOOP = true;
            
            // List of characters color.s
            List<Color> documentColors = new List<Color>();
            
            // Color for character id.
            Color activeColor = System.Windows.Media.Colors.Black;
            
            // Index of active color in colors list.
            int currentColorIndex = 0;

            // Current character.
            Character character = null;

            foreach (Loop loop in loops)
            {
                // If this is not a subtitle loop, get character.
                if(loop.Mode != DocumentMode.Subtitles)
                {
                    character = characters.Where(i => i.Name.ToUpper() == loop.CharacterName.Trim().ToUpper()).FirstOrDefault();
                }
                // If it is, reset character to null.
                else
                {
                    character = null;
                }

                // Set the current character color.
                if (character != null)
                {
                    if (isFinalDocument)
                    {
                        activeColor = Colors.Black;
                    }
                    else
                    {
                        activeColor = character.CharacterColor;
                    }
                }
                else
                {
                    // Default to black.
                    activeColor = Colors.Black;
                }

                // Add color to list.
                if (!documentColors.Contains(activeColor))
                {
                    documentColors.Add(activeColor);
                }

                // Get index of color in list.
                currentColorIndex = documentColors.IndexOf(activeColor);

                // If this is the first loop,
                // we must add a page break before.
                if (IS_FIRST_LOOP)
                {
                    // Add paragraph with line break.
                    if(loop.Mode != DocumentMode.Subtitles)
                    {
                        documentRTFParagraphs.Add(new RTFParagraph(loop.Timecode, loop.CharacterName,
                                new RTFParagraphOptions()
                                {
                                    DoNotSpellCheck = true,
                                    PageBreakBefore = true,
                                    ParagraphColor = currentColorIndex
                                })
                        {
                            IsLoop = true
                        });
                    }
                    else
                    {
                        documentRTFParagraphs.Add(new RTFParagraph(loop.Timecode,
                                new RTFParagraphOptions()
                                {
                                    DoNotSpellCheck = true,
                                    PageBreakBefore = true,
                                    ParagraphColor = currentColorIndex
                                })
                        {
                            IsLoop = true
                        });
                    }

                    // Set the flag.
                    IS_FIRST_LOOP = false;
                }
                else
                {
                    // Add blank paragraph.
                    documentRTFParagraphs.Add(new RTFParagraph(String.Empty, new RTFParagraphOptions()));

                    // Add actual paragraph.
                    if (loop.Mode != DocumentMode.Subtitles)
                    {
                        documentRTFParagraphs.Add(new RTFParagraph(loop.Timecode, loop.CharacterName,
                                new RTFParagraphOptions()
                                {
                                    DoNotSpellCheck = true,
                                    ParagraphColor = currentColorIndex
                                })
                        {
                            IsLoop = true
                        });
                    }
                    else
                    {
                        documentRTFParagraphs.Add(new RTFParagraph(loop.Timecode,
                                new RTFParagraphOptions()
                                {
                                    DoNotSpellCheck = true,
                                    ParagraphColor = currentColorIndex
                                })
                        {
                            IsLoop = true
                        });
                    }
                }

                // If it is a title, set the proper justification.
                RTFJustification justification = RTFJustification.Justified;

                if (loop.Mode != DocumentMode.Subtitles && loop.CharacterName.ToUpper() == Settings.ApplicationSettings.TitleString.ToUpper())
                {
                    justification = RTFJustification.Left;
                }

                // Add dialog paragraph.
                if(loop.Mode != DocumentMode.Subtitles)
                {
                    documentRTFParagraphs.Add(new RTFParagraph(loop.CharacterDialog, new RTFParagraphOptions()
                    {
                        ParagraphColor = currentColorIndex,
                        Justification = justification
                    }));
                }
                else
                {
                    documentRTFParagraphs.Add(new RTFParagraph(loop.Subtitles[0], new RTFParagraphOptions()
                    {
                        ParagraphColor = currentColorIndex,
                        Justification = justification
                    }));

                    // Check if there is a second line.
                    if (!String.IsNullOrEmpty(loop.Subtitles[1]))
                    {
                        documentRTFParagraphs.Add(new RTFParagraph(loop.Subtitles[1], new RTFParagraphOptions()
                        {
                            ParagraphColor = currentColorIndex,
                            Justification = justification
                        }));
                    }
                }
            }

            // Create color table.
            List<RTFColor> rtfColors = new List<RTFColor>();

            // Convert colors list to RTFColor list.
            // Add black and white.
            foreach (Color c in documentColors)
            {
                rtfColors.Add(new RTFColor(c.R, c.G, c.B));
            }

            // Create RTF color table.
            RTFColorTable colors = new RTFColorTable(rtfColors);
           
            // Create fonts.
            string[] fonts = new string[]
            {
                        settings.ContentFont,
                        settings.AdditionalFont,
                        settings.HeadersAndFootersFont
            };

            // Create final paragraphs list.
            List<RTFParagraph> finalDocumentContent = new List<RTFParagraph>();

            if (isFinalDocument)
            {
                // Add title page.
                finalDocumentContent.AddRange(RTFUtilities.GenerateTitlePage(programInfo));

                // Add characters and lines.
                finalDocumentContent.AddRange(RTFUtilities.CountLines(settings, characters));
            }

            // Add content.
            finalDocumentContent.AddRange(documentRTFParagraphs);
            
            // Create RTF document.
            RTFDocument rtf = new RTFDocument(fonts, finalDocumentContent, colors);
            
            // Set left header.
            rtf.Properties.DocumentTitleEnglish = $"{programInfo.EpisodeSeries.NameEnglish} - {programInfo.EpisodeNumber}";
            
            // Set right header.
            rtf.Properties.DocumentTitleSpanish = $"{programInfo.EpisodeSeries.NameSpanish} - {programInfo.EpisodeNumber}";
            
            // Set company name.
            rtf.Properties.Company = settings.TranslatorName;
            
            // Set author.
            rtf.Properties.DocumentAuthor = settings.TranslatorName;
            
            // Set comment.
            rtf.Properties.Comment = $"This episode of {programInfo.EpisodeSeries.NameEnglish} was translated by {settings.TranslatorName}.";
            
            // Generate file name.
            string filename = String.Empty;
            
            // Select type.
            if (isFinalDocument)
            {
                filename = Path.Combine(Settings.ApplicationSettings.Project.ProjectFolder, Settings.ApplicationSettings.Project.DocumentName + ".rtf");
            }
            else
            {
                filename = Path.Combine(Settings.ApplicationSettings.Project.ProjectFolder, Settings.ApplicationSettings.Project.DocumentName + " - PROOF.rtf");
            }
            
            // return result.
            return rtf.WriteDocument();
        }

        /// <summary>
        /// Generates RTF file for subtitles proof reading and SyncLoop format archival.
        /// </summary>
        public static string GenerateSyncLoopSubtitles(List<Loop> loops, Settings settings, ProgramInfo programInfo)
        {
            // List of document content as RTFParagraphs.
            List<RTFParagraph> documentRTFParagraphs = new List<RTFParagraph>();

            // Flag to know if this is the first content paragraph, to set page break.
            bool IS_FIRST_LOOP = true;

            // List of characters color.s
            List<Color> documentColors = new List<Color>();
            
            // Color for character id.
            documentColors.Add(Colors.Black);
            documentColors.Add(Colors.RoyalBlue);

            foreach (Loop loop in loops)
            {
                // Test for subtitle loop.
                if(loop.Mode == DocumentMode.Subtitles)
                {
                    // CHECK FOR FIRST LOOP, FOR BLANK SPACING.
                    if (IS_FIRST_LOOP)
                    {
                        // ADD PARAGRAPH.
                        documentRTFParagraphs.Add(new RTFParagraph(loop.Timecode,
                                new RTFParagraphOptions()
                                {
                                    DoNotSpellCheck = true,
                                    PageBreakBefore = true
                                })
                        {
                            IsLoop = true
                        });
                        // SET FLAG.
                        IS_FIRST_LOOP = false;
                    }
                    else
                    {
                        // ADD BLANK PARAGRAPH.
                        documentRTFParagraphs.Add(new RTFParagraph(String.Empty, new RTFParagraphOptions()));
                        // ADD ACTUAL PARAGRAPH.
                        documentRTFParagraphs.Add(new RTFParagraph(loop.Timecode,
                                new RTFParagraphOptions()
                                {
                                    DoNotSpellCheck = true
                                })
                        {
                            IsLoop = true
                        });
                    }
                    // ADD DIALOG PARAGRAPHS.
                    foreach (string sub in loop.Subtitles)
                    {
                        if (!String.IsNullOrEmpty(sub))
                        {
                            documentRTFParagraphs.Add(new RTFParagraph(sub, new RTFParagraphOptions()
                            {
                                Justification = RTFJustification.Justified,
                                ParagraphColor = (sub.Length > Settings.ApplicationSettings.SubtitleLength) ? 1 : 0
                            }));
                        }
                    }
                }
            }

            // Create color table.
            List<RTFColor> rtfColors = new List<RTFColor>();
            // Convert colors list to RTFColor list.
            // Add black and white.
            foreach (Color c in documentColors)
            {
                rtfColors.Add(new RTFColor(c.R, c.G, c.B));
            }

            RTFColorTable colors = new RTFColorTable(rtfColors);
            // Create fonts.
            string[] fonts = new string[]
            {
                        settings.ContentFont,
                        settings.AdditionalFont,
                        settings.HeadersAndFootersFont
            };

            // Create final paragraphs list.
            List<RTFParagraph> finalDocumentContent = new List<RTFParagraph>();

            // Add content.
            finalDocumentContent.AddRange(documentRTFParagraphs);
            // Create RTF document.
            RTFDocument rtf = new RTFDocument(fonts, finalDocumentContent, colors);
            // Set left header.
            rtf.Properties.DocumentTitleEnglish = $"{programInfo.EpisodeSeries.NameEnglish} - {programInfo.EpisodeNumber}";
            // Set right header.
            rtf.Properties.DocumentTitleSpanish = $"{programInfo.EpisodeSeries.NameSpanish} - {programInfo.EpisodeNumber}";
            // Set company name.
            rtf.Properties.Company = settings.TranslatorName;
            // Set author.
            rtf.Properties.DocumentAuthor = settings.TranslatorName;
            // Set comment.
            rtf.Properties.Comment = $"This episode of {programInfo.EpisodeSeries.NameEnglish} was translated by {settings.TranslatorName}.";
            //// Generate file name.
            //string filename = String.Empty;
            //// Set filename.
            //filename = Path.Combine(Settings.ApplicationSettings.Project.ProjectFolder, Settings.ApplicationSettings.Project.DocumentName + " SUBTITLES.rtf");
            // return result.
            return rtf.WriteDocument();
        }

        /// <summary>
        /// Counts character lines.
        /// </summary>
        public static List<RTFParagraph> CountLines(Settings settings, ObservableCollection<Character> characters)
        {

            // List of character names, gender and lines.
            List<RTFParagraph> result = new List<RTFParagraph>();
            // Add blank paragraph.
            result.Add(new RTFParagraph(String.Empty, new RTFParagraphOptions()));
            // Iterate through characters list.
            foreach (Character c in characters)
            {
                // Add paragraph.
                result.Add(new RTFParagraph(
                    $"{c.Name.ToUpper()} ({c.Gender}): {c.Lines}", new RTFParagraphOptions()));
            }
            return result;
        }


        #endregion
    }
}
