using SyncLoopLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using RichTextBox = System.Windows.Controls.RichTextBox;

namespace SyncLoop
{
    /// <summary>
    /// This class forms the basis for the different video engines used.
    /// It presents common virtual methods that are overriden in the specific video windows.
    /// </summary>
    public class VideoWindow : Window
    {

        #region FIELDS

        // Editor window document.
        RichTextBox Editor = ((TextEditor)System.Windows.Application.Current.Windows[0]).Editor;

        // Text editor window.
        TextEditor TextEditorWindow = (TextEditor)System.Windows.Application.Current.Windows[0];

        #endregion



        #region ENUMS

        /// <summary>
        /// SMPTE labels at the bottom of video windows.
        /// Left and right for RTF and Subtitles mode,
        /// and center for Excel mode.
        /// </summary>
        protected enum TimecodeLabels
        {
            /// <summary>
            /// Left timecode label.
            /// </summary>
            Left,
            /// <summary>
            /// Center timecode label.
            /// </summary>
            Center,
            /// <summary>
            /// Right timecode label.
            /// </summary>
            Right
        }

        #endregion



        #region MEMBERS

        /// <summary>
        /// Total lenght of video.
        /// </summary>
        protected Duration VideoDuration;

        /// <summary>
        /// Video frame rate. Defaults to NTSC drop-frame rate.
        /// </summary>
        protected double FrameRate = 29.97;

        /// <summary>
        /// Current video position in SMPTE format.
        /// </summary>
        protected SMPTE CurrentPosition;

        /// <summary>
        /// Cue in point of current loop.
        /// </summary>
        protected SMPTE CueIn;

        /// <summary>
        /// String representation of cue in point.
        /// </summary>
        protected string CueInString;

        /// <summary>
        /// Flag to indicate that a video is loaded in the player.
        /// </summary>
        protected bool IS_MOVIE_LOADED = false;

        /// <summary>
        /// Flag to indicate that a cue in point has been set for current loop.
        /// </summary>
        protected bool IS_CUE_IN_SET = false;

        /// <summary>
        /// Current video frame number. Only relevant for FFME mode,
        /// which reports individual frames.
        /// </summary>
        protected long FrameNumber;

        /// <summary>
        /// Path to video file.
        /// </summary>
        protected string VideoFile;

        /// <summary>
        /// Type of document to be generated. Used to define behaviour of in/out keys.
        /// </summary>
        protected DocumentMode DocumentMode;
        
        /// <summary>
        /// Subtitle info on line height, italics, alignment, etc.
        /// </summary>
        protected Subtitle Subtitle = null;

        /// <summary>
        /// Holds the last entered loop.
        /// </summary>
        protected static SMPTE LastLoop = null;

        /// <summary>
        /// Holds the current working loop.
        /// </summary>
        protected SMPTE CurrentLoop = null;

        /// <summary>
        /// Stores the actual frames for subtitle playback.
        /// </summary>
        protected long InFrame, OutFrame = 0;
        
        // protected string SMPTE;

        #endregion




        #region PROPERTIES

        /// <summary>
        /// Type of document (Excel or RTF).
        /// </summary>
        public DocumentMode DocumentType { get; set; }

        /// <summary>
        /// Text editor.
        /// </summary>
        public TextEditor EditorWindow { get; set; }

        /// <summary>
        /// Point holding the coordinates.
        /// </summary>
        public Point DefaultLocation { get; set; }

        /// <summary>
        /// Initial video offset.
        /// </summary>
        public SMPTE InitialOffset { get; set; }

        /// <summary>
        /// List of drop-frame timecodes.
        /// </summary>
        public List<SMPTE> DropFrameTimecodes { get; set; }

        #endregion



        #region VIRTUAL METHODS

        /// <summary>
        /// Creates final SMPTE string to be used in app.
        /// </summary>
        /// <param name="smpte">Loop object of current position.</param>
        /// <param name="plusOne">Adds one frame to SMPTE object to create valid subtitles.</param>
        /// <returns>SMPTE formated string</returns>
        protected virtual string GetSmpteString(SMPTE smpte, bool plusOne = false) { return String.Empty; }


