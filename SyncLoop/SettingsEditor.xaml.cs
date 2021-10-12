using SyncLoopLibrary;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;

namespace SyncLoop
{
    /// <summary>
    /// Interaction logic for SettingsEditor.xaml
    /// </summary>
    public partial class SettingsEditor : Window
    {

        #region MEMBERS

        /// <summary>
        /// ID of currently selected channel in combo box.
        /// </summary>
        private long CurrentChannelID = -1;

        #endregion



        #region CONSTRUCTORS

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SettingsEditor()
        {
            InitializeComponent();

            // Subscribe.
            ChannelsBox.SelectionChanged += ChannelsBoxSelectionChanged;

            ChannelCodeBox.TextChanged += ChannelCodeBoxTextChanged;
            // Select first item.
            ChannelsBox.SelectedIndex = 0;
            // Get fonts.
            EditorFontList.DataContext = Fonts.SystemFontFamilies;

            ContentFontList.DataContext = Fonts.SystemFontFamilies;

            AdditionalFontList.DataContext = Fonts.SystemFontFamilies;

            HeadersAndFootersFontList.DataContext = Fonts.SystemFontFamilies;
            // Subscribe to rendered content.
            ContentRendered += SettingsEditorContentRendered;
            // Set video engines list source.
            VideoEnginesList.ItemsSource = Enum.GetValues(typeof(VideoMode)).Cast<VideoMode>();
            // Set video engines list source.
            SubtitlesModeList.ItemsSource = Enum.GetValues(typeof(SubtitlesMode)).Cast<SubtitlesMode>();
        }

        #endregion



        #region EVENT HANDLERS

        /// <summary>
        /// Validates info.
        /// </summary>
        private void ChannelCodeBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            Validate();
        }

        /// <summary>
        /// Adds new channel to DB.
        /// </summary>
        private void AddChannelButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                // Create new channel.
                Channel newChannel = new Channel
                {
                    Code = ChannelCodeBox.Text,
                    Name = ChannelsBox.Text
                };
                // Insert it to DB and set returned ID.
                newChannel.ID = Database.InsertChannel(newChannel);
                // Add to channels list.
                ((ObservableCollection<Channel>)ChannelsBox.DataContext).Add(newChannel);
                // Enable remove button.
                RemoveButton.IsEnabled = true;

