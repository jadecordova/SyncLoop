using SyncLoopLibrary;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SyncLoop
{
    /// <summary>
    /// Interaction logic for Errors.xaml
    /// </summary>
    public partial class Errors : Window
    {

        #region PROPERTIES

        /// <summary>
        /// Reference to text editor.
        /// </summary>
        public RichTextBox Editor { get; set; }

        #endregion



        #region CONSTRUCTORS

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Errors()
        {
            InitializeComponent();

            // Subscribe to double click.
            ErrorsList.MouseDoubleClick += ErrorsList_MouseDoubleClick;
            // Subscribe to close.
            this.Closed += Errors_Closed;

            // Read position.
            if (Settings.ApplicationSettings.ErrorWindowLeft != null) this.Left = (double)Settings.ApplicationSettings.ErrorWindowLeft;

            if (Settings.ApplicationSettings.ErrorWindowTop != null) this.Top = (double)Settings.ApplicationSettings.ErrorWindowTop;

            if (Settings.ApplicationSettings.ErrorWindowWidth != null) this.Width = (double)Settings.ApplicationSettings.ErrorWindowWidth;

            if (Settings.ApplicationSettings.ErrorWindowHeight != null) this.Height = (double)Settings.ApplicationSettings.ErrorWindowHeight;
        }

        #endregion



        #region EVENT HANDLERS

        private void Errors_Closed(object sender, System.EventArgs e)
        {
            Settings.ApplicationSettings.ErrorWindowLeft = this.Left;

            Settings.ApplicationSettings.ErrorWindowTop = this.Top;

            Settings.ApplicationSettings.ErrorWindowWidth = this.Width;

            Settings.ApplicationSettings.ErrorWindowHeight = this.Height;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ErrorsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Create find manager.
            FindAndReplaceManager finder = new FindAndReplaceManager(Editor.Document);
            // Find.
            Utilities.SelectText(finder.FindNext(ErrorsList.SelectedItem.ToString(), FindOptions.MatchCase), Editor);
            // Focus.
            Editor.Focus();
        }

        #endregion
    }
}
