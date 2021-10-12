using SyncLoopLibrary;
using System;
using System.Diagnostics;
using System.Windows;

namespace SyncLoop
{
    public partial class TextEditor : Window
    {
        /// <summary>
        /// Adds program to database.
        /// </summary>
        /// <param name="firstTimecode">Initial timecode of program.</param>
        /// <param name="lastTimecode">Final timecode of program.</param>
        private void AddProgramToDB(string firstTimecode, string lastTimecode)
        {
            // We must check if the period is defined. If not, we have to allow user to create one,
            // since it cannot be null.
            if (Settings.ApplicationSettings.CurrentPeriod == null)
            {
                // Create the period editor window.
                PeriodEditor editor = new PeriodEditor();

                // And set the start date as the current date by default.
                editor.StartDateBox.SelectedDate = DateTime.Now;

                if (editor.ShowDialog() == true)
                {
                    // We create a new period object and assign it to settings.
                    Settings.ApplicationSettings.CurrentPeriod = new Period();

                    // If the user selected dates, we use them.
                    if (editor.StartDateBox.SelectedDate != null)
                    {
                        Settings.ApplicationSettings.CurrentPeriod.StartDate = (DateTime)editor.StartDateBox.SelectedDate;
                    }
                    else
                    {
                        // If not, we use the current date.
                        Settings.ApplicationSettings.CurrentPeriod.StartDate = DateTime.Now;
                    }
                    
                    // Same for the end date.
                    if (editor.EndDateBox.SelectedDate != null)
                    {
                        Settings.ApplicationSettings.CurrentPeriod.EndDate = (DateTime)editor.EndDateBox.SelectedDate;
                    }
                }

                // If the user canceled the dialog, we create a default period, since it cannot be null.
                else
                {
                    Settings.ApplicationSettings.CurrentPeriod = new Period();
                    Settings.ApplicationSettings.CurrentPeriod.StartDate = DateTime.Now;
                }

                // Finally, we insert the period into de datbase.
                Database.InsertPeriod(Settings.ApplicationSettings.CurrentPeriod);
            }

            // Get the program duration.
            int duration = SMPTE.GetProgramTime(firstTimecode, lastTimecode);

            // Calculate the amount.
            programInfo.Amount = programInfo.RateAmount * duration;

            // And insert the program into the DB.
            try
            {
                Database.InsertProgram(programInfo, duration, Settings.ApplicationSettings.CurrentPeriod);
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error inserting programa into DB: {e.Message}",
                                "SyncLoop", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
