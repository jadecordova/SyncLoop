using System;
using System.Windows;

namespace SyncLoop
{
    /// <summary>
    /// Interaction logic for InputDialog.xaml
    /// </summary>
    public partial class InputDialog : Window
    {

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="initialText">Text to populate input box.</param>
        public InputDialog(string initialText = "")
        {
            InitializeComponent();

            // If an initial text was passed...
            if (!String.IsNullOrWhiteSpace(initialText))
            {
                UserInput.Text = initialText;
            }
        }

        private void OKButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
