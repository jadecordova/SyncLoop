using SyncLoopLibrary;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Windows;

namespace SyncLoop
{
    /// <summary>
    /// The invoice editor window allows for checking and editing the invoices prior to printing.
    /// </summary>
    public partial class InvoicesEditor : Window
    {
        #region FIELDS

        // NumberFormatInfo associated with the es-VE culture.
        NumberFormatInfo FormatInfo = new CultureInfo("es-VE", false).NumberFormat;

        #endregion



        #region CONSTRUCTOR

        /// <summary>
        /// Default constructor.
        /// </summary>
        public InvoicesEditor()
        {
            InitializeComponent();

            Loaded += InvoicesEditor_Loaded;
        }

        #endregion



        #region EVENT HANDLERS

        /// <summary>
        /// Populate date boxes with current date.
        /// </summary>
        private void InvoicesEditor_Loaded(object sender, RoutedEventArgs e)
        {
            DayBox.Text = DateTime.Now.ToString("dd");

            MonthBox.Text = DateTime.Now.ToString("MM");

            YearBox.Text = DateTime.Now.ToString("yyy");

            TemplateBox.Text = Settings.ApplicationSettings.SVGTemplate;
        }

        /// <summary>
        /// Loads invoice template from file.
        /// It also sets the settings variable when the user selects another template.
        /// </summary>
        private void LoadTemplateButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "SVG File (*.svg)|*.svg|All files|*.*",

                RestoreDirectory = true
            };

            if (dialog.ShowDialog() == true)
            {
                TemplateBox.Text = dialog.FileName;

                Settings.ApplicationSettings.SVGTemplate = dialog.FileName;
            }
        }

        /// <summary>
        /// Generates the invoice files in SVG format.
        /// </summary>
        private async void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            // Firt, we open an read the template file set in the text field.
            string template = ReadTemplate(TemplateBox.Text);

            // Next, we generate the invoices.
            foreach (Invoice invoice in (ObservableCollection<Invoice>)InvoicesGrid.DataContext)
            {
                string t = String.Copy(template);
                // Replace day.
                t = t.Replace("%d%", DayBox.Text);
                // Replace month.
                t = t.Replace("%m%", MonthBox.Text);
                // Replace year.
                t = t.Replace("%y%", YearBox.Text);
                // Replace channel.
                t = t.Replace("%channel%", invoice.Channel.Name.Replace("&", "and"));
                // Replace subtotal.
                t = t.Replace("%subtotal%", invoice.Subtotal.ToString("N", FormatInfo));
                // Replace IVA rate.
                t = t.Replace("%ivarate%", invoice.IVA.ToString("N", FormatInfo));
                // Replace IVA amount.
                t = t.Replace("%ivaamount%", invoice.IvaAmount.ToString("N", FormatInfo));
                // Replace total.
                t = t.Replace("%total%", invoice.Total.ToString("N", FormatInfo));
                // Create month folder.
                string folder = System.IO.Path.Combine(Settings.ApplicationSettings.Folders["Invoices"], $"{YearBox.Text}-{MonthBox.Text}");

                // Try to create folder.
                if (!Directory.Exists(folder))
                {
                    try
                    {
                        Directory.CreateDirectory(folder);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"There was an error creating the invoices folder: {ex.Message}", 
                                         "SyncLoop", 
                                         MessageBoxButton.OK, MessageBoxImage.Warning);

                        return;
                    }
                }

                // Create invoice name.
                string fileName = System.IO.Path.Combine(folder, $"{invoice.Channel.Name} - {YearBox.Text}-{MonthBox.Text}.svg");
                // Try to write invoice.
                await Utilities.SaveDocumentAsync(t, fileName);
            }

            DialogResult = true;
        }

        #endregion



        #region METHODS

        /// <summary>
        /// Reads SVG template file.
        /// </summary>
        /// <param name="svgFilePath">SVG invoice template file path.</param>
        private string ReadTemplate(string svgFilePath)
        {
            string result = String.Empty;

            try
            {
                using (StreamReader reader = new StreamReader(svgFilePath))
                {
                    result = reader.ReadToEnd();
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show($"There was an error reading SVG template file: {exc.Message}",
                                "Error.", 
                                MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            return result;
        }

        #endregion
    }
}
