using System.Windows;
using System.Windows.Input;

namespace SyncLoop
{
    public partial class TextEditor : Window
    {
        private void ShowProgramInfo_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ShowProgramInfo_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            GetProgramInfo();
        }
    }
}
