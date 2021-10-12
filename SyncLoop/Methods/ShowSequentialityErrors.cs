using System.Collections.Generic;
using System.Windows;

namespace SyncLoop
{
    public partial class TextEditor : Window
    {
        /// <summary>
        /// Shows a window with any loop error.
        /// </summary>
        public void ShowErrors(List<string> errors)
        {
            Errors dialog = new Errors()
            {

                DataContext = errors,

                Editor = this.Editor
            };

            dialog.Show();
        }
    }
}
