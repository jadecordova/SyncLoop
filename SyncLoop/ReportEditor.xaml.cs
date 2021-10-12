using SyncLoopLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using Path = System.IO.Path;

namespace SyncLoop
{

    /// <summary>
    /// This window presents the financial data for the current period and allows for modification.
    /// </summary>
    public partial class ReportEditor : Window
    {
        #region FIELDS

        // We must create a list containing lists of programs sorted by channel.
        ObservableCollection<ObservableCollection<ProgramInfo>> ProgramsByChannel = new ObservableCollection<ObservableCollection<ProgramInfo>>();
        // Also, a list of ChannelReport objects. This is a user control.
        List<ChannelReport> ReportGrids = new List<ChannelReport>();
        // Collection of channels.
        ObservableCollection<Channel> Channels;
        // Collection of series.
        ObservableCollection<Series> SeriesList;
        // Chart data.
        ChartData Data = new ChartData();

        #endregion



        #region CONSTRUCTORS

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="channels">List of channels.</param>
        /// <param name="series">Listo of series.</param>
        /// <param name="period">Current period.</param>
        public ReportEditor(ObservableCollection<Channel> channels, ObservableCollection<SyncLoopLibrary.Series> series, Period period)
        {
            InitializeComponent();

            // Save parameters.
            Channels = channels;

            SeriesList = series;


            // We call the method to populate the grid.
            PopulateGrid(channels, series, period);
            // Get chart data from DB.
            Data = Database.GetChartData();
            // Set chart data context.
            Chart.DataContext = Data;
            // Subscribe.
            ContentRendered += ReportEditor_ContentRendered;
            // Reset general total.
            Settings.ApplicationSettings.GeneralTotal = 0;
        }

        #endregion



        #region EVENT HANDLERS

        /// <summary>
        /// Calculates total and sets date boxes to current date.
        /// </summary>
        private void ReportEditor_ContentRendered(object sender, EventArgs e)
        {
            // Calculate total.
            CalculateTotal();

            // We set the date boxes to the curren month and year.
            MonthBox.Text = DateTime.Now.ToString("MM");

            YearBox.Text = DateTime.Now.ToString("yyyy");
        }


        /// <summary>
        /// Updates current rates and selected programs in DB.
        /// Recalculates total.
        /// </summary>
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            // Create a flag to know if a program is selected.
            bool PROGRAM_SELECTED = false;

            // Update rates.
            Rates newRates = UpdateRates();

            foreach(ChannelReport report in ReportGrids)
            {
                // Now we iterate through each report grid to find out
                // if there are any programs selected which need to be updated.
                if (report.ReportGrid.SelectedItems.Count != 0)
                {
                    PROGRAM_SELECTED = true;

                    foreach(var program in report.ReportGrid.SelectedItems)
                    {
                        // Get current program.
                        ProgramInfo currenProgram = (ProgramInfo)program;
                        // Update its rates.
                        currenProgram.UpdateProgram(newRates); 
                    }

                    // And recalculate the totals.
                    report.GetTotals();
                }
            }

            CalculateTotal();

            if (!PROGRAM_SELECTED)
            {
                MessageBox.Show("Please, select programs to update.", 
                                "SyncLoop", 
                                MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }


        /// <summary>
        /// Deletes selected programs from DB.
        /// Recalculates total.
        /// </summary>
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

            if(MessageBox.Show("Are you sure you want to delete the selected programs?",
                            "SyncLoop",
                            MessageBoxButton.OKCancel,
                            MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                // Create a flag to know if a program is selected.
                bool PROGRAM_SELECTED = false;

                List<ProgramInfo> programsToRemove = new List<ProgramInfo>();

                foreach (ChannelReport report in ReportGrids)
                {
                    // Now we iterate through each report grid to find out
                    // if there are any programs selected which need to be updated.
                    if (report.ReportGrid.SelectedItems.Count != 0)
                    {
                        PROGRAM_SELECTED = true;

                        foreach (var program in report.ReportGrid.SelectedItems)
                        {

                            // Get current program.
                            ProgramInfo currenProgram = (ProgramInfo)program;
                            // Update its rates.
                            Database.DeleteProgram(currenProgram.ID);
                            // Add to removal list. We cannot remove it directly because it would modify the collection
                            // during an iteration.
                            programsToRemove.Add(currenProgram);

                        }
                        // Delete from list.
                        foreach(var program in programsToRemove)
                        {
                            report.Programs.Remove(program);
                        }
                        // And recalculate the totals.
                        report.GetTotals();
                    }
                }

                CalculateTotal();

                if (!PROGRAM_SELECTED)
                {
                    MessageBox.Show("Please, select programs to delete.", 
                                    "SyncLoop", 
                                    MessageBoxButton.OK, MessageBoxImage.Hand);
                }
            }
        }


        /// <summary>
        /// Saves rates to DB.
        /// </summary>
        private void SaveRatesButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateRates();

            Settings.ApplicationSettings.SaveSettings();
        }


