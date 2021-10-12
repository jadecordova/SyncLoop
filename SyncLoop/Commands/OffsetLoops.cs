using SyncLoopLibrary;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace SyncLoop
{
    public partial class TextEditor : Window
    {
        private void OffsetLoops_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = DOCUMENT_LOADED;
        }

        private void OffsetLoops_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // Create input dialog.
            InputDialog dialog = new InputDialog();
            // Show it.
            if(dialog.ShowDialog() == true)
            {
                // Create changes dictionary.
                Dictionary<string, string> changes = SMPTE.OffsetLoops(Editor.Document, new SMPTE(dialog.UserInput.Text), Settings.ApplicationSettings.SmpteFormat);

                // Create find manager.
                FindAndReplaceManager finder = new FindAndReplaceManager(Editor.Document);
                
                // Make changes.
                foreach(var change in changes)
                {
                    finder.Replace(change.Key, change.Value, FindOptions.None);
                }
            }
        }
    }
}
