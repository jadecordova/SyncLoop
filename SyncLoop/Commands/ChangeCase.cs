using System;
using System.Windows;
using System.Windows.Input;

namespace SyncLoop
{
    public partial class TextEditor : Window
    {
        private void UpperCase_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            // Enabled when text is selected in Editor.
            e.CanExecute = !String.IsNullOrEmpty(Editor.Selection.Text);
        }

        private void UpperCase_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // Convert selected text.
            Editor.Selection.Text = Editor.Selection.Text.ToUpper();
        }

        private void LowerCase_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            // Enabled when text is selected in Editor.
            e.CanExecute = !String.IsNullOrEmpty(Editor.Selection.Text);
        }

        private void LowerCase_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // Convert selected text.
            Editor.Selection.Text = Editor.Selection.Text.ToLower();
        }
    }
}
