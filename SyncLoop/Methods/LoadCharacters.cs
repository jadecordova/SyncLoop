using Newtonsoft.Json;
using SyncLoopLibrary;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

namespace SyncLoop
{

    public partial class TextEditor : Window
    {

        /// <summary>
        /// Loads characters from disk.
        /// </summary>
        private void LoadCharacters(string file, bool showMessage = true)
        {
            // Holds the read file.
            string json;

            // If user selected file.
            if (!String.IsNullOrEmpty(file))
            {
                if (File.Exists(file))
                {
                    try
                    {
                        using (StreamReader reader = new StreamReader(file))
                        {
                            json = reader.ReadToEnd();
                        }
                        if (!String.IsNullOrEmpty(json))
                        {
                            try
                            {
                                Characters = JsonConvert.DeserializeObject<ObservableCollection<Character>>(json);

                                // Save it to project object.
                                Settings.ApplicationSettings.Project.CharactersFile = file;
                                
                                // Inform success.
                                if (showMessage) MessageBox.Show($"{Characters.Count} characters loaded.", "SyncLoop", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show($"There was an error reading the characters file contents: {e.Message}",
                                                "SyncLoop", MessageBoxButton.OK, MessageBoxImage.Warning);

                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Characters file is invalid.",
                                            "SyncLoop",
                                            MessageBoxButton.OK, MessageBoxImage.Warning);

                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"There was an error reading the characters file: {ex.Message}",
                                        "SyncLoop", 
                                        MessageBoxButton.OK, MessageBoxImage.Warning);

                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Characters file doesn't exist.",
                                    "SyncLoop", 
                                    MessageBoxButton.OK, MessageBoxImage.Warning);

                    return;
                }
            }
        }
    }
}
