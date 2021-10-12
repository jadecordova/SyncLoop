using System;
using System.Collections.ObjectModel;

namespace SyncLoopLibrary
{
    public static partial class Utilities
    {
        /// <summary>
        /// Initializes editor shortcuts.
        /// </summary>
        /// <returns>Collection of shortcut strings.</returns>
        public static ObservableCollection<string> InitShortcuts()
        {
            ObservableCollection<string> result = new ObservableCollection<string>();
            // Add defaults.
            result.Add("(GESTOS)");
            result.Add("(PAUSA)");
            // Init with empty strings.
            for (int i = 2; i < 8; i++)
            {
                result.Add(String.Empty);
            }
            return result;
        }
    }
}
