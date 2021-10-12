using FFmpeg.AutoGen;
using SyncLoopLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Unosquare.FFME;
using Unosquare.FFME.Common;
using MessageBox = System.Windows.MessageBox;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using System.Diagnostics;

namespace SyncLoop
{
    /// <summary>
    /// Interaction logic for SubtitlesPlayer.xaml
    /// </summary>
    public partial class SubtitlesPlayer : Window
    {

        // The video file path.
        string Video;
        // List of subtitle loops.
        List<Loop> Loops;
        // Queue of jump points.
        Queue<JumpPoint> Jumps;
        // Next jump point.
        JumpPoint NextJump = null;
        // Total number of loops.
        int NumberOfLoops = 0;
        // Stores the current reported picture number.
        long FrameNumber = 0;
        // Flags.
        bool IS_MOVIE_LOADED = false;

        bool SUBTITLES_ENDED = false;
        // Current loop index.
        int CurrentIndex = 0;
        // Las reported picture number. Used for checking if frames are missing.
        private static long LastPicture = 0;



        #region CONSTRUCTORS

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="videoPath">Video file path.</param>
        /// <param name="loops">List of subtitle loops.</param>
        public SubtitlesPlayer(string videoPath, List<Loop> loops)
        {
            InitializeComponent();

            // Store parameters.
            Video = videoPath;

            Loops = loops;
            // Get total number of loops.
            NumberOfLoops = Loops.Count;

            if (!Library.IsInitialized)
            {
                InitFFME();
            }

            // Events.
            VideoPlayer.MediaOpening += MediaOpeningList;

            VideoPlayer.MediaOpened += MediaOpenedList;

            VideoPlayer.MediaFailed += VideoPlayer_MediaFailed;

            VideoPlayer.RenderingVideo += RenderingVideoList;

            ContentRendered += SubtitlesPlayer_ContentRendered;
        }

        /// <summary>
        /// Constructor based on jump points.
        /// </summary>
        /// <param name="videoPath">Video file path.</param>
        /// <param name="jumps">Queue of jump point object.</param>
        /// <remarks>
        /// This video uses FFME built-in FMPEG subtitles library.
        /// It uses a queue to specify jumps in the video, to avoid watching
        /// the whole file when checking subtitles.
        /// </remarks>
        public SubtitlesPlayer(string videoPath, Queue<JumpPoint> jumps)
        {

            InitializeComponent();

            // Store parameters.
            Video = videoPath;

            Jumps = jumps;

            if (!Library.IsInitialized)
            {
                InitFFME();
            }

            // Window events.
            PreviewKeyDown += Window_PreviewKeyDown;
            // Video player events.
            VideoPlayer.MediaOpening += MediaOpeningQueue;

            VideoPlayer.MediaOpened += MediaOpenedQueue;

            VideoPlayer.MediaFailed += VideoPlayer_MediaFailed;

            VideoPlayer.RenderingVideo += RenderingVideoQueue;

            ContentRendered += SubtitlesPlayer_ContentRendered;
            // Hide the dark background.
            SubtitleArea.Visibility = Visibility.Collapsed;

        }

        #endregion



        #region METHODS

        /// <summary>
        /// Inits FFME engine.
        /// </summary>
        private void InitFFME()
        {
            // https://ffmpeg.zeranoe.com/builds/win32/shared/ffmpeg-4.0.2-win32-shared.zip

            Library.FFmpegDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Libraries", "ffmpeg" + (Environment.Is64BitProcess ? "64" : "32"));

            Library.FFmpegLoadModeFlags = FFmpegLoadMode.FullFeatures;

            Library.LoadFFmpeg();

            Library.EnableWpfMultiThreadedVideo = false;
        }

        #endregion



        #region EVENTS

        /// <summary>
        /// Set subtitles and audio track.
        /// </summary>
        private void MediaOpeningQueue(object sender, MediaOpeningEventArgs e)
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
            string subtitles = Path.ChangeExtension(Video, ".srt");

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
        /// Sets flag and plays video.
        /// </summary>
        private void MediaOpenedQueue(object sender, MediaOpenedEventArgs e)
        {
            IS_MOVIE_LOADED = true;

            VideoPlayer.Play();
        }


        /// <summary>
        /// Set subtitles and audio track.
        /// </summary>
        private void MediaOpeningList(object sender, MediaOpeningEventArgs e)
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

            // Get audio tracks and assign it.
            var audioStreams = e.Info.Streams.Where(kvp => kvp.Value.CodecType == AVMediaType.AVMEDIA_TYPE_AUDIO).Select(kvp => kvp.Value);

            var selectedStream = audioStreams
                .FirstOrDefault(s => s.StreamId == 2);

            e.Options.AudioStream = selectedStream;

        }

        /// <summary>
        /// Sets flag and initializes subtitles jump list.
        /// </summary>
        private void MediaOpenedList(object sender, MediaOpenedEventArgs e)
        {
            IS_MOVIE_LOADED = true;
            // Create SMPTE to 1 second before first subtitle.
            SMPTE initialSubtitle = (new SMPTE((int)(Loops[0].InFrame - 30)));
            // Get the second.
            int seconds = SMPTE.ConvertSMPTEtoSeconds(initialSubtitle.Timecode);

            VideoPlayer.Position = TimeSpan.FromSeconds(seconds);

            VideoPlayer.Play();
        }

