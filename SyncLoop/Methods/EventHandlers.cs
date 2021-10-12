using SyncLoopLibrary;
using System.Windows;
using System.Windows.Controls;

namespace SyncLoop
{
    public partial class TextEditor : Window
    {

        /// <summary>
        /// Not sure why I subscribed to this event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectionChanged(object sender, RoutedEventArgs e)
        {
            UpdateTaskBarLabel();
        }

        /// <summary>
        /// Identifies the editor window for intercommunications with the video player window
        /// and attempts to load project file, if there is one.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextEditorLoaded(object sender, RoutedEventArgs e)
        {
            Player.EditorWindow = this;

            if (Settings.ApplicationSettings.Project != null)
            {
                LoadProject(Settings.ApplicationSettings.Project);
            }
        }
    }
}
