using SyncLoopLibrary;
using System;
using System.IO;
using System.Windows;

namespace SyncLoop.Video
{

    /// <summary>
    /// Interaction logic for Internal.xaml
    /// </summary>
    public partial class InternalPlayer : VideoWindow
    {

        #region MEMBERS

        /// <summary>
        /// State of the video player/
        /// </summary>
        private bool IS_PLAYING = false;

        #endregion



        #region CONSTRUCTORS

        /// <summary>
        /// Default constructor.
        /// </summary>
        public InternalPlayer()
        {
            InitializeComponent();
            // Get movie properties.
            GetMovieInfo();
            // Register closer event.
            Closed += VideoWindow_Closing;
            // Register MediaOpened event.
            VideoPlayer.MediaOpened += VideoOpened;
            // Allow scrubbing.
            VideoPlayer.ScrubbingEnabled = true;
            // Play video to load. Go figure...
            VideoPlayer.Pause();
        }

        #endregion



        #region VIRTUAL METHODS OVERRIDES

        /// <summary>
        /// Produces final SMPTE string.
        /// </summary>
        /// <param name="smpte">Loop from current position.</param>
        /// <param name="plusOne">Flag to add an extra frame for automatically generated cue in points for subtitles.</param>
        /// <returns>Formatted string.</returns>
        protected override string GetSmpteString(SMPTE smpte, bool plusOne = false)
        {
            if (IS_MOVIE_LOADED)
            {
                SMPTE currentPosition;

                // Check if initial offset has been set.
                if (InitialOffset != null)
                {
                    // Current position smpte.
                    currentPosition = InitialOffset + smpte;
                }
                else
                {
                    currentPosition = smpte;
                }
                
                // Normalize it.
                currentPosition.Normalize();
                // Return string.
                return DropFrames(currentPosition).ToString();
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
            double currentPosition = VideoPlayer.Position.TotalMilliseconds;
            // Frame duration in milliseconds.
            double frameDuration = 1000 / FrameRate;
            // Requeste position.
            double newPosition = currentPosition + seconds * 1000;

            // Check for beginning or end of movie.
            if (newPosition < 0)
            {
                newPosition = 0;
            }
            else if (newPosition > VideoPlayer.NaturalDuration.TimeSpan.TotalMilliseconds)
            {
                newPosition = VideoPlayer.NaturalDuration.TimeSpan.TotalMilliseconds;
            }
            
            // Set position.
            VideoPlayer.Position = TimeSpan.FromMilliseconds(newPosition);
            // Set label.
            CenterLabel.Text = GetSmpteString(new SMPTE(VideoPlayer.Position));
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

            // Create TimeSpan from it.
            TimeSpan position = new TimeSpan(
                0,
                finalLoop.TimecodeTokens[0],
                finalLoop.TimecodeTokens[1],
                finalLoop.TimecodeTokens[2],
                finalLoop.ConvertFramesToMilliseconds(FrameRate));
            
            // Check for allowed navigation.
            if (position > VideoPlayer.NaturalDuration)
            {
                position = VideoPlayer.NaturalDuration.TimeSpan;
            }
            else if (position < TimeSpan.Zero)
            {
                position = TimeSpan.Zero;
            }

            VideoPlayer.Position = position;
            // Set label.
            CenterLabel.Text = GetSmpteString(new SMPTE(position));
        }


        /// <summary>
        /// NextFrame video override for this video player type.
        /// </summary>
        protected override void NextFrame()
        {
            // Current position of the video.
            double currentPosition = VideoPlayer.Position.TotalMilliseconds;
            // Frame duration in milliseconds.
            double frameDuration = 1000 / FrameRate;
            // Requeste position.
            double newPosition = currentPosition + frameDuration;
         
            // Forward if possible.
            if (newPosition <= VideoPlayer.NaturalDuration.TimeSpan.TotalMilliseconds)
            {
                VideoPlayer.Position = TimeSpan.FromMilliseconds(newPosition);
                // Set label.
                CenterLabel.Text = GetSmpteString(new SMPTE(VideoPlayer.Position));
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
                VideoPlayer.Source = new Uri(file);
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
        /// Pause override for this video player.
        /// </summary>
        protected override void Pause()
        {
            if (IS_PLAYING)
            {
                // Play..
                VideoPlayer.Pause();
                // Set flag.
                IS_PLAYING = false;
            }
        }


        /// <summary>
        /// PlayOrPause video override for this video player type.
        /// </summary>
        protected override void PlayOrPause()
        {
            if (!IS_PLAYING)
            {
                // Pause.
                VideoPlayer.Play();
                // Set flag
                IS_PLAYING = true;
            }
            else
            {
                // Play..
                VideoPlayer.Pause();
                // Set flag.
                IS_PLAYING = false;
                // Set current position.
                CurrentPosition = new SMPTE(VideoPlayer.Position);
                // Update label.                
                CenterLabel.Text = GetSmpteString(CurrentPosition);
            }
        }


        /// <summary>
        /// PlayOrPause video override for this video player type.
        /// </summary>
        protected override void Play()
        {
            if (!IS_PLAYING)
            {
                VideoPlayer.Play();
                // Set flag.
                IS_PLAYING = true;
            }
        }


        /// <summary>
        /// PreviousFrame video override for this video player type.
        /// </summary>
        protected override void PreviousFrame()
        {
            // Current position of the video.
            double currentPosition = VideoPlayer.Position.TotalMilliseconds;
            // Frame duration in milliseconds.
            double frameDuration = 1000 / FrameRate;
            // Requeste position.
            double newPosition = currentPosition - frameDuration;
        
            // Back if possible.
            if (newPosition >= 0)
            {
                VideoPlayer.Position = TimeSpan.FromMilliseconds(newPosition);
                // Set label.
                CenterLabel.Text = GetSmpteString(new SMPTE(VideoPlayer.Position));
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
            // Set current position.
            CurrentPosition = new SMPTE(VideoPlayer.Position);

            return VideoPlayer.Position;
        }

        #endregion



        #region METHODS

        /// <summary>
        /// Gets video file properties.
        /// </summary>
        public void GetMovieInfo()
        {
            // Frame rate of video.
            FrameRate = 30000 / 1001d;
        }

        #endregion



        #region EVENT HANDLERS

        /// <summary>
        /// Event handler for media loaded.
        /// </summary>
        private void VideoOpened(object sender, RoutedEventArgs e)
        {
            // Set flag.
            IS_MOVIE_LOADED = true;
            // Set initial offset.
            InitialOffset = new SMPTE(DialogsService.GetSmpte());
            // Set initial label.
            // Update current position.
            UpdatePosition();

            // Set labels.
            switch (DocumentType)
            {
                case DocumentMode.Excel:

                    UpdateLabels(GetSmpteString(CurrentPosition), TimecodeLabels.Center);
                    break;

                case DocumentMode.RTF:

                    UpdateLabels(GetSmpteString(CurrentPosition), TimecodeLabels.Left);
                    break;

                case DocumentMode.Subtitles:

                    UpdateLabels(GetSmpteString(CurrentPosition), TimecodeLabels.Center);
                    break;
            }

            // We set the keydown event handler now that the video is loaded
            // to avoid errors when pressing the keys.
            PreviewKeyDown += Window_PreviewKeyDown;
        }


        #endregion
    }
}
