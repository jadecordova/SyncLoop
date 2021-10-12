using System.Windows;

namespace SyncLoop
{
    /// <summary>
    /// Dialog for the creation and editing of work periods.
    /// </summary>
    public partial class PeriodEditor : Window
    {
        #region CONSTRUCTORS

        /// <summary>
        /// Default constructor.
        /// </summary>
        public PeriodEditor()
        {
            InitializeComponent();
        }

        #endregion



        #region EVENT HANDLERS

        /// <summary>
        /// This handler just makes sure that the start date is set.
        /// </summary>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (StartDateBox.SelectedDate == null)
            {
                MessageBox.Show("Please, select a star date for the period.", 
                                "SyncLoop", 
                                MessageBoxButton.OK, MessageBoxImage.Hand);
            }
            else
            {
                DialogResult = true;
            }
        }

        #endregion
    }
}
