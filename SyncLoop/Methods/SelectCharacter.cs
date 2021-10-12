using SyncLoopLibrary;
using System.Windows;

namespace SyncLoop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class TextEditor : Window
    {
        /// <summary>
        /// /// Selects character from dialog.
        /// /// </summary>
        /// /// <param name="characterToLook">Text editor current line.</param>
        public Character SelectCharacter(string characterToLook)
        {

            ProgramCharacterSelector = new CharacterSelector(characterToLook)
            {
                DataContext = Characters
            };

            bool? result = ProgramCharacterSelector.ShowDialog();

            ProgramCharacterSelector.Focus();

            if (result == true)
            {
                if ((Character)ProgramCharacterSelector.CharactersList.SelectedItem != null)
                {
                    return (Character)ProgramCharacterSelector.CharactersList.SelectedItem;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
