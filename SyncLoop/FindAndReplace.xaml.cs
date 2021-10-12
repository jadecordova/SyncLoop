using SyncLoopLibrary;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace SyncLoop
{
    /// <summary>
    /// Interaction logic for FindAndReplace.xaml
    /// </summary>
    public partial class FindAndReplace : Window
    {

        #region FIELDS

        FindAndReplaceManager Manager;

        RichTextBox Editor;

        #endregion

        
        
        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for the find and replace dialog.
        /// </summary>
        /// <param name="editor">RichTextBox to perform the search on.</param>
        /// <param name="initialText">Text to populate the search box initially.</param>
        public FindAndReplace(RichTextBox editor, string initialText)
        {
            InitializeComponent();

            // Subscribe to close event.
            Closed += FindAndReplace_Closed;
            // Create find and replace manager.
            Manager = new FindAndReplaceManager(editor.Document);
            // Set document.
            Editor = editor;
            // Set initial values.
            if (!string.IsNullOrEmpty(initialText))
            {
                searchTextBox.Text = initialText;

                replaceTextBox.Text = initialText;

                replaceTextBox.SelectAll();

                replaceTextBox.Focus();
            }

            // Read position.
            if (Settings.ApplicationSettings.FindWindowHeight != null) this.Height = (double)Settings.ApplicationSettings.FindWindowHeight;

            if (Settings.ApplicationSettings.FindWindowWidth != null) this.Width = (double)Settings.ApplicationSettings.FindWindowWidth;

            if (Settings.ApplicationSettings.FindWindowTop != null) this.Top = (double)Settings.ApplicationSettings.FindWindowTop;

            if (Settings.ApplicationSettings.FindWindowLeft != null) this.Left = (double)Settings.ApplicationSettings.FindWindowLeft;
        }

        #endregion



        #region EVENT HANDLERS

        private void FindAndReplace_Closed(object sender, System.EventArgs e)
        {
            // Save position.
            Settings.ApplicationSettings.FindWindowHeight = this.Height;

            Settings.ApplicationSettings.FindWindowWidth = this.Width;

            Settings.ApplicationSettings.FindWindowTop = this.Top;

            Settings.ApplicationSettings.FindWindowLeft = this.Left;
        }

        private void Find(object sender, RoutedEventArgs e)
        {
            // Create options.
            FindOptions options = GetOptions();
            // Find.
            TextRange selection = Manager.FindNext(searchTextBox.Text, options);
            // Select found text.
            Utilities.SelectText(selection, Editor);
        }

        private void Replace(object sender, RoutedEventArgs e)
        {
            // Create options.
            FindOptions options = GetOptions();
            // Find.
            TextRange selection = Manager.Replace(searchTextBox.Text, replaceTextBox.Text, options);
            // Select found text.
            Utilities.SelectText(selection, Editor);
        }

        private void ReplaceAll(object sender, RoutedEventArgs e)
        {
            // Create options.
            FindOptions options = GetOptions();
            // Find.
            int result = Manager.ReplaceAll(searchTextBox.Text, replaceTextBox.Text, options, null);
            // Inform.
            MessageBox.Show($"{result} instances replaced.", 
                             "SyncLoop", 
                             MessageBoxButton.OK, MessageBoxImage.Information);
        }
        
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #endregion



        #region METHODS


        private FindOptions GetOptions()
        {
            // Create options.
            FindOptions options = FindOptions.None;
            // Case match.
            if (matchCaseCheckBox.IsChecked == true)
            {
                options |= FindOptions.MatchCase;
            }

            // Whole words.
            if (matchWholeWordCheckBox.IsChecked == true)
            {
                options |= FindOptions.MatchWholeWord;
            }

            return options;
        }

        #endregion
    }
}
