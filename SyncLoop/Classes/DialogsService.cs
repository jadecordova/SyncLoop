using SyncLoopLibrary;
using System;

namespace SyncLoop
{
    /// <summary>
    /// Utility class for input dialogs.
    /// </summary>
    public static class DialogsService
    {

        #region MEMBERS

        private static InputDialog dialog;

        #endregion



        #region METHODS

        /// <summary>
        /// General method to get input.
        /// </summary>
        /// <param name="labelText">Label text.</param>
        /// <param name="initialText">Initial value of the text box. Defaults to empty.</param>
        /// <returns>Input from the user.</returns>
        private static string GetValue(string labelText, string initialText = "")
        {
            string result = String.Empty;

            dialog = new InputDialog();
            dialog.LabelText.Text = labelText;
            dialog.UserInput.Text = initialText;
            dialog.ShowDialog();

            if (dialog.DialogResult == true)
            {
                result = dialog.UserInput.Text;
            }

            return result;
        }


        /// <summary>
        /// Gets SMPTE timecode.
        /// </summary>
        /// <returns>Valid SMPTE timecode string.</returns>
        public static string GetSmpte()
        {
            // Get user input.
            return SMPTE.ValidateSmpte(GetValue("Input time code:"));
        }

        #endregion
    }
}
