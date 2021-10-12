namespace SyncLoopLibrary
{
    public static partial class Utilities
    {
        /// <summary>
        /// Initializes editor shortcuts.
        /// </summary>
        /// <param name="document">Document text.</param>
        /// <returns>Collection of shortcut strings.</returns>
        public static string ReplaceGestures(string document)
        {
            // Result object.
            return document.Replace(Settings.ApplicationSettings.GesturesCharacter, "(GESTOS)");
        }
    }
}
