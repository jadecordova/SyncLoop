using SyncLoopLibrary;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;

namespace SyncLoop
{

    public partial class TextEditor : Window
    {
        #region MEMBERS

        /// <summary>
        /// Holds current program info.
        /// </summary>
        ProgramInfo programInfo = new ProgramInfo();

        #endregion



        #region PROPERTIES

        /// <summary>
        /// Holds all series defined for the Series combo box.
        /// </summary>
        public ObservableCollection<Series> EpisodeSeries { get; set; }

        #endregion


        /// <summary>
        /// Opens dialog to get current program info.
        /// This method first checks to see if the current directory is set, which is set on opening the text file.
        /// If not, it gives the user the chance to select a directory, or returns if no directory is selected.
        /// </summary>
        protected void GetProgramInfo()
        {

            // First, we check if the original text file folder is set.
            // It is set upon opening the text file in the text editor.
            if (String.IsNullOrEmpty(OriginalTextFileFolder))
            {
                // If not, we must let the user select a folder.
                FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog()
                {
                    Description = "Select folder to save project."
                };


                if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    OriginalTextFileFolder = folderBrowserDialog.SelectedPath;
                }
                else
                {
                    // If no folder was select, we inform the user and return.
                    MessageBox.Show("No folder has been selected to save the program info file. +" +
                                    "Changes will not be saved. Please, save document and try again.",
                                    "SyncLoop", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }

            // By now we have an original file folder set.
            // This folder could be the original from which the translated text file
            // was opened from, or, in the case of an RTF proofread file,
            // the auto-generated folder created after the proof file was saved.

            // Now we create the program info dialog.
            ProgramInfoDialog program = new ProgramInfoDialog(programInfo, Channels, series);

            if (program.ShowDialog() == true)
            {
                string documentName = String.Empty;

                // Create the document name.
                // First we must check if the channel was set.
                if (!String.IsNullOrEmpty(programInfo.EpisodeChannel.Code))
                {
                    // That would be the first part of the name.
                    documentName = programInfo.EpisodeChannel.Code;
                }
                else
                {
                    // If channel is not set, we inform and return.
                    MessageBox.Show($"Program channel must be set. " +
                                    $"If there is no channels in the selection list, please add one in the Settings dialog.",
                                    "SyncLoop", MessageBoxButton.OK, MessageBoxImage.Exclamation);

                    return;
                }

                // We are only gonna use the English series name,
                // so we have to check if that was set too.
                if (!String.IsNullOrEmpty(programInfo.EpisodeSeries.NameEnglish))
                {
                    // And add it to the name...
                    documentName += " - " + programInfo.EpisodeSeries.NameEnglish;
                }
                else
                {
                    // ...or inform the user and return.
                    MessageBox.Show($"Series name in English must be set.",
                                    "SyncLoop",
                                    MessageBoxButton.OK, MessageBoxImage.Exclamation);

                    return;
                }

                // We add the episode number, if set.
                if (!String.IsNullOrEmpty(programInfo.EpisodeNumber)) documentName += $" - {programInfo.EpisodeNumber}";

                // And the English episode name, if set.
                if (!String.IsNullOrEmpty(programInfo.EpisodeNameEnglish)) documentName += $" - {programInfo.EpisodeNameEnglish}";

                // The program code is mandatory, so we must check for it...
                if (!String.IsNullOrEmpty(programInfo.EpisodeCode))
                {
                    // ...and add it to the name.
                    documentName += " - " + programInfo.EpisodeCode;
                }
                else
                {
                    System.Windows.MessageBox.Show($"Program code must be set.","SyncLoop",MessageBoxButton.OK, MessageBoxImage.Exclamation);

                    return;
                }

                // At this point we have an episode name that should be the same
                // no matter if the file was the original text file or the RTF proof read file.
                // So we set the name.
                programInfo.DocumentName = documentName;

                // If we loaded an RTF proof read file, the generated folder
                // and the original text folder should be the same.
                // If not, we must assign it.
                if (programInfo.ProjectDirectory != OriginalTextFileFolder)
                {
                    programInfo.ProjectDirectory = $"{OriginalTextFileFolder}\\{programInfo.EpisodeCode}";
                }

                // We set the name in the project file.
                Settings.ApplicationSettings.Project.DocumentName = programInfo.DocumentName;

                // And the project folder.
                Settings.ApplicationSettings.Project.ProjectFolder = programInfo.ProjectDirectory;
                
                // Now we create the project folder, if it doesn't exist.
                try
                {
                    Directory.CreateDirectory(Settings.ApplicationSettings.Project.ProjectFolder);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Could not create program directory. {ex.Message}",
                                    "SyncLoop",
                                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }

                // Finally, we create the program info file name...
                string programInfoFilename = Path.Combine(Settings.ApplicationSettings.Project.ProjectFolder, $"{Settings.ApplicationSettings.Project.DocumentName} - Program Info.json");

                // ...Save the program info file....
                programInfo.Save(programInfoFilename);
                
                // ...and set it in the project file object.
                Settings.ApplicationSettings.Project.ProgramInfoFile = programInfoFilename;
            }
        }
    }
}
