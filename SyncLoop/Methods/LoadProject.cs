using SyncLoopLibrary;
using System.Windows;

namespace SyncLoop
{

    public partial class TextEditor : Window
    {

        /// <summary>
        /// Loads project from file and opens the appropriate documents and video file.
        /// </summary>
        /// <param name="project">Project object.</param>
        private void LoadProject(Project project)
        {
            if (project != null)
            {
                // Get the program info.
                if (project.ProgramInfoFile != null)
                {
                    programInfo.Load(project.ProgramInfoFile);
                }

                // First, let's see if there is a proof read document set,
                // since this is most likely the one the user will want to work with.
                if (project.ProofReadFile != null)
                {
                    OpenTextFile(project.ProofReadFile);
                }
                else if (project.TextFile != null)
                {
                    OpenTextFile(project.TextFile);

                    MessageBox.Show("There is no proof read file defined. Text file loaded.",
                                    "SyncLoop",
                                    MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                if (project.XamlFile != null)
                {
                    OpenTextFile(project.XamlFile);
                }

                // Now let's load the characters.
                if (project.CharactersFile != null)
                {
                    LoadCharacters(project.CharactersFile, false);
                }
                
                // And finally, the video.
                if (project.VideoFile != null)
                {
                    Player.OpenVideo(project.VideoFile);
                }
            }
            else
            {
                MessageBox.Show("Invalid project file.",
                                "SyncLoop", 
                                MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
