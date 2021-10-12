using SyncLoopLibrary;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SyncLoop
{
    /// <summary>
    /// This window allos for series creation and editing.
    /// </summary>
    public partial class SeriesEditor : Window
    {
        #region MEMBERS

        /// <summary>
        /// New series to be created.
        /// </summary>
        public Series NewSeries = null;

        #endregion



        #region CONSTRUCTORS

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SeriesEditor()
        {
            InitializeComponent();

            // Subscribe.
            ContentRendered += SeriesEditorContentRendered;

            SeriesComboBox.SelectionChanged += SeriesComboBoxSelectionChanged;

            ChannelsComboBox.SelectionChanged += ChannelsComboBoxSelectionChanged;

            EnglishNameBox.TextChanged += TextChanged;

            SpanishNameBox.TextChanged += TextChanged;
        }

        #endregion



        #region EVENT HANDLERS

        /// <summary>
        /// Validates fields.
        /// </summary>
        private void ChannelsComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Validate();
        }

        /// <summary>
        /// Sets series and validates fields.
        /// </summary>
        private void SeriesComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SeriesComboBox.SelectedItem != null)
            {
                // Get channel ID.
                long channelID = ((Series)SeriesComboBox.SelectedItem).ChannelID;
                // Get channel.
                Channel seriesChannel = ((ObservableCollection<Channel>)ChannelsComboBox.DataContext).Single(x => x.ID == channelID);
                // Select it in box.
                ChannelsComboBox.SelectedItem = seriesChannel;

                // Set names if not default.
                if (SeriesComboBox.SelectedIndex != 0)
                {
                    EnglishNameBox.Text = ((Series)SeriesComboBox.SelectedItem).NameEnglish;

                    SpanishNameBox.Text = ((Series)SeriesComboBox.SelectedItem).NameSpanish;
                }

                Validate();
            }
        }

        /// <summary>
        /// Selects fields.
        /// </summary>
        private void SeriesEditorContentRendered(object sender, EventArgs e)
        {
            if (DataContext != null)
            {
                SeriesComboBox.SelectedItem = (Series)DataContext;

                ChannelsComboBox.SelectedItem = ((ObservableCollection<Channel>)ChannelsComboBox.DataContext).Single(x => x.ID == ((Series)DataContext).ChannelID);
            }
            else
            {
                // Select first items.
                ChannelsComboBox.SelectedIndex = 0;

                SeriesComboBox.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Saves series.
        /// </summary>
        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            // Check if series has been created and set properties.
            if (NewSeries != null)
            {
                NewSeries.NameEnglish = EnglishNameBox.Text;

                NewSeries.NameSpanish = SpanishNameBox.Text;

                NewSeries.ChannelID = ((Channel)ChannelsComboBox.SelectedItem).ID;
            }

            DialogResult = true;
        }

        /// <summary>
        /// Creates new series.
        /// </summary>
        private void NewButtonClick(object sender, RoutedEventArgs e)
        {
            // Reset boxes.
            ChannelsComboBox.SelectedIndex = 0;

            SeriesComboBox.SelectedIndex = 0;

            EnglishNameBox.Text = String.Empty;

            SpanishNameBox.Text = String.Empty;
            // Disable series box.
            SeriesComboBox.IsEnabled = false;
            // Disable new button.
            NewButton.IsEnabled = false;
            // Create new series object.
            NewSeries = new Series();
        }

        /// <summary>
        /// Validates fields.
        /// </summary>
        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            Validate();
        }

        #endregion



        #region METHODS

        /// <summary>
        /// Validates fields.
        /// </summary>
        private void Validate()
        {
            bool result = true;

            if ((SeriesComboBox.SelectedItem == null ||
                 SeriesComboBox.SelectedIndex == 0) &&
                 NewSeries == null)
            {
                result = false;
            }

            if (ChannelsComboBox.SelectedItem == null ||
                ChannelsComboBox.SelectedIndex == 0)
            {
                result = false;
            }

            if (String.IsNullOrEmpty(EnglishNameBox.Text)) result = false;
            if (String.IsNullOrEmpty(SpanishNameBox.Text)) result = false;

            SaveButton.IsEnabled = result;
        }

        #endregion
    }
}