        /// <summary>
        /// Alerts failure.
        /// </summary>
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

        /// <summary>
        /// Sets video file and window title.
        /// </summary>
        private void SubtitlesPlayer_ContentRendered(object sender, EventArgs e)
        {
            VideoPlayer.Source = new Uri(Video);
            // Set title bar.
            Title = Video;
        }

        /// <summary>
        /// Gets frame number and controls subtitles rendering and jumps.
        /// </summary>
        private void RenderingVideoList(object sender, RenderingVideoEventArgs e)
        {
            FrameNumber = e.PictureNumber;
            // Check for missing frames, if delta > 1.
            long delta = FrameNumber - LastPicture;
            // This will store the frames to check for subtitles cues.
            long[] frames;

            // If we are not at the end of the loops...
            if (CurrentIndex < NumberOfLoops)
            {
                if (delta > 1)
                {
                    // Create an array of the appropriate size and populate it with the missing frames and the current one.
                    frames = new long[delta];

                    for (int d = 0; d < delta; d++)
                    {
                        frames[d] = LastPicture + d + 1;
                    }
                }
                else
                {
                    frames = new long[] { FrameNumber };
                }

                for (int i = 0; i < frames.Length; i++)
                {
                    if (Loops[CurrentIndex].InFrame == frames[i])
                    {
                        Subtitle1TextBlock.Text = Loops[CurrentIndex].Subtitles[0];

                        Subtitle2TextBlock.Text = Loops[CurrentIndex].Subtitles[1];
                    }

                    if (Loops[CurrentIndex].OutFrame == frames[i])
                    {
                        Subtitle1TextBlock.Text = string.Empty;

                        Subtitle2TextBlock.Text = string.Empty;

                        CurrentIndex++;

                        if (CurrentIndex < NumberOfLoops && Loops[CurrentIndex].InFrame - FrameNumber > 60)
                        {
                            SMPTE next = (new SMPTE((int)(Loops[CurrentIndex].InFrame - 30)));

                            int seconds = SMPTE.ConvertSMPTEtoSeconds(next.Timecode);

                            VideoPlayer.Position = TimeSpan.FromSeconds(seconds);
                        }
                    }

                }
            }
            else
            {
                VideoPlayer.Stop();
            }

            LastPicture = FrameNumber;
        }

        /// <summary>
        /// Gets frame number and controls subtitles rendering and jumps.
        /// </summary>
        private void RenderingVideoQueue(object sender, RenderingVideoEventArgs e)
        {
            if (!SUBTITLES_ENDED)
            {
                FrameNumber = e.PictureNumber;
                // Check for missing frames, if delta > 1.
                long delta = FrameNumber - LastPicture;
                // This will store the frames to check for subtitles cues.
                long[] frames;

                if (delta > 1)
                {
                    // Create an array of the appropriate size and populate it with the missing frames and the current one.
                    frames = new long[delta];

                    for (int d = 0; d < delta; d++)
                    {
                        frames[d] = LastPicture + d + 1;
                    }
                }
                else
                {
                    frames = new long[] { FrameNumber };
                }

                if (NextJump == null)
                {
                    NextJump = Jumps.Dequeue();
                }

                for (int i = 0; i < frames.Length; i++)
                {
                    if (NextJump.Frame == frames[i])
                    {
                        VideoPlayer.Position = NextJump.Span;
                        // Get next jump point.
                        if (Jumps.Count > 0)
                        {
                            NextJump = Jumps.Dequeue();
                        }
                        else
                        {
                            SUBTITLES_ENDED = true;
                        }
                    }
                }
            }

            LastPicture = FrameNumber;
        }

        /// <summary>
        /// Navigation controls.
        /// </summary>
        protected void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (IS_MOVIE_LOADED)
            {
                switch (e.Key)
                {
                    // Play/Pause key.
                    case Key.Enter:

                        if (!VideoPlayer.IsPlaying)
                        {
                            VideoPlayer.Play();
                        }
                        else
                        {
                            VideoPlayer.Pause();
                        }

                        e.Handled = true;

                        break;

                    // Advance one frame.
                    case Key.Right:

                        if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                        {
                            VideoPlayer.Position += TimeSpan.FromSeconds(10);
                        }
                        else if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                        {
                            VideoPlayer.Position += TimeSpan.FromSeconds(1);
                        }
                        else
                        {
                            VideoPlayer.StepForward();
                        }

                        e.Handled = true;

                        break;

                    // Back one frame.
                    case Key.Left:

                        if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                        {
                            VideoPlayer.Position -= TimeSpan.FromSeconds(10);
                        }
                        else if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                        {
                            VideoPlayer.Position -= TimeSpan.FromSeconds(1);
                        }
                        else
                        {
                            VideoPlayer.StepBackward();
                        }

                        e.Handled = true;

                        break;

                }
            }
        }

        #endregion
    }
}
