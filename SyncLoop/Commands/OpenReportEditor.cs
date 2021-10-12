using SyncLoopLibrary;
using System.Windows;
using System.Windows.Input;

namespace SyncLoop
{
    public partial class TextEditor : Window
    {
        private void OpenReportEditor_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Settings.ApplicationSettings.CurrentPeriod != null;
        }

        private void OpenReportEditor_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // Create window.
            ReportEditor report = new ReportEditor(Channels, series, Settings.ApplicationSettings.CurrentPeriod);

            // Set data context.
            report.DataContext = Settings.ApplicationSettings;

            report.ShowDialog();
        }
    }
}
