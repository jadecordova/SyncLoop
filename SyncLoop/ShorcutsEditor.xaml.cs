using System.Windows;

namespace SyncLoop
{
    /// <summary>
    /// This window allows for shorcut editings.
    /// </summary>
    public partial class ShorcutsEditor : Window
    {

        #region CONSTRUCTORS

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ShorcutsEditor()
        {
            InitializeComponent();
        }

        #endregion



        #region EVENT HANDLERS

        /// <summary>
        /// Closes dialog.
        /// </summary>
        private void OK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        #endregion
    }
}
