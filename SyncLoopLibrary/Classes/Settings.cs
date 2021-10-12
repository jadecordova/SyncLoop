using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace SyncLoopLibrary
{
    /// <summary>
    /// Application settings.
    /// </summary>
    public class Settings : Notifier
    {

        #region FIELDS

        private DocumentMode documentType;

        #endregion



        #region GLOBAL

        /// <summary>
        /// Static settings object.
        /// </summary>
        public static Settings ApplicationSettings { get; set; } = new Settings();

        #endregion



        #region GENERAL PROPERTIES

        /// <summary>
        /// Project file object.
        /// </summary>
        public Project Project { get; set; } = null;

        /// <summary>
        /// SVG template file.
        /// </summary>
        public string SVGTemplate { get; set; }

        /// <summary>
        /// Settings file.
        /// </summary>
        public string SettingsFile { get; set; }

        /// <summary>
        /// RegEx for general SMPTE format used in search and loop offset.
        /// </summary>
        public string SmpteFormat { get; set; } = @"\d\d:\d\d:\d\d:\d\d";

        /// <summary>
        /// Current rates.
        /// </summary>
        public Rates CurrentRates { get; set; } = null;

        /// <summary>
        /// Should subtitles accented characters be changed?
        /// </summary>
        public bool EncodeCharacters { get; set; } = true;

        /// <summary>
        /// Current period.
        /// </summary>
        public Period CurrentPeriod { get; set; } = null;

        /// <summary>
        /// General Total.
        /// </summary>
        public decimal GeneralTotal { get; set; } = 0;

        /// <summary>
        /// Special folders.
        /// </summary>
        public Dictionary<string, string> Folders { get; set; }

        /// <summary>
        /// Loading error message.
        /// </summary>
        public string ErrorMessage { get; set; } = null;

        #endregion



        #region EXCEL PROPERTIES

        /// <summary>
        /// Should the row for general total lines be included?
        /// </summary>
        public bool WriteExcelTotalsRow { get; set; } = false;

        /// <summary>
        /// Should this columns be visible?
        /// </summary>
        public bool HideLoopAndEnglishColumns { get; set; } = false;

        /// <summary>
        /// Should the font for loops be Courier New or the default font?
        /// </summary>
        public bool UseCourierNewForContentInExcel { get; set; } = false;

        /// <summary>
        /// Should the internal loop lines counter should be used instead of Excel formula?
        /// </summary>
        public bool UseInternalLineCount { get; set; } = false;

        #endregion



        #region TEXT EDITOR PROPERTIES

        /// <summary>
        /// Type of document created by application
        /// </summary>
        public DocumentMode DocumentType
        {
            get { return documentType; }
            set
            {
                documentType = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Default additional font size.
        /// </summary>
        public int AdditionalFontSize { get; set; } = 14;

        /// <summary>
        /// Default content font size.
        /// </summary>
        public int ContentFontSize { get; set; } = 14;

        /// <summary>
        /// Default text editor font size.
        /// </summary>
        public int EditorFontSize { get; set; } = 28;

        /// <summary>
        /// Default headers and footers font size.
        /// </summary>
        public int HeadersAndFootersFontSize { get; set; } = 10;

        /// <summary>
        /// Maximum number of characters permitted by loop.
        /// </summary>
        public int LoopLength { get; set; } = 228;

        /// <summary>
        /// Subtitle line length.
        /// </summary>
        public int SubtitleLength { get; set; } = 35;

        /// <summary>
        /// Number of frames between subtitles.
        /// </summary>
        public int FramesBetweenSubtitles { get; set; } = 1;

        /// <summary>
        /// Number of decimal places used by the units converter.
        /// </summary>
        public int ConverterDecimalPlaces { get; set; } = 2;

        /// <summary>
        /// Additional font fot the document.
        /// </summary>
        public string AdditionalFont { get; set; } = "Calibri";

        /// <summary>
        /// Font for the reading portion of the document.
        /// </summary>
        public string ContentFont { get; set; } = "Courier New";

        /// <summary>
        /// Font for the text editor.
        /// </summary>
        public string EditorFont { get; set; } = "Courier New";

        /// <summary>
        /// Font for headers and footers.
        /// </summary>
        public string HeadersAndFootersFont { get; set; } = "Courier New";

        /// <summary>
        /// String to identify title loops.
        /// </summary>
        public string TitleString { get; set; } = "LETRERO";

        /// <summary>
        /// Number of characters per line for character line counting.
        /// </summary>
        public int CharactersPerLine { get; set; } = 57;

        /// <summary>
        /// Translator name.
        /// </summary>
        public string TranslatorName { get; set; } = "Glyphos, Servicios de Comunicación C. A.";

        /// <summary>
        /// Character shorcut for gestures.
        /// </summary>
        public string GesturesCharacter { get; set; } = "+";

        /// <summary>
        /// Autosaves document after this number of loops.
        /// </summary>
        public int Autosave { get; set; } = 5;

        /// <summary>
        /// Date due of current program que from client.
        /// </summary>
        public DateTime CurrentDateDue { get; set; } = DateTime.Now;

        /// <summary>
        /// Flag for text editor text changed event.
        /// </summary>
        public bool IsSubtitle { get; set; } = false;

        /// <summary>
        /// Offset to scroll documents when creating subtitles.
        /// </summary>
        public int SubtitlesScrollOffset { get; set; } = 200;

        /// <summary>
        /// Spell checking
        /// </summary>
        public bool SpellCheckerEnabled { get; set; } = false;

        #endregion



        #region VIDEO PROPERTIES

        /// <summary>
        /// Video engine used.
        /// </summary>
        public VideoMode VideoEngine { get; set; } = VideoMode.Internal;

        /// <summary>
        /// Numbe of frames to go back to compensate for human reaction speed.
        /// </summary>
        public int FrameCompensation { get; set; } = 7;

        /// <summary>
        /// Amount of seconds to rewind video after inserting a smpte.
        /// </summary>
        public int SecondsToRewindVideoAfterLoop { get; set; } = 1;

        /// <summary>
        /// Subtitles mode.
        /// </summary>
        public SubtitlesMode SubtitlesMode = SubtitlesMode.FFME;

        #endregion



        #region WINDOW POSITIONS

        /// <summary>
        /// Error window top.
        /// </summary>
        public double? ErrorWindowTop { get; set; } = null;

        /// <summary>
        /// Error window left.
        /// </summary>
        public double? ErrorWindowLeft { get; set; } = null;

        /// <summary>
        /// Error window width.
        /// </summary>
        public double? ErrorWindowWidth { get; set; } = null;

        /// <summary>
        /// Error window height.
        /// </summary>
        public double? ErrorWindowHeight { get; set; } = null;

        /// <summary>
        /// Find window top.
        /// </summary>
        public double? FindWindowTop { get; set; } = null;

        /// <summary>
        /// Find window left.
        /// </summary>
        public double? FindWindowLeft { get; set; } = null;

        /// <summary>
        /// Find window width.
        /// </summary>
        public double? FindWindowWidth { get; set; } = null;

        /// <summary>
        /// Find window height.
        /// </summary>
        public double? FindWindowHeight { get; set; } = null;

        #endregion



        #region CONSTRUCTORS

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Settings()
        {
        }

        #endregion



        #region METHODS

        /// <summary>
        /// Saves app settings to disk, to predetermined folder.
        /// This folder is stored in the private variable Folders["Settings"].
        /// The file, called "Settings.json", is stored in <see cref="SettingsFile"/>.
        /// </summary>
        public async void SaveSettings()
        {
            // Create folder if it doesn't exist.
            if (!Directory.Exists(Folders["Settings"]))
            {
                Directory.CreateDirectory(Folders["Settings"]);
            }
            // Write file.
            try
            {
                using (StreamWriter writer = new StreamWriter(SettingsFile))
                {
                    // Use Newtonsoft.JSON.
                    await writer.WriteAsync(JsonConvert.SerializeObject(this, Formatting.Indented));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"There was an error writing the settings file: {ex.Message}", 
                                 "SyncLoop", 
                                 MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Saves settings in synchronous mode.
        /// </summary>
        public void SaveSettingsSync()
        {
            // Create folder if it doesn't exist.
            if (!Directory.Exists(Folders["Settings"]))
            {
                Directory.CreateDirectory(Folders["Settings"]);
            }
            // Write file.
            try
            {
                using (StreamWriter writer = new StreamWriter(SettingsFile))
                {
                    // Use Newtonsoft.JSON.
                    writer.Write(JsonConvert.SerializeObject(this, Formatting.Indented));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"There was an error writing the settings file: {ex.Message}", 
                                 "SyncLoop", 
                                 MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Loads app settings from disk, from predetermined folder.
        /// This folder is stored in the private variable Folders["Settings"]>.
        /// The file, called "Settings.json", is stored in <see cref="SettingsFile"/>.
        /// </summary>
        public static void LoadSettings(string file)
        {
            // CREATE VARIABLE TO HOLD THE JSON TEXT FILE.
            string json;
            // IF THE FILE EXISTS...
            if (File.Exists(file))
            {
                try
                {
                    using (StreamReader reader = new StreamReader(file))
                    {
                        json = reader.ReadToEnd();
                    }
                    if (!String.IsNullOrEmpty(json))
                    {
                        try
                        {
                            ApplicationSettings = JsonConvert.DeserializeObject<Settings>(json);
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show($"There was an error reading the settings file contents: {e.Message}",
                                             "SyncLoop",
                                             MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Settings file is invalid.",
                                        "SyncLoop",
                                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"There was an error reading the settings file: {ex.Message}",
                                     "SyncLoop", 
                                     MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Settings file doesn't exist.",
                                "SyncLoop", 
                                MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        #endregion
    }
}