                System.Windows.MessageBox.Show("Channel Added.", 
                                               "SyncLoop", 
                                               MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error inserting channel in database: {ex.Message}", 
                                                "SyncLoop", 
                                                MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Removes channel from DB.
        /// </summary>
        private void RemoveChannelButtonClick(object sender, RoutedEventArgs e)
        {
            // Get channel to delete.
            Channel channelToDelete = null;

            if (ChannelsBox.SelectedItem != null)
            {

                if (((Channel)ChannelsBox.SelectedItem).ID != 0)
                {
                    channelToDelete = (Channel)ChannelsBox.SelectedItem;
                }

            }

            // Delete it.
            if (channelToDelete != null)
            {
                try
                {
                    Database.DeleteChannel(channelToDelete);
                    // Delete from list.
                    ((ObservableCollection<Channel>)ChannelsBox.DataContext).Remove(channelToDelete);
                    // Clear boxes.
                    ChannelCodeBox.Text = String.Empty;

                    ChannelsBox.SelectedIndex = 0;

                    System.Windows.MessageBox.Show($"Channel deleted.", 
                                                    "SyncLoop", 
                                                    MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show($"Error deleting channel from database: {ex.Message}", 
                                                    "SyncLoop", 
                                                    MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        /// <summary>
        /// Edits channel in DB.
        /// </summary>
        private void EditChannelButtonClick(object sender, RoutedEventArgs e)
        {
            // Get channel to edit.
            Channel channelToUpdate = null;

            if (!String.IsNullOrEmpty(ChannelsBox.Text) && 
                ChannelsBox.SelectedIndex != 0 && 
                !String.IsNullOrEmpty(ChannelCodeBox.Text))
            {
                try
                {
                    channelToUpdate = ((ObservableCollection<Channel>)ChannelsBox.DataContext).Single(x => x.ID == CurrentChannelID);

                    channelToUpdate.Code = ChannelCodeBox.Text;

                    channelToUpdate.Name = ChannelsBox.Text;
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show($"Error selecting channel ID: {ex.Message}", 
                                                    "SyncLoop", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }

            // Delete it.
            if (channelToUpdate != null)
            {
                try
                {
                    Database.UpdateChannel(channelToUpdate);

                    System.Windows.MessageBox.Show($"Channel updated.", 
                                                    "SyncLoop", 
                                                    MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show($"Error updating channel in database: {ex.Message}", 
                                                    "SyncLoop",
                                                    MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        /// <summary>
        /// Get selecte channel.
        /// </summary>
        private void ChannelsBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((Channel)ChannelsBox.SelectedItem != null)
            {
                // Set current channel ID.
                CurrentChannelID = ((Channel)ChannelsBox.SelectedItem).ID;
                // Set code text.
                ChannelCodeBox.Text = ((Channel)ChannelsBox.SelectedItem).Code;
                // Enable delete button.
                RemoveButton.IsEnabled = true;
            }
            else
            {
                // Clear code text.
                ChannelCodeBox.Text = String.Empty;
                // Dissabel delete button.
                RemoveButton.IsEnabled = false;
            }

            Validate();
        }

        /// <summary>
        /// Set up for the window.
        /// </summary>
        private void SettingsEditorContentRendered(object sender, EventArgs e)
        {

            SelectCurrentFont(EditorFontList, ((Settings)this.DataContext).EditorFont);

            SelectCurrentFont(ContentFontList, ((Settings)this.DataContext).ContentFont);

            SelectCurrentFont(AdditionalFontList, ((Settings)this.DataContext).AdditionalFont);

            SelectCurrentFont(HeadersAndFootersFontList, ((Settings)this.DataContext).HeadersAndFootersFont);
            
            // Select document type.
            switch (((Settings)DataContext).DocumentType)
            {
                case DocumentMode.Excel:

                    DocumentType.SelectedIndex = 0;
                    break;

                case DocumentMode.RTF:

                    DocumentType.SelectedIndex = 1;
                    break;

                case DocumentMode.Subtitles:

                    DocumentType.SelectedIndex = 2;
                    break;
            }

            // Select video engine.
            VideoEnginesList.SelectedItem = ((Settings)this.DataContext).VideoEngine;
            // Select subtitles mode.
            SubtitlesModeList.SelectedItem = ((Settings)this.DataContext).SubtitlesMode;

        }

        /// <summary>
        /// Sets values and saves settings.
        /// </summary>
        private void OKButtonClick(object sender, RoutedEventArgs e)
        {

            // Set editor font.
            ((Settings)this.DataContext).EditorFont = ((FontFamily)EditorFontList.SelectedItem).Source;
            // Set content font.
            ((Settings)this.DataContext).ContentFont = ((FontFamily)ContentFontList.SelectedItem).Source;
            // Set additional font.
            ((Settings)this.DataContext).AdditionalFont = ((FontFamily)AdditionalFontList.SelectedItem).Source;
            // Set headers and footers font.
            ((Settings)this.DataContext).HeadersAndFootersFont = ((FontFamily)HeadersAndFootersFontList.SelectedItem).Source;
            // Set video engine.
            ((Settings)this.DataContext).VideoEngine = (VideoMode)VideoEnginesList.SelectedItem;
            // Set document mode.
            ((Settings)this.DataContext).DocumentType = (DocumentMode)DocumentType.SelectedIndex;
            // Set subtitlse mode.
            ((Settings)this.DataContext).SubtitlesMode = (SubtitlesMode)SubtitlesModeList.SelectedItem;

            // Save settings.
            ((Settings)this.DataContext).SaveSettings();

            // Close.
            DialogResult = true;
        }

        /// <summary>
        /// Shows file dialog to select data folder.
        /// </summary>
        private void SelectDataFolderButton_Click(object sender, RoutedEventArgs e)
        {
            // Prompt user to select a data folder.
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            // Set title.
            dialog.Description = "Select data folder.";

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Settings.ApplicationSettings.Folders["Data"] = dialog.SelectedPath;
            }

        }

        #endregion



        #region METHODS

        /// <summary>
        /// Selects font.
        /// </summary>
        private void SelectCurrentFont(System.Windows.Controls.ComboBox list, string appSettingValue)
        {
            // Select current font.
            foreach (FontFamily item in list.Items)
            {
                if (item.Source == appSettingValue)
                {
                    list.SelectedIndex = list.Items.IndexOf(item);
                }
            }
        }

        /// <summary>
        /// Validates fields.
        /// </summary>
        private void Validate()
        {
            RemoveButton.IsEnabled = ((Channel)ChannelsBox.SelectedItem != null && ChannelsBox.SelectedIndex != 0);

            EditButton.IsEnabled = !String.IsNullOrEmpty(ChannelsBox.Text) && ChannelsBox.SelectedIndex != 0 && !String.IsNullOrEmpty(ChannelCodeBox.Text);
        }

        #endregion

    }
}