        /// <summary>
        /// Creates final SMPTE string to be used in app.
        /// </summary>
        /// <param name="frameOffset">Number of frames.</param>
        /// <returns>SMPTE formated string</returns>
        protected virtual string GetSmpteString(int frameOffset) { return String.Empty; }


        /// <summary>
        /// To be overriden by child class with logic for jumping in time for the specific player.
        /// </summary>
        protected virtual void GoToTime(int seconds) { }


        /// <summary>
        /// To be overriden by child class with logic for jumping in time for the specific player.
        /// </summary>
        protected virtual void GoToTime(SMPTE smpte) { }


        /// <summary>
        /// To be overriden by child class with logic for next frame for the specific player.
        /// </summary>
        protected virtual void NextFrame() { }


        /// <summary>
        /// To be overriden in child class with actual method for opening file.
        /// </summary>
        /// <param name="file">File path to video.</param>
        public virtual void OpenVideo(string file) { }


        /// <summary>
        /// To be overriden by child class with pause method of the specific player.
        /// </summary>
        protected virtual void Pause() { }


        /// <summary>
        /// To be overriden by child class with play and pause methods of the specific player.
        /// </summary>
        protected virtual void PlayOrPause() { }


        /// <summary>
        /// To be overriden by child class with play method of the specific player.
        /// </summary>
        protected virtual void Play() { }


        /// <summary>
        /// To be overriden by child class with logic for previous frame for the specific player.
        /// </summary>
        protected virtual void PreviousFrame() { }


        /// <summary>
        /// Updates video window timecode labels.
        /// </summary>
        /// <param name="timecode">Timecode string.</param>
        /// <param name="label">Label to update.</param>
        protected virtual void UpdateLabels(string timecode, TimecodeLabels label) { }


        /// <summary>
        /// Updates video current position.
        /// </summary>
        protected virtual TimeSpan? UpdatePosition(int frameNumber = -1) { return null; }


        /// <summary>
        /// Plays the subtitles for revision.
        /// </summary>
        public virtual void PlaySubtitles(Queue<Loop> loops) { }

        #endregion



        #region METHODS

        /// <summary>
        /// Converts system TimeSpan to SMPTE format string.
        /// </summary>
        /// <param name="timeSpan">TimeSpan objecto to covert.</param>
        /// <param name="frameRate">Frame rate of current video.</param>
        /// <returns>SMPTE formatted string.</returns>
        protected string ConvertTimeSpanToSmpte(TimeSpan timeSpan, double frameRate)
        {
            // Get milliseconds component.
            int milliseconds = timeSpan.Milliseconds;

            // Convert it to frames.
            int frames = Convert.ToInt32(Math.Round((milliseconds * frameRate) / 1000));

            // Return formated string.
            return ($"{timeSpan.Hours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}:{frames:D2}");
        }


        /// <summary>
        /// Changes the appropriate frames for drop frame SMPTE format.
        /// </summary>
        /// <param name="smpte">Loop object to check.</param>
        /// <returns>Frame-dropped smpte.</returns>
        protected SMPTE DropFrames(SMPTE smpte)
        {
            if (smpte.TimecodeTokens[1] % 10 > 0)
            {
                if (smpte.TimecodeTokens[2] == 0)
                {
                    if ((smpte.TimecodeTokens[3] == 0) || (smpte.TimecodeTokens[3] == 1))
                    {
                        smpte.TimecodeTokens[3] = 2;
                    }
                }
            }

            return smpte;
        }


        /// <summary>
        /// Sets initial window position
        /// </summary>
        public void InitialPosition()
        {
            // Get current screen info.
            ScreenInfo screen = new ScreenInfo(this);

            // Set default location of windows.
            DefaultLocation = screen.GetVideoWindowDefaultCoordinates();

            // Set coordinates.
            this.Left = DefaultLocation.X;
            this.Top = DefaultLocation.Y;
        }