        /// <summary>
        /// Creates report in Excel format.
        /// </summary>
        private void CreateReportButton_Click(object sender, RoutedEventArgs e)
        {
            // First, we create the tuple to store the period data.
            Tuple<int, decimal> periodData = null;

            // Get period data from database.
            // This includes number of programs and total amount.
            if(Settings.ApplicationSettings.CurrentPeriod != null)
            {
                periodData = Database.GetPeriodData(Settings.ApplicationSettings.CurrentPeriod.ID);
            }
            
            // Get current month and year.
            if(int.TryParse(MonthBox.Text, out int month) && int.TryParse(YearBox.Text, out int year))
            {
                // If valid, prompt user to enter dollar value.
                InputDialog dialog = new InputDialog();
                // Set text.
                dialog.LabelText.Text = "Please, enter current U.S. dollar value.";

                if(dialog.ShowDialog() == true)
                {
                    // If dollar is valid...
                    if(decimal.TryParse(dialog.UserInput.Text, out decimal dollar))
                    {
                        // If we actually got data from database...
                        if(periodData != null)
                        {
                            long id = Database.InsertPeriodData(month, year, periodData.Item2, periodData.Item1, dollar);
                            // Test for success.
                            if(id > 0)
                            {
                                MessageBox.Show("Period data recorded.", 
                                                "SyncLoop", 
                                                MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                        else
                        {
                            MessageBox.Show($"Invalid period data from database. No data was written to database.", 
                                             "SyncLoop", 
                                             MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Invalid dollar value. No data was written to database.", 
                                         "SyncLoop", 
                                         MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    MessageBox.Show($"Dollar value is required. No data was written to database.", 
                                     "SyncLoop", 
                                     MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show($"Month or year values are invalid", 
                                 "SyncLoop", 
                                 MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            // Create the Excel report.
            string report = ExcelUtilities.CreateExcelReport(new Report()
                            {
                                Translator = Settings.ApplicationSettings.TranslatorName,
                                Month = int.Parse(MonthBox.Text),
                                Year = int.Parse(YearBox.Text),
                                IVA = decimal.Parse(IvaBox.Text),
                                Reports = ReportGrids
                            });

            // Create the folder for the current month.
            string folder = Path.Combine(Settings.ApplicationSettings.Folders["Reports"], $"{YearBox.Text}-{MonthBox.Text}");
            
            // Create directory.
            if (!Directory.Exists(folder))
            {
                try
                {
                    Directory.CreateDirectory(folder);
                }
                catch(Exception ex)
                {
                    MessageBox.Show($"Error creating report folder: {ex.Message}", 
                                     "SyncLoop", 
                                     MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }

            // Create the file name.
            string fileName = Path.Combine(folder, $"Reporte de Trabajo - Glyphos, C. A. - {MonthBox.Text}-{YearBox.Text}.xls");

            try
            {
                using (StreamWriter sw = new StreamWriter(fileName, false, Encoding.UTF8))
                {
                    sw.Write(report);
                    // If we made it here,
                    // we inform success.
                    MessageBox.Show($"Report for {MonthBox.Text}-{YearBox.Text} was successfully created.", 
                                     "SyncLoop", 
                                     MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception WriteReport)
            {
                MessageBox.Show($"There was en error writing the Excel report file: {WriteReport.Message}", 
                                 "SyncLoop", 
                                 MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }


        /// <summary>
        /// Generates invoices SVG files for printing.
        /// </summary>
        private void GenerateInvoicesButton_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Invoice> invoices = new ObservableCollection<Invoice>();

            foreach(ChannelReport report in ReportGrids)
            {
                invoices.Add(new Invoice()
                {
                    Channel = report.Channel,
                    Subtotal = report.Subtotal,
                    IVA = report.IVA,
                    IvaAmount = report.Subtotal * (report.IVA / 100),
                    Total = report.Total
                });
            }

            InvoicesEditor editor = new InvoicesEditor()
            {
                DataContext = invoices
            };

            editor.ShowDialog();

        }


        /// <summary>
        /// Creates new period.
        /// </summary>
        private void NewPeriodButton_Click(object sender, RoutedEventArgs e)
        {
            PeriodEditor editor = new PeriodEditor();
            // Use current date by default.
            editor.StartDateBox.SelectedDate = DateTime.Now;
            // Declare period object.
            Period newPeriod = null;

            if (editor.ShowDialog() == true)
            {
                newPeriod = new Period();

                if (editor.StartDateBox.SelectedDate != null)
                {
                    newPeriod.StartDate = (DateTime)editor.StartDateBox.SelectedDate;
                }
                else
                {
                    newPeriod.StartDate = DateTime.Now;
                }

                if (editor.EndDateBox.SelectedDate != null)
                {
                    newPeriod.EndDate = (DateTime)editor.EndDateBox.SelectedDate;
                }

                // Insert period into DB.
                Database.InsertPeriod(newPeriod);
                // Set new period in settings.
                Settings.ApplicationSettings.CurrentPeriod = newPeriod;
                // Reset the grid.
                PopulateGrid(Channels, SeriesList, newPeriod);
            }
        }

        #endregion



        #region METHODS

        /// <summary>
        /// Populates the report grid with the period's data.
        /// </summary>
        /// <param name="channels">Collection of channels.</param>
        /// <param name="series">Collection of series.</param>
        /// <param name="period">Period to use.</param>
        private void PopulateGrid(ObservableCollection<Channel> channels, ObservableCollection<Series> series, Period period)
        {
            // First,we clear the grid.
            ReportGrids.Clear();
            // And the panel.
            GridsPanel.Children.Clear();

            // Now we iterate through the channels to get
            // the programs for this period.
            foreach (Channel channel in channels)
            {
                ProgramsByChannel.Add(Database.GetPrograms(channel.ID, channels, series, period));


                // CREATE GRID FOR EACH CHANNEL IF NOT NULL.
                if (ProgramsByChannel.Last() != null)
                {
                    // FIND DATAGRID.
                    ReportGrids.Add(new ChannelReport(channels, series, ProgramsByChannel.Last()));
                    // SET REPORT GRID CHANNEL.
                    ReportGrids.Last().Channel = channel;

                    ReportGrids.Last().IVA = 16;
                    // ADD TO STACK PANEL OF REPORT TAB.
                    GridsPanel.Children.Add(ReportGrids.Last());

                    // Set sorting.
                    ReportGrids.Last().ReportGrid.Items.SortDescriptions.Add(new SortDescription("DateDelivered", ListSortDirection.Ascending));

                }
            }
        }


        /// <summary>
        /// Calculates the total amount earned.
        /// </summary>
        private void CalculateTotal()
        {

            Settings.ApplicationSettings.GeneralTotal = 0;

            foreach (ChannelReport report in ReportGrids)
            {
                // Update general total.
                Settings.ApplicationSettings.GeneralTotal += report.Subtotal;
            }

            TotalBox.Text = Settings.ApplicationSettings.GeneralTotal.ToString("N");
        }

        /// <summary>
        /// Updates current rates.
        /// </summary>
        public Rates UpdateRates()
        {
            // We declare values for the new rates.
            decimal normal, rush, lessThan48hours, iva = 0;

            // And try to get those values from the text boxes.
            try
            {
                normal = decimal.Parse(NormalBox.Text);
            }
            catch
            {
                MessageBox.Show("Invalid normal rate.", 
                                "SyncLoop", 
                                MessageBoxButton.OK, MessageBoxImage.Warning);

                NormalBox.SelectAll();

                NormalBox.Focus();

                return null;
            }

            try
            {
                rush = decimal.Parse(RushBox.Text);
            }
            catch
            {
                MessageBox.Show("Invalid rush rate.", 
                                "SyncLoop", 
                                MessageBoxButton.OK, MessageBoxImage.Warning);

                RushBox.SelectAll();

                RushBox.Focus();

                return null;
            }

            try
            {
                lessThan48hours = decimal.Parse(LessThan48HoursBox.Text);
            }
            catch
            {
                MessageBox.Show("Invalid less than 48 hours rate.", 
                                "SyncLoop", 
                                MessageBoxButton.OK, MessageBoxImage.Warning);

                LessThan48HoursBox.SelectAll();

                LessThan48HoursBox.Focus();

                return null;
            }

            try
            {
                iva = decimal.Parse(IvaBox.Text);
            }
            catch
            {
                MessageBox.Show("Invalid IVA rate.", 
                                "SyncLoop", 
                                MessageBoxButton.OK, MessageBoxImage.Warning);

                IvaBox.SelectAll();

                IvaBox.Focus();

                return null;
            }

            // If all values are legal, we create a new Rates object.
            Rates newRates = new Rates()
            {
                Normal = normal,
                Rush = rush,
                LessThan48Hours = lessThan48hours,
                IVA = iva
            };

            // Update app settings.
            Settings.ApplicationSettings.CurrentRates = newRates;

            return newRates;
        }

        #endregion
    }
}
