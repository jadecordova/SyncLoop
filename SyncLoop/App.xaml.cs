using Newtonsoft.Json;
using SyncLoopLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Animation;
using TextBox = System.Windows.Controls.TextBox;
using MessageBox = System.Windows.MessageBox;

namespace SyncLoop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {

        #region OVERRIDES

        /// <summary>
        /// Overrides the OnStartUp method.
        /// </summary>
        protected override void OnStartup(StartupEventArgs e)
        {

            // App initialization.
            Init();
            // The base method.
            base.OnStartup(e);



            #region ARGUMENT CHECK

            // If the program was opened by double clicking a project file,
            // the filename will be here.
            if (e != null && e.Args.Length > 0)
            {
                // string to load file into.
                string json = null;

                // Deserialize the project.
                try
                {
                    using (StreamReader reader = new StreamReader(e.Args[0]))
                    {
                        json = reader.ReadToEnd();
                    }

                    if (!String.IsNullOrEmpty(json))
                    {
                        try
                        {
                            Settings.ApplicationSettings.Project = JsonConvert.DeserializeObject<Project>(json);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"There was an error reading the project file contents: {ex.Message}", 
                                             "SyncLoop",
                                             MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Project file is invalid.", 
                                        "SyncLoop",
                                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"There was an error reading the project file: {ex.Message}", 
                                     "SyncLoop",
                                     MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }

            #endregion

        }

        #endregion



        #region METHODS

        /// <summary>
        /// This method initializes the general application settings.
        /// This is done here to have these values as global variables and avoid the need for helpers
        /// to pass information between classes in different assemblies.
        /// It also register a focus event for text boxes.
        /// </summary>
        private void Init()
        {
            // Reset error message.
            Settings.ApplicationSettings.ErrorMessage = null;
            // Notifications.
            StringBuilder notifications = new StringBuilder();
            // Settings file name.
            Settings.ApplicationSettings.SettingsFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"SyncLoop Data\Settings\Settings.json");


            // Try to load settings file.
            if (File.Exists(Settings.ApplicationSettings.SettingsFile))
            {
                Settings.LoadSettings(Settings.ApplicationSettings.SettingsFile);
            }
            else
            {
                notifications.Append("Settings file does not exist." + Environment.NewLine);
            }

            // Reset settings file path.
            // Settings file name.
            Settings.ApplicationSettings.SettingsFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"SyncLoop Data\Settings\Settings.json");

            // Create data folders. This has to be set every time, since the program may be running
            // on a differente computer.
            // First we get the application folder.
            string appFolder = AppDomain.CurrentDomain.BaseDirectory;

            // Now we set the data folder.
            string dataFolder = Path.Combine(appFolder, "SyncLoop Data");

            // Check if the dictionary is set.
            if (Settings.ApplicationSettings.Folders == null)
            {
                Settings.ApplicationSettings.Folders = new Dictionary<string, string>();
            }

            // And create the folders.
            Utilities.SetSpecialFolders(dataFolder);


            // Check for folders.
            foreach (KeyValuePair<string, string> folder in Settings.ApplicationSettings.Folders)
            {
                // Check for data folder.
                if (!Directory.Exists(folder.Value))
                {
                    // Add to notifications.
                    notifications.Append($"Folder {folder.Key} do not exist." + Environment.NewLine);

                    // Try to create folder.
                    try
                    {
                        Directory.CreateDirectory(folder.Value);
                        // Notify.
                        notifications.Append($"Folder {folder.Key} created." + Environment.NewLine);
                    }
                    catch (Exception ex)
                    {
                        notifications.Append($"Folder {folder.Key} could not be created: {ex.Message}" + Environment.NewLine);
                    }
                }
            }

            // Check invoices template
            string svgTemplateFile = Path.Combine(Settings.ApplicationSettings.Folders["Invoices Template"], "SVG Template.svg");

            string svgTemplateData = String.Empty;

            if (!File.Exists(svgTemplateFile))
            {
                notifications.Append("SVG invoice template does not exist. A new one will be created from stored data." + Environment.NewLine);

                // Get template from database.
                try
                {
                    svgTemplateData = Database.GetSVGTemplate();
                }
                catch (Exception e)
                {
                    notifications.Append($"Error reading template from database: {e.Message}" + Environment.NewLine);
                }
                // If string has content...
                if (!String.IsNullOrEmpty(svgTemplateData))
                {
                    // Write template to data folder.
                    using (StreamWriter writer = new StreamWriter(svgTemplateFile))
                    {
                        try
                        {
                            writer.Write(svgTemplateData);
                        }
                        catch (Exception e)
                        {
                            notifications.Append($"Error writting SVG template file: {e.Message}" + Environment.NewLine);
                        }
                    }

                }
            }

            // Set SVG file in settings.
            if (File.Exists(svgTemplateFile))
            {
                Settings.ApplicationSettings.SVGTemplate = svgTemplateFile;
            }

            // Check for file.
            if (!File.Exists(Settings.ApplicationSettings.SettingsFile))
            {
                // Create a new one.
                Settings.ApplicationSettings.SaveSettingsSync();
                // Notify.
                notifications.Append("Settings file created." + Environment.NewLine);
            }

            // Get current period.
            Settings.ApplicationSettings.CurrentPeriod = Database.GetPeriod();
            // Create project file object.
            Settings.ApplicationSettings.Project = new Project();
            // Works for tab into textbox
            EventManager.RegisterClassHandler(typeof(System.Windows.Controls.TextBox), System.Windows.Controls.TextBox.GotFocusEvent, new RoutedEventHandler(TextBox_GotFocus));

            // File association.
            if (Utilities.IsNotAssociated())
            {
                Utilities.Create_Sync_FileAssociation();
            }

            if (!String.IsNullOrWhiteSpace(notifications.ToString()))
            {
                Settings.ApplicationSettings.ErrorMessage = notifications.ToString();
            }
        }

        #endregion



        #region EVENT HANDLERS

        /// <summary>
        /// Saves project file and settings on exit.
        /// </summary>
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            // When exiting the application, we should save the project file.
            // First, let's check a project or document was actually opened.
            if (Settings.ApplicationSettings.Project.DocumentName != null)
            {
                // Then, create the project file name and path.
                string path = Path.Combine(Settings.ApplicationSettings.Project.ProjectFolder, Settings.ApplicationSettings.Project.DocumentName + ".syncloop");

                if (!String.IsNullOrEmpty(path))
                {

                    try
                    {
                        using (StreamWriter writer = new StreamWriter(path))
                        {
                            writer.Write(JsonConvert.SerializeObject(Settings.ApplicationSettings.Project, Formatting.Indented));
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"There was an error writing the project file: {ex.Message}", 
                                         "SyncLoop", 
                                         MessageBoxButton.OK, MessageBoxImage.Warning);
                    }

                }
            }

            // And finally we save the settings.
            Settings.ApplicationSettings.SaveSettingsSync();
        }

        /// <summary>
        /// Selects all text in text box.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Arguments.</param>
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            (sender as TextBox).SelectAll();
        }

        #endregion
    }
}
