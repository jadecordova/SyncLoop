using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SyncLoopLibrary.Commands
{
    /// <summary>
    /// Menu commands.
    /// </summary>
    public static class SyncLoopCommands
    {
        /// <summary>
        /// Exit command.
        /// Alt+F4
        /// </summary>
        public static readonly RoutedUICommand Exit = new RoutedUICommand
            (
                "_Exit",
                "Exit",
                typeof(SyncLoopCommands),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.F4, ModifierKeys.Alt)
                }
            );

        /// <summary>
        /// Go to command.
        /// Ctrl+G.
        /// </summary>
        public static readonly RoutedUICommand GoTo = new RoutedUICommand
           (
               "_Go to...",
               "GoTo",
               typeof(SyncLoopCommands),
               new InputGestureCollection()
               {
                    new KeyGesture(Key.G, ModifierKeys.Control)
               }
           );

        /// <summary>
        /// Edit character command.
        /// Ctlr+Shift+E.
        /// </summary>
        public static readonly RoutedUICommand EditCharacter = new RoutedUICommand
           (
               "_Edit character...",
               "EditCharacter",
               typeof(SyncLoopCommands),
               new InputGestureCollection()
               {
                    new KeyGesture(Key.E, ModifierKeys.Control | ModifierKeys.Shift)
               }
           );

        /// <summary>
        /// Settings command.
        /// Ctrl+Shift+S.
        /// </summary>
        public static readonly RoutedUICommand ApplicationSeetings = new RoutedUICommand
           (
               "_Settings...",
               "ApplicationSeetings",
               typeof(SyncLoopCommands),
               new InputGestureCollection()
               {
                    new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Shift)
               }
           );

        /// <summary>
        /// Program info command.
        /// Ctrl+Shift+P.
        /// </summary>
        public static readonly RoutedUICommand ShowProgramInfo = new RoutedUICommand
           (
               "_Program info...",
               "ApplicationSeetings",
               typeof(SyncLoopCommands),
               new InputGestureCollection()
               {
                    new KeyGesture(Key.P, ModifierKeys.Control | ModifierKeys.Shift)
               }
           );

        /// <summary>
        /// Scroll to line command.
        /// Ctrl+Alt+Down.
        /// </summary>
        public static readonly RoutedUICommand ScrollToLine = new RoutedUICommand
           (
               "Scroll to current l_ine",
               "ScrollToLine",
               typeof(SyncLoopCommands),
               new InputGestureCollection()
               {
                    new KeyGesture(Key.Down, ModifierKeys.Control | ModifierKeys.Alt)
               }
           );

        /// <summary>
        /// Generate proof read file command.
        /// Ctrl+D.
        /// </summary>
        public static readonly RoutedUICommand GenerateProofReadFile = new RoutedUICommand
           (
               "_Generate proof read file",
               "GenerateProofReadFile",
               typeof(SyncLoopCommands),
               new InputGestureCollection()
               {
                    new KeyGesture(Key.D, ModifierKeys.Control)
               }
           );

        /// <summary>
        /// Generate RTF document command.
        /// Ctrl+Alt+R.
        /// </summary>
        public static readonly RoutedUICommand GenerateRTFDocument = new RoutedUICommand
           (
               "Generate _RTF document",
               "GenerateRTFDocument",
               typeof(SyncLoopCommands),
               new InputGestureCollection()
               {
                    new KeyGesture(Key.R, ModifierKeys.Control| ModifierKeys.Alt)
               }
           );

        /// <summary>
        /// Generate Excel document command.
        /// Ctrl+Alt+E.
        /// </summary>
        public static readonly RoutedUICommand GenerateExcelDocument = new RoutedUICommand
           (
               "Generate _Excel document",
               "GenerateExcelDocument",
               typeof(SyncLoopCommands),
               new InputGestureCollection()
               {
                    new KeyGesture(Key.E, ModifierKeys.Control| ModifierKeys.Alt)
               }
           );

        /// <summary>
        /// Generate subtitles documents command.
        /// Ctrl+Alt+T.
        /// </summary>
        public static readonly RoutedUICommand GenerateSubtitlesDocuments = new RoutedUICommand
           (
               "Generate _subtitles document",
               "GenerateExcelDoGenerateSubtitlesDocumentscument",
               typeof(SyncLoopCommands),
               new InputGestureCollection()
               {
                    new KeyGesture(Key.T, ModifierKeys.Control| ModifierKeys.Alt)
               }
           );

        /// <summary>
        /// Load characters.
        /// Ctrl+Alt+L.
        /// </summary>
        public static readonly RoutedUICommand LoadCharacters = new RoutedUICommand
           (
               "_Load characters...",
               "LoadCharacters",
               typeof(SyncLoopCommands),
               new InputGestureCollection()
               {
                    new KeyGesture(Key.L, ModifierKeys.Control | ModifierKeys.Alt)
               }
           );

        /// <summary>
        /// Read characters from editor command.
        /// Ctrl+Shift+A.
        /// </summary>
        public static readonly RoutedUICommand ReadCharacters = new RoutedUICommand
           (
               "_Read characters from document",
               "ReadCharacters",
               typeof(SyncLoopCommands),
               new InputGestureCollection()
               {
                    new KeyGesture(Key.A, ModifierKeys.Control | ModifierKeys.Shift)
               }
           );

        /// <summary>
        /// Convert units command.
        /// Ctrl+Alt+C.
        /// </summary>
        public static readonly RoutedUICommand ConvertUnits = new RoutedUICommand
           (
               "Convert _units",
               "ConvertUnits",
               typeof(SyncLoopCommands),
               new InputGestureCollection()
               {
                    new KeyGesture(Key.C, ModifierKeys.Control | ModifierKeys.Alt)
               }
           );

        /// <summary>
        /// Find and replace command.
        /// Ctrl+F.
        /// </summary>
        public static readonly RoutedUICommand FindAndReplace = new RoutedUICommand
           (
               "_Find and replace...",
               "FindAndReplace",
               typeof(SyncLoopCommands),
               new InputGestureCollection()
               {
                    new KeyGesture(Key.F, ModifierKeys.Control)
               }
           );

        /// <summary>
        /// Offset loops command.
        /// Ctrl+Shift+O.
        /// </summary>
        public static readonly RoutedUICommand OffsetLoops = new RoutedUICommand
           (
               "_Offset loops",
               "OffsetLoops",
               typeof(SyncLoopCommands),
               new InputGestureCollection()
               {
                    new KeyGesture(Key.O, ModifierKeys.Control | ModifierKeys.Shift)
               }
           );

        /// <summary>
        /// Write character title command.
        /// Alt+A.
        /// </summary>
        public static readonly RoutedUICommand WriteCharacterTitle = new RoutedUICommand
           (
               "_Write character title...",
               "WriteCharacterTitle",
               typeof(SyncLoopCommands),
               new InputGestureCollection()
               {
                    new KeyGesture(Key.A, ModifierKeys.Alt)
               }
           );

        /// <summary>
        /// Edit shorcuts command.
        /// Alt+S.
        /// </summary>
        public static readonly RoutedUICommand EditShortcuts = new RoutedUICommand
           (
               "Edit shor_tcuts...",
               "EditShortcuts",
               typeof(SyncLoopCommands),
               new InputGestureCollection()
               {
                    new KeyGesture(Key.S, ModifierKeys.Alt)
               }
           );

        /// <summary>
        /// Shorcut command.
        /// </summary>
        public static readonly RoutedUICommand Shortcut1 = new RoutedUICommand
           (
               "Shortcut _1",
               "Shortcut1",
               typeof(SyncLoopCommands),
               new InputGestureCollection()
               {
                    new KeyGesture(Key.F5)
               }
           );

        /// <summary>
        /// Shorcut command.
        /// </summary>
        public static readonly RoutedUICommand Shortcut2 = new RoutedUICommand
           (
               "Shortcut _2",
               "Shortcut2",
               typeof(SyncLoopCommands),
               new InputGestureCollection()
               {
                    new KeyGesture(Key.F6)
               }
           );

        /// <summary>
        /// Shorcut command.
        /// </summary>
        public static readonly RoutedUICommand Shortcut3 = new RoutedUICommand
           (
               "Shortcut _3",
               "Shortcut3",
               typeof(SyncLoopCommands),
               new InputGestureCollection()
               {
                    new KeyGesture(Key.F7)
               }
           );

        /// <summary>
        /// Shorcut command.
        /// </summary>
        public static readonly RoutedUICommand Shortcut4 = new RoutedUICommand
           (
               "Shortcut _4",
               "Shortcut4",
               typeof(SyncLoopCommands),
               new InputGestureCollection()
               {
                    new KeyGesture(Key.F8)
               }
           );

        /// <summary>
        /// Shorcut command.
        /// </summary>
        public static readonly RoutedUICommand Shortcut5 = new RoutedUICommand
           (
               "Shortcut _5",
               "Shortcut5",
               typeof(SyncLoopCommands),
               new InputGestureCollection()
               {
                    new KeyGesture(Key.F9)
               }
           );

        /// <summary>
        /// Shorcut command.
        /// </summary>
        public static readonly RoutedUICommand Shortcut6 = new RoutedUICommand
           (
               "Shortcut _6",
               "Shortcut6",
               typeof(SyncLoopCommands),
               new InputGestureCollection()
               {
                    new KeyGesture(Key.F10)
               }
           );

        /// <summary>
        /// Shorcut command.
        /// </summary>
        public static readonly RoutedUICommand Shortcut7 = new RoutedUICommand
           (
               "Shortcut _7",
               "Shortcut7",
               typeof(SyncLoopCommands),
               new InputGestureCollection()
               {
                    new KeyGesture(Key.F11)
               }
           );

        /// <summary>
        /// Shorcut command.
        /// </summary>
        public static readonly RoutedUICommand Shortcut8 = new RoutedUICommand
           (
               "Shortcut _8",
               "Shortcut8",
               typeof(SyncLoopCommands),
               new InputGestureCollection()
               {
                    new KeyGesture(Key.F12)
               }
           );

        /// <summary>
        /// Edit series command.
        /// Ctrl+Alt+S.
        /// </summary>
        public static readonly RoutedUICommand EditSeries = new RoutedUICommand
           (
               "Edit series...",
               "EditSeries",
               typeof(SyncLoopCommands),
               new InputGestureCollection()
               {
                    new KeyGesture(Key.S, ModifierKeys.Control  | ModifierKeys.Alt)
               }
           );

        /// <summary>
        /// Report editor command.
        /// Alt+R.
        /// </summary>
        public static readonly RoutedUICommand OpenReportEditor = new RoutedUICommand
           (
               "Open report editor...",
               "OpenReportEditor",
               typeof(SyncLoopCommands),
               new InputGestureCollection()
               {
                    new KeyGesture(Key.R, ModifierKeys.Alt)
               }
           );

        /// <summary>
        /// Upper case command.
        /// Alt+Up.
        /// </summary>
        public static readonly RoutedUICommand UpperCase = new RoutedUICommand
           (
               "Upper case",
               "UpperCase",
               typeof(SyncLoopCommands),
               new InputGestureCollection()
               {
                    new KeyGesture(Key.Up, ModifierKeys.Alt)
               }
           );

        /// <summary>
        /// Lower case command.
        /// Alt+Down.
        /// </summary>
        public static readonly RoutedUICommand LowerCase = new RoutedUICommand
           (
               "Lower case",
               "UpperCase",
               typeof(SyncLoopCommands),
               new InputGestureCollection()
               {
                    new KeyGesture(Key.Down, ModifierKeys.Alt)
               }
           );

        /// <summary>
        /// Play subtitles command.
        /// Alt+P.
        /// </summary>
        public static readonly RoutedUICommand PlaySubtitles = new RoutedUICommand
           (
               "Play subtitles",
               "PlaySubtitles",
               typeof(SyncLoopCommands),
               new InputGestureCollection()
               {
                    new KeyGesture(Key.P, ModifierKeys.Alt)
               }
           );

        /// <summary>
        /// Check length command.
        /// Alt+Enter.
        /// </summary>
        public static readonly RoutedUICommand CheckLength = new RoutedUICommand
           (
               "Check paragraph length",
               "CheckLength",
               typeof(SyncLoopCommands),
               new InputGestureCollection()
               {
                    new KeyGesture(Key.Enter, ModifierKeys.Alt)
               }
           );

    }
}
