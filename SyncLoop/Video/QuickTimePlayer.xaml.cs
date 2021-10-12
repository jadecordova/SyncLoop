using SyncLoopLibrary;
using System;
using System.IO;
using System.Windows;

namespace SyncLoop.Video
{
    /// <summary>
    /// Interaction logic for QuickTimePlayer.xaml
    /// </summary>
    public partial class QuickTimePlayer : VideoWindow
    {
        #region MEMBERS

        /// <summary>
        /// Video player.
        /// </summary>
        private AxQTOControlLib.AxQTControl VideoPlayer;

        /// <summary>
        /// Video TimeScale.
        /// </summary>
        private int TimeScale;

        /// <summary>
        /// Time units per frame.
        /// </summary>
        private int UnitsPerFrame;

        /// <summary>
        /// QuickTime video duration.
        /// Using new to hide parent member.
        /// </summary>
        new private int VideoDuration;

        #endregion



        #region CONSTRUCTORS

        /// <summary>
        /// Default constructor.
        /// </summary>
        public QuickTimePlayer()
        {
            InitializeComponent();
            // Create player.
            VideoPlayer = new AxQTOControlLib.AxQTControl();
            // Add it to WinFormHost.
            PlayerHost.Child = VideoPlayer;
            // Register closer event.
            Closed += VideoWindow_Closing;
        }

        #endregion



        #region VIRTUAL METHODS OVERRIDES

        /// <summary>
        /// Returns final SMPTE format string.
        /// </summary>
        /// <param name="smpte">Loop to extract SMPTE from or null.</param>
        /// <param name="plusOne">Flag to insert extra frame.</param>
        /// <returns>SMPTE formatted string.</returns>
        protected override string GetSmpteString(SMPTE smpte, bool plusOne = false)
        {
            if (IS_MOVIE_LOADED)
            {
                // Calculate current frame.
                FrameNumber = (int)Math.Round(VideoPlayer.Movie.Time / (float)UnitsPerFrame);

                if (!plusOne)
                {
                    return DropFrameTimecodes[(int)FrameNumber].ToString();
                }
                else
                {
                    return DropFrameTimecodes[(int)FrameNumber + 1].ToString();
                }

            }
            else
            {
                return String.Empty;
            }
        }


        /// <summary>
        /// Returns final SMPTE format string.
        /// </summary>
        /// <param name="frameOffset">Frames to compensate for human reaction time.</param>
        /// <returns>SMPTE formatted string.</returns>
        protected override string GetSmpteString(int frameOffset)
        {
            if (IS_MOVIE_LOADED)
            {
                // Calculate current frame.
                FrameNumber = (int)Math.Round(VideoPlayer.Movie.Time / (float)UnitsPerFrame);

                return DropFrameTimecodes[(int)FrameNumber - frameOffset].ToString();
            }
            else
            {
                return String.Empty;
            }
        }


        /// <summary>
        /// Video file navigation.
        /// </summary>
        /// <param name="seconds">Number of seconds to move.</param>
        protected override void GoToTime(int seconds)
        {
            // Current position of the video.
            int currentPosition = VideoPlayer.Movie.Time;
            // Requeste position.
            int newPosition = currentPosition + UnitsPerFrame * 30 * seconds;
        
            // Check for beginning or end of movie.
            if ((newPosition >= 0) && (newPosition <= VideoPlayer.Movie.Duration))
            {
                VideoPlayer.Movie.Time = newPosition;
            }
            else if (newPosition < 0)
            {
                VideoPlayer.Movie.Time = 0;
            }
            else if (newPosition > VideoPlayer.Movie.Duration)
            {
                VideoPlayer.Movie.GotoEnd();
            }
            
            // Update label.
            CenterLabel.Text = GetSmpteString(null);
        }


        /// <summary>
        /// Video file navigation.
        /// </summary>
        /// <param name="smpte">SMPTE format time code string.</param>
        protected override void GoToTime(SMPTE smpte)
        {
            SMPTE finalLoop = new SMPTE();

            // Substract destination smpte from initial offset.
            try
            {
                finalLoop = smpte - InitialOffset;
            }
            catch
            {
                finalLoop = new SMPTE(new int[] { 0, 0, 0, 0 });
            }
            
            // Number of units in hours token.
            int units = finalLoop.TimecodeTokens[0] * 60 * 60 * 30 * UnitsPerFrame;
            // Number of units in minutes token.
            units += finalLoop.TimecodeTokens[1] * 60 * 30 * UnitsPerFrame;
            // Number of units in seconds token.
            units += finalLoop.TimecodeTokens[2] * 30 * UnitsPerFrame;
            // Number of units in frames token.
            units += finalLoop.TimecodeTokens[3] * UnitsPerFrame;

            if (units > VideoPlayer.Movie.Duration)
            {
                units = VideoPlayer.Movie.Duration;
            }
            else if (units < 0)
            {
                units = 0;
            }
            
            // Send the player there.
            VideoPlayer.Movie.Time = units;
            // Update label.
            CenterLabel.Text = GetSmpteString(null);
        }


        /// <summary>
        /// NextFrame video override for this video player type.
        /// </summary>
        protected override void NextFrame()
        {
            // Current position of the video.
            int currentPosition = VideoPlayer.Movie.Time;
            // Requeste position.
            int newPosition = currentPosition += UnitsPerFrame;
         
            // Forward if possible.
            if (newPosition <= VideoPlayer.Movie.Duration)
            {
                VideoPlayer.Movie.Time += UnitsPerFrame;
                // Update label.
                CenterLabel.Text = GetSmpteString(null);
            }
        }


