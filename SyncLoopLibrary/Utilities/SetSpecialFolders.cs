using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace SyncLoopLibrary
{
    public static partial class Utilities
    {
        /// <summary>
        /// Sets data, settings, reports, invoices template and invoices folders.
        /// </summary>
        /// <param name="dataFolder">Data folder path.</param>
        public static void SetSpecialFolders(string dataFolder)
        {
            string settingsFolder = Path.Combine(dataFolder, "Settings");
            string reportsFolder = Path.Combine(dataFolder, "Reports");
            string invoicesFolder = Path.Combine(dataFolder, "Invoices");
            string invoicesTemplateFolder = Path.Combine(dataFolder, "Invoices Template");

            // Set the entries.
            if (!Settings.ApplicationSettings.Folders.ContainsKey("Data"))
            {
                Settings.ApplicationSettings.Folders.Add("Data", dataFolder);
            }
            else
            {
                Settings.ApplicationSettings.Folders["Data"] = dataFolder;
            }


            if (!Settings.ApplicationSettings.Folders.ContainsKey("Settings"))
            {
                Settings.ApplicationSettings.Folders.Add("Settings", settingsFolder);
            }
            else
            {
                Settings.ApplicationSettings.Folders["Settings"] = settingsFolder;
            }


            if (!Settings.ApplicationSettings.Folders.ContainsKey("Reports"))
            {
                Settings.ApplicationSettings.Folders.Add("Reports", reportsFolder);
            }
            else
            {
                Settings.ApplicationSettings.Folders["Reports"] = reportsFolder;
            }


            if (!Settings.ApplicationSettings.Folders.ContainsKey("Invoices"))
            {
                Settings.ApplicationSettings.Folders.Add("Invoices", invoicesFolder);
            }
            else
            {
                Settings.ApplicationSettings.Folders["Invoices"] = invoicesFolder;
            }


            if (!Settings.ApplicationSettings.Folders.ContainsKey("Invoices Template"))
            {
                Settings.ApplicationSettings.Folders.Add("Invoices Template", invoicesTemplateFolder);
            }
            else
            {
                Settings.ApplicationSettings.Folders["Invoices Template"] = invoicesTemplateFolder;
            }
        }
    }
}
