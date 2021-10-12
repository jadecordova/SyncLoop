using SyncLoopLibrary;
using System;
using System.Windows;
using System.Windows.Input;

namespace SyncLoop
{
    public partial class TextEditor : Window
    {
        private void ConvertUnits_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            // Enabled when text is selected in Editor.
            e.CanExecute = !String.IsNullOrEmpty(Editor.Selection.Text);
        }

        private void ConvertUnits_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // Convert selected text.
            Editor.Selection.Text = UnitConverter.Convert(Editor.Selection.Text);
        }
    }
}
