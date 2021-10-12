using SyncLoopLibrary;
using System.Windows;
using System.Windows.Input;

namespace SyncLoop
{
    public partial class TextEditor : Window
    {
        private void ReadCharacters_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = DOCUMENT_LOADED;
        }

        private void ReadCharacters_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Characters = Utilities.ReadCharactersFromDocument(Editor.Document);

            // Create character selector.
            ProgramCharacterSelector = new CharacterSelector()
            {
                DataContext = Characters
            };

            // Show the dialog.
            if(ProgramCharacterSelector.ShowDialog() == true)
            {
                // Save.
                ProgramCharacterSelector.SaveCharacters();
            }
        }
    }
}
