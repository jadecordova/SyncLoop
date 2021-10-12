using SyncLoopLibrary;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SyncLoop
{
    /// <summary>
    /// Interaction logic for ProgramInfo.xaml
    /// </summary>
    public partial class ProgramInfoDialog : Window
    {

        #region MEMBERS

        /// <summary>
        /// List of series.
        /// </summary>
        ObservableCollection<Series> series;

        /// <summary>
        /// Program info object to supply info to and from.
        /// </summary>
        ProgramInfo ProgramInfo;

        /// <summary>
        /// Channels list.
        /// </summary>
        ObservableCollection<Channel> Channels;

        /// <summary>
        /// Current series.
        /// </summary>
        Series ProgramSeries;

        /// <summary>
        /// Current channel.
        /// </summary>
        Channel ProgramChannel;

        #endregion



        #region CONSTRUCTORS

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="info">ProgramInfo object with program details.</param>
        /// <param name="channels">Channels list.</param>
        /// <param name="series">Series list.</param>
        public ProgramInfoDialog(ProgramInfo info, ObservableCollection<Channel> channels, ObservableCollection<Series> series)
        {
            InitializeComponent();

            ProgramInfo = info;

            this.series = series;

            DataContext = ProgramInfo;

            ChannelsComboBox.DataContext = channels;

            Channels = channels;

            SeriesComboBox.DataContext = series;

            EpisodeCode.TextChanged += FieldsTextChanged;

            SeriesComboBox.SelectionChanged += SeriesComboBoxSelectionChanged;

            ChannelsComboBox.SelectionChanged += ChannelsComboBoxSelectionChanged;

            ContentRendered += ProgramInfoDialogContentRendered;

            RateBox.ItemsSource = Enum.GetValues(typeof(RateType)).Cast<RateType>();

            // Set sorting.
            SeriesComboBox.Items.SortDescriptions.Add(new SortDescription("NameEnglish", ListSortDirection.Ascending));

            ChannelsComboBox.Items.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));

        }

        #endregion



        #region EVENT HANDLERS

        /// <summary>
        /// Sets and selects the series.
        /// </summary>
        private void SeriesComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SeriesComboBox.SelectedItem != null)
            {
                long channelID = ((Series)SeriesComboBox.SelectedItem).ChannelID;
                // Select channel.
                Channel seriesChannel = ((ObservableCollection<Channel>)ChannelsComboBox.DataContext).Single(x => x.ID == channelID);
                // Select it in combo box.
                ChannelsComboBox.SelectedItem = seriesChannel;
            }

            OKButton.IsEnabled = ValidateFields();
        }

        /// <summary>
        /// Selects channel and validates parameters.
        /// </summary>
        private void ChannelsComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OKButton.IsEnabled = ValidateFields();
        }


        private void OKButtonClick(object sender, RoutedEventArgs e)
        {
            // Set channel.
            ProgramInfo.EpisodeChannel = (Channel)ChannelsComboBox.SelectedItem;
            // Set series.
            ProgramInfo.EpisodeSeries = (Series)SeriesComboBox.SelectedItem;
            // Set rate.
            ProgramInfo.Rate = (RateType)RateBox.SelectedItem;
            
            // Set the rate amount.
            if(Settings.ApplicationSettings.CurrentRates != null)
            {
                switch (ProgramInfo.Rate)
                {
                    case RateType.Normal:

                        ProgramInfo.RateAmount = Settings.ApplicationSettings.CurrentRates.Normal;
                        break;

                    case RateType.Rush:

                        ProgramInfo.RateAmount = Settings.ApplicationSettings.CurrentRates.Rush;
                        break;

                    case RateType.Less_than_48_hours:

                        ProgramInfo.RateAmount = Settings.ApplicationSettings.CurrentRates.LessThan48Hours;
                        break;
                }
            }
            else
            {
                MessageBox.Show("Rates are invalid.",
                                "SyncLoop",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            // Set date due.
            ProgramInfo.DateDue = (DateTime)DateBox.SelectedDate;
            // Set global due date.
            Settings.ApplicationSettings.CurrentDateDue = ProgramInfo.DateDue;

            DialogResult = true;
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        /// <summary>
        /// Validates fields to activate OK button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FieldsTextChanged(object sender, TextChangedEventArgs e)
        {
            OKButton.IsEnabled = ValidateFields();
        }

        /// <summary>
        /// Sets series and date, and focuses the series combo box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProgramInfoDialogContentRendered(object sender, EventArgs e)
        {
            SetSeries();
            // Set date.
            DateBox.SelectedDate = Settings.ApplicationSettings.CurrentDateDue;

            SeriesComboBox.Focus();
        }

        /// <summary>
        /// Load program info from file.
        /// </summary>
        private void LoadProgramInfoButtonClick(object sender, RoutedEventArgs e)
        {
            // Select file.
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Json Files (*.json)|*.json|All files|*.*",

                RestoreDirectory = true
            };

            if(dialog.ShowDialog() == true)
            {
                try
                {
                    ProgramInfo.Load(dialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading info file: {ex.Message}", 
                                     "SyncLoop", 
                                     MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                SetSeries();
            }
        }

        #endregion



        #region METHODS

        /// <summary>
        /// Validates the info needed.
        /// </summary>
        /// <returns>Are all the needed fields filled?</returns>
        private bool ValidateFields()
        {

            bool result = true;
            // Series box should have a valid item selected.
            if (SeriesComboBox.SelectedItem!= null && ((Series)SeriesComboBox.SelectedItem).ID == 0) result = false;
            // Channel box should have a valid item selected.
            if (ChannelsComboBox.SelectedItem!= null && ((Channel)ChannelsComboBox.SelectedItem).ID == 0) result = false;

            // Episode code must be set.
            if (String.IsNullOrEmpty(this.EpisodeCode.Text))
            {
                EpisodeCode.Background = Brushes.CornflowerBlue;

                result = false;
            }
            else
            {
                EpisodeCode.Background = Brushes.White;
            }

            return result;
        }

        /// <summary>
        /// Sets current series and channel.
        /// </summary>
        private void SetSeries()
        {
            // Select series.
            if (ProgramInfo.EpisodeSeries != null)
            {
                ProgramSeries = series.SingleOrDefault(x => x.ID == ProgramInfo.EpisodeSeries.ID);
            }

            if (ProgramSeries != null)
            {
                SeriesComboBox.SelectedItem = ProgramSeries;
            }
            else
            {
                SeriesComboBox.SelectedIndex = 0;
            }

            // Select channel.
            if (ProgramInfo.EpisodeChannel != null)
            {
                ProgramChannel = ((ObservableCollection<Channel>)ChannelsComboBox.DataContext).SingleOrDefault(x => x.ID == ProgramInfo.EpisodeChannel.ID);
            }

            if (ProgramChannel != null)
            {
                ChannelsComboBox.SelectedItem = ProgramChannel;
            }
            else
            {
                ChannelsComboBox.SelectedIndex = 0;
            }
        }

        #endregion



        #region COMMANDS

        /// <summary>
        /// Edits series in database
        /// </summary>
        private void EditSeries_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SeriesEditor editor = new SeriesEditor();
            // Set channel box context.
            editor.ChannelsComboBox.DataContext = Channels;
            // Set series box context.
            editor.SeriesComboBox.DataContext = series;
            
            // Set data context if a series is selected.
            if (SeriesComboBox.SelectedItem != null && SeriesComboBox.SelectedIndex != 0)
            {
                editor.DataContext = SeriesComboBox.SelectedItem;
            }

            if (editor.ShowDialog() == true)
            {
                if (editor.NewSeries != null)
                {
                    // Init ID to negative value to test for success of DB entrye.
                    long newSeriesID = -1;
                    // Insert into DB.
                    newSeriesID = Database.InsertSeries(editor.NewSeries);

                    // Check for success.
                    if (newSeriesID >= 0)
                    {
                        // Set ID returned by DB.
                        editor.NewSeries.ID = newSeriesID;
                        // Add to series list.
                        series.Add(editor.NewSeries);
                        // Select it in box.
                        SeriesComboBox.SelectedItem = editor.NewSeries;
                    }
                }
                // If no series was created, update selected series in DB.
                else
                {
                    // Get selected series.
                    Series selectedSeries = (Series)editor.SeriesComboBox.SelectedItem;
                    // Update it.
                    selectedSeries.ChannelID = ((Channel)editor.ChannelsComboBox.SelectedItem).ID;

                    selectedSeries.NameEnglish = editor.EnglishNameBox.Text;

                    selectedSeries.NameSpanish = editor.SpanishNameBox.Text;

                    // Update it in DB.
                    try
                    {
                        Database.UpdateSeries(selectedSeries);

                        SeriesComboBox.SelectedItem = selectedSeries;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error updating series in database: {ex.Message}", 
                                         "Error", 
                                         MessageBoxButton.OK, MessageBoxImage.Warning);
                    }

                }
            }
        }

        #endregion
    }
}
