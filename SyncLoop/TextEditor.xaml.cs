using SyncLoop.Video;
using SyncLoopLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace SyncLoop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class TextEditor : Window
    {
        #region MEMBERS

        /// <summary>
        /// Flag for the existence of a document in the editor.
        /// Sets the CanExecute flag to true on menu items.
        /// </summary>
        bool DOCUMENT_LOADED = false;

        /// <summary>
        /// Character selector window.
        /// </summary>
        CharacterSelector ProgramCharacterSelector;

        /// <summary>
        /// Characters list.
        /// </summary>
        ObservableCollection<Character> Characters = new ObservableCollection<Character>();

        /// <summary>
        /// Channels list.
        /// </summary>
        ObservableCollection<Channel> Channels = new ObservableCollection<Channel>();

        /// <summary>
        /// Saved series from DB.
        /// </summary>
        ObservableCollection<Series> series = new ObservableCollection<Series>();
        
        /// <value>
        /// Shortcuts to be inserted by pressing the function keys.
        /// </value>
        public static ObservableCollection<string> shortcuts = Utilities.InitShortcuts();

        /// <summary>
        /// Directory where original translated text file was opened from.
        /// </summary>
        string OriginalTextFileFolder = String.Empty;

        /// <summary>
        /// Name of text file in editor.
        /// </summary>
        string TextFileName = String.Empty;

        /// <summary>
        /// Video player window.
        /// </summary>
        VideoWindow Player;

        /// <summary>
        /// This list holds the final Loop collection for generating
        /// the Exel document.
        /// </summary>
        List<Loop> DocumentLoops = new List<Loop>();

        /// <summary>
        /// Keeps track of loops for autosave.
        /// </summary>
        static int LoopsSinceLastSave = 0;

        #endregion



        #region CONSTRUCTORS

        /// <summary>
        /// Default constructor.
        /// </summary>
        public TextEditor()
        {
            InitializeComponent();

            #region GENERAL

            // Get channels from DB.
            Channels = Database.GetChannels();
            // Get series from DB.
            series = Database.GetSeries();

            #endregion



            #region VIDEO

            // Create video player.
            CreateVideoWindow();

            #endregion



            #region TEXT EDITOR

            // Subscribe.
            // this.Loaded += TextEditor_Loaded;
            // Get screen info.
            ScreenInfo screen = new ScreenInfo(this);
            // Set editor position.
            Left = 1;

            Top = 1;

            Height = screen.ScreenHeight;

            ContentRendered += TextEditor_ContentRendered;

            Width = Player.DefaultLocation.X;
            // Subscribe to selection changed event.
            Editor.SelectionChanged += SelectionChanged;
            // Data bindings.
            Editor.DataContext = Settings.ApplicationSettings;

            FontSize.DataContext = Settings.ApplicationSettings;

            ModeBlock.DataContext = Settings.ApplicationSettings;

            ModeBorder.DataContext = Settings.ApplicationSettings;

            #endregion

        }

        #endregion



        #region METHODS

        /// <summary>
        /// Creates video window.
        /// </summary>
        private void CreateVideoWindow()
        {
            switch (Settings.ApplicationSettings.VideoEngine)
            {
                case VideoMode.FFME:

                    Player = new FFMEPlayer()
                    {
                        Title = "FFME"
                    };

                    break;

                case VideoMode.QuickTime:

                    Player = new QuickTimePlayer()
                    {
                        Title = "QuickTime"
                    };

                    break;

                case VideoMode.Internal:

                    Player = new InternalPlayer()
                    {
                        Title = "Internal"
                    };

                    break;

                default:

                    break;
            }

            // Set document mode.
            Player.DocumentType = Settings.ApplicationSettings.DocumentType;
            // Open video player.
            Player.ShowWindow();
        }

        #endregion



        #region EVENT HANDLERS

        /// <summary>
        /// Displays any loading error.
        /// </summary>
        private void TextEditor_ContentRendered(object sender, EventArgs e)
        {
            if (Settings.ApplicationSettings.ErrorMessage != null)
            {
                MessageBox.Show(Settings.ApplicationSettings.ErrorMessage,
                                "SyncLoop",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
            }
        }


        /// <summary>
        /// Confirms closing.
        /// </summary>
        protected override void OnClosing(CancelEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to exit?", 
                                "SyncLoop", 
                                MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.Cancel)
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// Opens video window.
        /// </summary>
        private void OpenVideoWindowButton_Click(object sender, RoutedEventArgs e)
        {
            CreateVideoWindow();

            OpenVideoWindowButton.Visibility = Visibility.Hidden;
        }

        #endregion
    }
}