        /// <summary>
        /// Open video override for this video player type.
        /// </summary>
        /// <param name="file">File path.</param>
        public override void OpenVideo(string file)
        {
            if (File.Exists(file))
            {
                VideoPlayer.URL = file;
                // Set title bar.
                this.Title = file;
            }
            else
            {
                MessageBox.Show("Video file doesn't exist.",
                                "SyncLoop", 
                                MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        /// <summary>
        /// Pause video override for this video player type.
        /// </summary>
        protected override void Pause()
        {
            if (VideoPlayer.Movie != null)
            {
                // Pause movie.
                VideoPlayer.Movie.Pause();
            }
        }


        /// <summary>
        /// PlayOrPause video override for this video player type.
        /// </summary>
        protected override void PlayOrPause()
        {
            if (VideoPlayer.Movie != null)
            {
                if (VideoPlayer.Movie.Rate == 0)
                {
                    VideoPlayer.Movie.Play();
                }
                else
                {
                    // Pause movie.
                    VideoPlayer.Movie.Pause();
                }
            }
        }


        /// <summary>
        /// PlayOrPause video override for this video player type.
        /// </summary>
        protected override void Play()
        {
            if (VideoPlayer.Movie != null)
            {
                VideoPlayer.Movie.Play();
            }
        }


        /// <summary>
        /// PreviousFrame video override for this video player type.
        /// </summary>
        protected override void PreviousFrame()
        {
            // Current position of the video.
            int currentPosition = VideoPlayer.Movie.Time;
            // Requeste position.
            int newPosition = currentPosition -= UnitsPerFrame;
        
            // Back if possible.
            if (newPosition >= 0)
            {
                VideoPlayer.Movie.Time -= UnitsPerFrame;
                // Update label.
                CenterLabel.Text = GetSmpteString(null);
            }
        }


        /// <summary>
        /// Updates video window timecode labels.
        /// </summary>
        /// <param name="timecode">Timecode string.</param>
        /// <param name="label">Label to update.</param>
        protected override void UpdateLabels(string timecode, TimecodeLabels label)
        {
            switch (label)
            {
                case TimecodeLabels.Left:

                    CueInLabel.Text = timecode;
                    break;

                case TimecodeLabels.Center:

                    CenterLabel.Text = timecode;
                    break;

                case TimecodeLabels.Right:

                    CueOutLabel.Text = timecode;
                    break;
            }
        }


        /// <summary>
        /// Updates video current position.
        /// </summary>
        protected override TimeSpan? UpdatePosition(int frameNumber = -1)
        {
            // Update current position.
            CurrentPosition = new SMPTE(GetSmpteString(null));

            return null;
        }

        #endregion



        #region EVENT HANDLERS

        /// <summary>
        /// Event handler for media loaded.
        /// </summary>
        private void VideoOpened(object sender, AxQTOControlLib._IQTControlEvents_StatusUpdateEvent e)
        {
            if (e.statusCode == 4099)
            {
                // Set flag.
                IS_MOVIE_LOADED = true;
                // Get movie data.
                GetMovieInfo();
                // Set initial offset.
                InitialOffset = new SMPTE(DialogsService.GetSmpte());
                // Generate drop frame timecodes.
                DropFrameTimecodes = InitialOffset.GenerateDropFrameTimecode(Convert.ToInt32(VideoDuration + 1), VideoMode.QuickTime);
                // Update current position.
                UpdatePosition();
        
                // Set labels.
                switch (DocumentType)
                {
                    case DocumentMode.Excel:

                        UpdateLabels(GetSmpteString(null), TimecodeLabels.Center);
                        break;

                    case DocumentMode.RTF:

                        UpdateLabels(GetSmpteString(null), TimecodeLabels.Left);
                        break;

                    case DocumentMode.Subtitles:

                        UpdateLabels(GetSmpteString(null), TimecodeLabels.Center);
                        break;
                }

                // We set the keydown event handler now that the video is loaded
                // to avoid errors when pressing the keys.
                PreviewKeyDown += Window_PreviewKeyDown;
            }
        }


        /// <summary>
        /// Windows load handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QuickTimePlayerLoaded(object sender, RoutedEventArgs e)
        {
            // Initialize QuickTime and video control.
            VideoPlayer.BorderStyle = QTOControlLib.BorderStylesEnum.bsNone;
            // Hide controls.
            VideoPlayer.MovieControllerVisible = false;
            // Initialize QuickTime.
            int loadError = VideoPlayer.QuickTimeInitialize();
        
            // Check if there was an error loading QuickTime.
            if (loadError != 0) MessageBox.Show("Error loading QuickTime: " + loadError.ToString());
            
            // Assign video opened event handler.
            VideoPlayer.StatusUpdate += VideoOpened;
        }

        #endregion



        #region METHODS

        /// <summary>
        /// Gets video file properties.
        /// </summary>
        public void GetMovieInfo()
        {
            // Frame rate of video.
            FrameRate = VideoPlayer.Movie.StaticFrameRate;
            // QuickTime time scale of movie, generally 600 units per seconds.
            TimeScale = VideoPlayer.Movie.TimeScale;
            // Number of units per frame.
            UnitsPerFrame = (int)Math.Round(TimeScale / FrameRate);
            // Number of minutes.
            VideoDuration = VideoPlayer.Movie.Duration / TimeScale / 60;
        }

        #endregion
    }
}
