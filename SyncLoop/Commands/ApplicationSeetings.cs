using SyncLoopLibrary;
using System.Windows;
using System.Windows.Input;

namespace SyncLoop
{
    public partial class TextEditor : Window
    {
        private void ApplicationSeetings_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        // The channels variable is defined in TextEditor.xaml.cs
        private void ApplicationSeetings_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // Create settings window.
            SettingsEditor settings = new SettingsEditor();
            // Set general data context.
            settings.DataContext = Settings.ApplicationSettings;
            // Set channels data context..
            settings.ChannelsBox.DataContext = Channels;
            // Show editor.
            if (settings.ShowDialog() == true)
            {
                // Set player video mode.
                Player.DocumentType = Settings.ApplicationSettings.DocumentType;
            }
        }
    }
}
