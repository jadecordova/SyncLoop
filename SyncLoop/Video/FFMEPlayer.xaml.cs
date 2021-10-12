using FFmpeg.AutoGen;
using SyncLoopLibrary;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using Unosquare.FFME;
using Unosquare.FFME.Common;

namespace SyncLoop.Video
{
    /// <summary>
    /// Interaction logic for FFMEPlayer.xaml
    /// </summary>
    public partial class FFMEPlayer : VideoWindow
    {

        #region CONSTRUCTORS

        /// <summary>
        /// Default constructor.
        /// </summary>
        public FFMEPlayer()
        {
            InitializeComponent();

            InitFFME();

            Subscribe();

            DataContext = Settings.ApplicationSettings;

            CueInLabel.DataContext = Settings.ApplicationSettings;
        }

        /// <summary>
        /// Constructor for video file path.
        /// </summary>
        /// <param name="filePath">Video file path.</param>
        public FFMEPlayer(string filePath = "")
        {
            InitializeComponent();

            // Save parameter.
            VideoFile = filePath;

            InitFFME();

            Subscribe();

            DataContext = Settings.ApplicationSettings;
        }

        #endregion



        #region VIRTUAL METHODS OVERRIDES

        /// <summary>
        /// Gets final SMPTE formatted string.
        /// </summary>
        /// <param name="smpte">Loop to extract SMPTE from or null.</param>
        /// <param name="plusOne">Flag to add one extra frame to separate automatically generated in points for subtitles.</param>
        /// <returns>SMPTE formatted string.</returns>
        protected override string GetSmpteString(SMPTE smpte, bool plusOne = false)
        {
            if (IS_MOVIE_LOADED)
            {
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
        /// Gets final SMPTE formatted string.
        /// </summary>
        /// <param name="frameOffset">Number of frames to substract to compensate for human reaction time.</param>
        /// <returns>SMPTE formatted string.</returns>
        protected override string GetSmpteString(int frameOffset)
        {
            if (IS_MOVIE_LOADED)
            {
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
            VideoPlayer.Position += TimeSpan.FromSeconds(seconds);
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
                position = (TimeSpan)VideoPlayer.NaturalDuration;
            }
            else if (position < TimeSpan.Zero)
            {
                position = TimeSpan.Zero;
            }

            VideoPlayer.Position = position;
            
            // Set label.
            // Frame number is not accurately reported, so we must calculate it manually.
            CenterLabel.Text = GetSmpteString(new SMPTE(position));
        }


        /// <summary>
        /// NextFrame video override for this video player type.
        /// </summary>
        protected override void NextFrame()
        {
            // Forward.
            VideoPlayer.StepForward();
            // Set label.
            CenterLabel.Text = GetSmpteString(null);
        }


        /// <summary>
        /// Open video override for this video player type.
        /// </summary>
        /// <param name="file">File path.</param>
        public override void OpenVideo(string file)
        {
            if (File.Exists(file))
            {
                //Set variable.
                VideoFile = file;

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
        /// Pause video override for this video player type.
        /// </summary>
        protected override void Pause()
        {
            if (!VideoPlayer.IsPaused)
            {
                VideoPlayer.Pause();
            }
        }


        /// <summary>
        /// PlayOrPause video override for this video player type.
        /// </summary>
        protected override void PlayOrPause()
        {
            if (!VideoPlayer.IsPlaying)
            {
                VideoPlayer.Play();
            }
            else
            {
                VideoPlayer.Pause();
            }
        }


        /// <summary>
        /// PlayOrPause video override for this video player type.
        /// </summary>
        protected override void Play()
        {
            if(!VideoPlayer.IsPlaying)
            {
                VideoPlayer.Play();

            }
        }


        /// <summary>
        /// PreviousFrame video override for this video player type.
        /// </summary>
        protected override void PreviousFrame()
        {
            // Back.
            VideoPlayer.StepBackward();
            // Set label.
            CenterLabel.Text = GetSmpteString(null);
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
            if(frameNumber >= 0)
            {
                CurrentPosition = DropFrameTimecodes[frameNumber];
            }
            else
            {
                CurrentPosition = DropFrameTimecodes[(int)FrameNumber];
            }

            // Get current player position.
            return VideoPlayer.Position;
        }

        #endregion



        #region EVENT HANDLERS

        /// <summary>
        /// Event handler for media opening.
        /// Sets subtitles source and delay. Sets audio stream.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VideoPlayer_MediaOpening(object sender, Unosquare.FFME.Common.MediaOpeningEventArgs e)
        {
            var media = sender as Unosquare.FFME.MediaElement;

            // You can start off by adjusting subtitles delay
            // This defaults to 0 but you can delay (or advance with a negative delay)
            // the subtitle timestamps.
            e.Options.SubtitlesDelay = TimeSpan.Zero; // See issue #216

            // You can disable the requirement of buffering packets by setting the playback
            // buffer percent to 0. Values of less than 0.5 for live or network streams are not recommended.
            e.Options.MinimumPlaybackBufferPercent = 1;

            // Legacy audio out is the use of the WinMM api as opposed to using DirectSound
            // Enable legacy audio out if you are having issues with the DirectSound driver.
            media.RendererOptions.UseLegacyAudioOut = false;

            // You can limit how often the video renderer updates the picture.
            // We keep it as 0 to refresh the video according to the native stream specification.
            media.RendererOptions.VideoRefreshRateLimit = 0;

            // Get possible subtitles file.
            string subtitles = Path.ChangeExtension(VideoFile, ".srt");

            // Set subtitles
            if (File.Exists(subtitles))
            {
                e.Options.SubtitlesSource = subtitles;
            }

            // Get audio tracks and assign it.
            var audioStreams = e.Info.Streams.Where(kvp => kvp.Value.CodecType == AVMediaType.AVMEDIA_TYPE_AUDIO).Select(kvp => kvp.Value);
            var selectedStream = audioStreams
                .FirstOrDefault(s => s.StreamId == 2);

            e.Options.AudioStream = selectedStream;

        }


        /// <summary>
        /// Event handler for media loaded.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VideoPlayer_MediaOpened(object sender, Unosquare.FFME.Common.MediaOpenedEventArgs e)
        {
            var media = sender as Unosquare.FFME.MediaElement;
            // Set frame rate.
            FrameRate = media.VideoFrameRate;
            // Set flag.
            IS_MOVIE_LOADED = true;
            // Get video duration.
            VideoDuration = (Duration)VideoPlayer.NaturalDuration;
            // Set initial offset.
            InitialOffset = new SMPTE(DialogsService.GetSmpte());
            // Generate drop frame timecodes.
            DropFrameTimecodes = InitialOffset.GenerateDropFrameTimecode(Convert.ToInt32(VideoDuration.TimeSpan.TotalMinutes + 1), VideoMode.FFME);
            // Set labels.
            switch (DocumentType)
            {
                case DocumentMode.Excel:
                    UpdateLabels(DropFrameTimecodes[0].ToString(), TimecodeLabels.Center);
                    break;
                case DocumentMode.RTF:
                    UpdateLabels(DropFrameTimecodes[0].ToString(), TimecodeLabels.Left);
                    break;
                case DocumentMode.Subtitles:
                    UpdateLabels(DropFrameTimecodes[0].ToString(), TimecodeLabels.Center);
                    break;
            }
        }


        /// <summary>
        /// Event handler for media failed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VideoPlayer_MediaFailed(object sender, MediaFailedEventArgs e)
        {
            MessageBox.Show(
                this,
                $"Media Failed: {e.ErrorException.GetType()}\r\n{e.ErrorException.Message}",
                $"{nameof(Unosquare.FFME.MediaElement)} Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error,
                MessageBoxResult.OK);
        }

        #endregion



        #region METHODS

        /// <summary>
        /// Initializes FFME.
        /// </summary>
        private void InitFFME()
        {
            // https://ffmpeg.zeranoe.com/builds/win32/shared/ffmpeg-4.0.2-win32-shared.zip

            Library.FFmpegDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Libraries", "ffmpeg" + (Environment.Is64BitProcess ? "64" : "32"));

            Library.FFmpegLoadModeFlags = FFmpegLoadMode.FullFeatures;

            Library.LoadFFmpeg();

            Library.EnableWpfMultiThreadedVideo = false;
        }


        /// <summary>
        /// Subscription to event handlers.
        /// </summary>
        private void Subscribe()
        {
            // Window events.
            PreviewKeyDown  += Window_PreviewKeyDown;

            Closed          += VideoWindow_Closing;

            // Video player events.
            VideoPlayer.MediaOpening    += VideoPlayer_MediaOpening;

            VideoPlayer.MediaOpened     += VideoPlayer_MediaOpened;

            VideoPlayer.MediaFailed     += VideoPlayer_MediaFailed;

            VideoPlayer.RenderingVideo  += (s, e) => { FrameNumber = e.PictureNumber; };

        }

        #endregion
    }
}