        /// <summary>
        /// Sets window default location and shows it.
        /// </summary>
        public void ShowWindow()
        {
            // Set initial position.
            InitialPosition();

            // Show window.
            Show();
        }

        #endregion



        #region COMMANDS

        /// <summary>
        /// CanExecute for Exit command.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Exit_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }


        /// <summary>
        /// Executed for Exit command.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Exit_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }


        /// <summary>
        /// CanExecute for Open command.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Open_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }


        /// <summary>
        /// Executed for Open command.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Movies (*.mov,*.mp4)|*.mov;*.mp4|All files|*.*",
                RestoreDirectory = true
            };

            // The comparison is to "true" because we are using a reference
            // to Microsoft.Win32. If instead we use a reference to System.Windows.Forms
            // it would be neccessary to use System.Windows.Forms.DialogResult.OK.
            if (dialog.ShowDialog() == true)
            {
                // Set the video file in the project file object.
                Settings.ApplicationSettings.Project.VideoFile = dialog.FileName;
                // And open the video.
                OpenVideo(dialog.FileName);

            }
        }


        /// <summary>
        /// Executed for Go To command.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="e"></param>
        protected void GoTo_Executed(object target, ExecutedRoutedEventArgs e)
        {
            GoToTime(new SMPTE(DialogsService.GetSmpte()));
        }


        /// <summary>
        /// CanExecute for Go To command.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GoTo_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = IS_MOVIE_LOADED;
        }

        #endregion



        #region EVENTS

        /// <summary>
        /// Defines the keyboard control for the video window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Window_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (IS_MOVIE_LOADED)
            {
                // First, we must create the frame compensation SMPTE.
                SMPTE frameCompensation = new SMPTE(Settings.ApplicationSettings.FrameCompensation);

                switch (e.Key)
                {



                    #region DUBBING

                    // Reset key.
                    case Key.Escape:
                        
                        IS_CUE_IN_SET = false;

                        CurrentLoop = LastLoop;

                        e.Handled = true;

                        break;

                    // Play/Pause key.
                    case Key.Enter:

                        PlayOrPause();

                        e.Handled = true;

                        break;

                    // Enter time code keys.
                    case Key.Space:
                    case Key.NumPad0:

                        switch (DocumentType)
                        {

                            case DocumentMode.Excel:
                            case DocumentMode.Subtitles:

                                Pause();

                                UpdatePosition();

                                string Smpte = String.Empty;

                                // We must check if we are at the beginning of the video, because if so, we cannot substract
                                // the frame compensation as it would generate an error when trying to substract an earlier loop
                                // from a later one. We must convert the loops to frames and check. We create the SMPTE string.
                                // The we check for the possibility to substract.
                                if (CurrentPosition.ConvertToFrames() > Settings.ApplicationSettings.FrameCompensation)
                                {

                                    if (Settings.ApplicationSettings.VideoEngine == VideoMode.Internal)
                                    {
                                        Smpte = GetSmpteString(CurrentPosition - frameCompensation);
                                    }
                                    else
                                    {
                                        Smpte = GetSmpteString(Settings.ApplicationSettings.FrameCompensation);
                                    }
                                }
                                else
                                {
                                    // If substraction is not possible, we use the original SMPTE.
                                    Smpte = GetSmpteString(CurrentPosition);
                                }

                                UpdateLabels(Smpte, TimecodeLabels.Center);

                                SMPTE currentSMPTE = new SMPTE(Smpte);

                                // Calculate warning.
                                if (LastLoop != null)
                                {
                                    // Create substraction result.
                                    SMPTE result = null;

                                    try
                                    {
                                        result = currentSMPTE - LastLoop;
                                    }
                                    catch
                                    {
                                        // If there was an exception, the 2nd SMPTE ocurrs before the 1st, so we set the warning.
                                        // Insert loop. The method will return false if insertion is canceled.
                                        if (EditorWindow.InsertLoop(Smpte, true))
                                        {
                                            // Set last loop.
                                            LastLoop = currentSMPTE;
                                        }
                                        else
                                        {
                                            CurrentLoop = LastLoop;
                                        }

                                        break;
                                    }

                                    if (result.ConvertToFrames() == 0)
                                    {
                                        if (EditorWindow.InsertLoop(Smpte, true))
                                        {
                                            // Set last loop.
                                            LastLoop = currentSMPTE;
                                        }
                                        else
                                        {
                                            CurrentLoop = LastLoop;
                                        }
                                    }
                                    else
                                    {
                                        if (EditorWindow.InsertLoop(Smpte, false))
                                        {
                                            // Set last loop.
                                            LastLoop = currentSMPTE;
                                        }
                                        else
                                        {
                                            CurrentLoop = LastLoop;
                                        }
                                    }
                                }
                                else
                                {
                                    if (EditorWindow.InsertLoop(Smpte, false))
                                    {
                                        // Set last loop.
                                        LastLoop = currentSMPTE;
                                    }
                                    else
                                    {
                                        CurrentLoop = LastLoop;
                                    }
                                }

                                // Rewind.
                                GoToTime(-Settings.ApplicationSettings.SecondsToRewindVideoAfterLoop);

                                break;

                            case DocumentMode.RTF:

                                if (IS_CUE_IN_SET)
                                {
                                    Pause();

                                    UpdatePosition();

                                    string cueOutString = GetSmpteString(CurrentPosition);

                                    UpdateLabels(cueOutString, TimecodeLabels.Right);

                                    EditorWindow.InsertLoop(CueInString, cueOutString);

                                    IS_CUE_IN_SET = false;

                                    GoToTime(-Settings.ApplicationSettings.SecondsToRewindVideoAfterLoop);
                                }
                                else
                                {
                                    UpdatePosition();

                                    // Check for the possibility to substract.
                                    if (CurrentPosition.ConvertToFrames() > Settings.ApplicationSettings.FrameCompensation)
                                    {
                                        if (Settings.ApplicationSettings.VideoEngine == VideoMode.Internal)
                                        {
                                            CueInString = GetSmpteString(CurrentPosition - new SMPTE(Settings.ApplicationSettings.FrameCompensation));
                                        }
                                        else
                                        {
                                            CueInString = GetSmpteString(Settings.ApplicationSettings.FrameCompensation);
                                        }
                                    }
                                    else
                                    {
                                        CueInString = GetSmpteString(CurrentPosition);
                                    }

                                    UpdateLabels(CueInString, TimecodeLabels.Left);

                                    IS_CUE_IN_SET = true;
                                }

                                break;
                        }

                        e.Handled = true;

                        break;

                    #endregion



                    #region SUBTITLES

                    // Period for subtitles.
                    case Key.Decimal:

                        // Check for document mode SUBTITLES.
                        if (Settings.ApplicationSettings.DocumentType == DocumentMode.Subtitles)
                        {
                            if (!IS_CUE_IN_SET)
                            {
                                // Get actual frames.
                                InFrame = FrameNumber;

                                UpdatePosition();

                                if (CurrentPosition.ConvertToFrames() > Settings.ApplicationSettings.FrameCompensation)
                                {
                                    if (Settings.ApplicationSettings.VideoEngine == VideoMode.Internal)
                                    {
                                        CueInString = GetSmpteString(CurrentPosition - new SMPTE(Settings.ApplicationSettings.FrameCompensation));
                                    }
                                    else
                                    {
                                        CueInString = GetSmpteString(Settings.ApplicationSettings.FrameCompensation);
                                    }
                                }
                                else
                                {
                                    CueInString = GetSmpteString(CurrentPosition);
                                }

                                UpdateLabels(CueInString, TimecodeLabels.Left);

                                IS_CUE_IN_SET = true;

                                Subtitle = new Subtitle();

                            }
                            else
                            {
                                // Get out frame.
                                OutFrame = FrameNumber;

                                UpdatePosition();

                                string cueOutString = GetSmpteString(CurrentPosition);

                                Paragraph currentParagraph, nextParagraph = null;
                                
                                // Check length.
                                if (EditorWindow.Editor != null)
                                {
                                    Settings.ApplicationSettings.IsSubtitle = true;

                                    currentParagraph = EditorWindow.Editor.CaretPosition.Paragraph;

                                    if (currentParagraph != null) currentParagraph.Background = Utilities.CheckSubtitleLength(currentParagraph);

                                    nextParagraph = EditorWindow.GetNextParagraph();

                                    if (nextParagraph != null) nextParagraph.Background = Utilities.CheckSubtitleLength(nextParagraph);
                                }

                                EditorWindow.InsertLoop(CueInString, cueOutString, Subtitle, false, InFrame, OutFrame);



                                // Find next paragraph.
                                TextPointer nextLoop = EditorWindow.FindNextLoop();

                                // Set caret.
                                if (nextLoop != null)
                                {
                                    EditorWindow.Editor.CaretPosition = nextLoop;

                                    // Rectangle corresponding to the coordinates of the selected text.
                                    Rect screenPos = EditorWindow.Editor.Selection.Start.GetCharacterRect(LogicalDirection.Forward);
                                    
                                    // Apply offset.
                                    double offset = screenPos.Top + EditorWindow.Editor.VerticalOffset - Settings.ApplicationSettings.SubtitlesScrollOffset;
                                    
                                    // The offset - half the size of the RichtextBox to keep the selection centered.
                                    EditorWindow.Editor.ScrollToVerticalOffset(offset);
                                }


                                // Set cue in point adding the number of frames specified
                                // in Settings. This will allow to enter the subtitles
                                // continuously, without stopping between them, which would
                                // create the problem of subtitles beginning before the previous
                                // has ended.
                                CueIn = CurrentPosition + new SMPTE(Settings.ApplicationSettings.FramesBetweenSubtitles);

                                // Get the actual frame.
                                InFrame = FrameNumber + Settings.ApplicationSettings.FramesBetweenSubtitles;

                                // Get cue in string.
                                CueInString = GetSmpteString(CueIn, true);

                                // Check for control key to end current subtitle run.
                                if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                                {
                                    // Reset flag.
                                    IS_CUE_IN_SET = false;

                                    // Set flag.
                                    Settings.ApplicationSettings.IsSubtitle = false;
                                }

                                UpdateLabels(CueInString, TimecodeLabels.Left);
                                UpdateLabels(cueOutString, TimecodeLabels.Right);
                            }
                        }

                        e.Handled = true;

                        break;

                    #endregion



                    #region NAVIGATION

                    // Advance one frame.
                    case Key.Right:

                        if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                        {
                            GoToTime(10);
                        }
                        else if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                        {
                            GoToTime(1);
                        }
                        else
                        {
                            NextFrame();
                        }

                        e.Handled = true;

                        break;

                    // Back one frame.
                    case Key.Left:

                        if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                        {
                            GoToTime(-10);
                        }
                        else if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                        {
                            GoToTime(-1);
                        }
                        else
                        {
                            PreviousFrame();
                        }

                        e.Handled = true;

                        break;

                    #endregion



                    #region SUBTITLES FORMAT

                    // Subtitles height.
                    case Key.NumPad1:

                        if (Settings.ApplicationSettings.DocumentType == DocumentMode.Subtitles)
                        {
                            if (Subtitle != null) Subtitle.Line = 1;
                        }

                        e.Handled = true;

                        break;

                    case Key.NumPad2:

                        if (Settings.ApplicationSettings.DocumentType == DocumentMode.Subtitles)
                        {
                            if (Subtitle != null) Subtitle.Line = 2;
                        }

                        e.Handled = true;

                        break;

                    case Key.NumPad3:

                        if (Settings.ApplicationSettings.DocumentType == DocumentMode.Subtitles)
                        {
                            if (Subtitle != null) Subtitle.Line = 3;
                        }

                        e.Handled = true;

                        break;

                    case Key.NumPad4:

                        if (Settings.ApplicationSettings.DocumentType == DocumentMode.Subtitles)
                        {
                            if (Subtitle != null) Subtitle.Line = 4;
                        }

                        e.Handled = true;

                        break;

                    case Key.NumPad5:

                        if (Settings.ApplicationSettings.DocumentType == DocumentMode.Subtitles)
                        {
                            if (Subtitle != null) Subtitle.Line = 5;
                        }

                        e.Handled = true;

                        break;

                    case Key.NumPad6:

                        if (Settings.ApplicationSettings.DocumentType == DocumentMode.Subtitles)
                        {
                            if (Subtitle != null) Subtitle.Line = 6;
                        }

                        e.Handled = true;

                        break;

                    case Key.NumPad7:

                        if (Settings.ApplicationSettings.DocumentType == DocumentMode.Subtitles)
                        {
                            if (Subtitle != null) Subtitle.Line = 7;
                        }

                        e.Handled = true;

                        break;

                    case Key.NumPad8:

                        if (Settings.ApplicationSettings.DocumentType == DocumentMode.Subtitles)
                        {
                            if (Subtitle != null) Subtitle.Line = 8;
                        }

                        e.Handled = true;

                        break;

                    case Key.NumPad9:

                        if (Settings.ApplicationSettings.DocumentType == DocumentMode.Subtitles)
                        {
                            if (Subtitle != null) Subtitle.Line = 9;
                        }

                        e.Handled = true;

                        break;

                    case Key.D0:

                        if (Settings.ApplicationSettings.DocumentType == DocumentMode.Subtitles)
                        {
                            if (Subtitle != null) Subtitle.Line = 0;
                        }

                        e.Handled = true;

                        break;

                    // Subtitles italics.
                    case Key.I:

                        if (Settings.ApplicationSettings.DocumentType == DocumentMode.Subtitles)
                        {
                            if (Subtitle != null) Subtitle.FirstLineItalics = true;
                            if (Subtitle != null) Subtitle.SecondLineItalics = true;
                        }

                        e.Handled = true;

                        break;

                    case Key.D1:

                        if (Settings.ApplicationSettings.DocumentType == DocumentMode.Subtitles)
                        {
                            if (Subtitle != null) Subtitle.FirstLineItalics = true;
                            if (Subtitle != null) Subtitle.SecondLineItalics = false;
                        }

                        e.Handled = true;

                        break;

                    case Key.D2:

                        if (Settings.ApplicationSettings.DocumentType == DocumentMode.Subtitles)
                        {
                            if (Subtitle != null) Subtitle.FirstLineItalics = false;
                            if (Subtitle != null) Subtitle.SecondLineItalics = true;
                        }

                        e.Handled = true;

                        break;

                    case Key.N:

                        if (Settings.ApplicationSettings.DocumentType == DocumentMode.Subtitles)
                        {
                            if (Subtitle != null) Subtitle.FirstLineItalics = false;
                            if (Subtitle != null) Subtitle.SecondLineItalics = false;
                        }

                        e.Handled = true;

                        break;

                    // Subtitles alignment.
                    case Key.C:

                        if (Settings.ApplicationSettings.DocumentType == DocumentMode.Subtitles)
                        {
                            if (Subtitle != null) Subtitle.Alignment = SubtitleAlignment.Center;
                        }

                        e.Handled = true;

                        break;

                    case Key.R:

                        if (Settings.ApplicationSettings.DocumentType == DocumentMode.Subtitles)
                        {
                            if (Subtitle != null) Subtitle.Alignment = SubtitleAlignment.Right;
                        }

                        e.Handled = true;

                        break;

                    case Key.L:

                        if (Settings.ApplicationSettings.DocumentType == DocumentMode.Subtitles)
                        {
                            if (Subtitle != null) Subtitle.Alignment = SubtitleAlignment.Left;
                        }

                        e.Handled = true;

                        break;

                        #endregion
                }
            }
        }


        /// <summary>
        /// Activates the open video window button on video window close.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void VideoWindow_Closing(object sender, EventArgs e)
        {
            TextEditorWindow.OpenVideoWindowButton.Visibility = Visibility.Visible;
        }

        #endregion

    }
}
