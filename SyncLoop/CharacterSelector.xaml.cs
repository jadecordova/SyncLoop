using Newtonsoft.Json;
using SyncLoopLibrary;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace SyncLoop
{
    /// <summary>
    /// Interaction logic for CharacterSelector.xaml
    /// </summary>
    public partial class CharacterSelector : Window
    {
        #region MEMBERS

        /// <summary>
        /// Index of last character.
        /// </summary>
        private static int lastCharacter = 0;

        /// <summary>
        /// Index of list of colors assigned to last character created.
        /// </summary>
        private static int currentCharacterColorIndex = 0;

        /// <summary>
        /// Text from text editor to search for character.
        /// </summary>
        private string characterText = String.Empty;

        #endregion



        #region CONSTRUCTORS

        /// <summary>
        /// Default constructor.
        /// Can accept a line of text from text editor
        /// containing the character to look for.
        /// </summary>
        public CharacterSelector(string textEditorText = "")
        {
            InitializeComponent();
            // Set character text.
            characterText = textEditorText;
            // Set sorting.
            CharactersList.Items.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            // Do stupid things just to get the focus on list item.
            CharactersList.ItemContainerGenerator.StatusChanged += ItemContainerGeneratorOnStatusChanged;
        }

        #endregion



        #region METHODS

        /// <summary>
        /// Selects character based on name.
        /// </summary>
        /// <param name="text">Text to compare with names.</param>
        private void SelectCharacter(string text)
        {
            // Holds the character name.
            string name = String.Empty;

            // If character is not null or empty...
            if (!String.IsNullOrEmpty(text))
            {
                // Iterate the list box.
                foreach (var listItem in CharactersList.Items)
                {
                    // Assign the current name to variable.
                    name = ((Character)listItem).Name.ToUpper();

                    // If the line passed from the text editor contains the name of this list box entry...
                    if (text.ToUpper().Contains(name))
                    {
                        // Get the index of this item. Set it as last character selected.
                        lastCharacter = CharactersList.Items.IndexOf(listItem);
                        // No need for further iteration, so we...
                        break;
                    }
                }
            }

            // And select it in the list box.
            CharactersList.SelectedIndex = lastCharacter;
        }


        /// <summary>
        /// Saves characters to disk.
        /// </summary>
        public async void SaveCharacters()
        {
            if (!String.IsNullOrEmpty(Settings.ApplicationSettings.Project.ProjectFolder) && !String.IsNullOrEmpty(Settings.ApplicationSettings.Project.DocumentName))
            {
                // Get file name.
                string charactersFilePath = Path.Combine(Settings.ApplicationSettings.Project.ProjectFolder, Settings.ApplicationSettings.Project.DocumentName + @" - Characters.json");
                // Assign it to project file object.
                Settings.ApplicationSettings.Project.CharactersFile = charactersFilePath;

                try
                {
                    using (StreamWriter writer = new StreamWriter(charactersFilePath))
                    {
                        await writer.WriteAsync(JsonConvert.SerializeObject((ObservableCollection<Character>)DataContext, Formatting.Indented));
                        // Save in project object.
                        Settings.ApplicationSettings.Project.CharactersFile = charactersFilePath;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"There was an error writing the characters info file: {ex.Message}", 
                                    "SyncLoop", 
                                    MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show($"No project folder selected. Please save your file first.", 
                                "SyncLoop", 
                                MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        #endregion



        #region EVENT HANDLERS

        /// <summary>
        /// Method to do stupid things to get the focus on first element of characters list.
        /// </summary>
        void ItemContainerGeneratorOnStatusChanged(object sender, EventArgs eventArgs)
        {
            if (CharactersList.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
            {
                var index = CharactersList.SelectedIndex;

                if (index >= 0)
                {
                    if (CharactersList.ItemContainerGenerator.ContainerFromIndex(index) is ListBoxItem item) item.Focus();
                }
            }
        }


        /// <summary>
        /// Selects the character passed by the line of text.
        /// </summary>
        private void CharacterSelectorContentRendered(object sender, EventArgs e)
        {
            // Select character.
            SelectCharacter(characterText);
        }


        /// <summary>
        /// Sets last character.
        /// </summary>
        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            // Set selected character.
            lastCharacter = CharactersList.SelectedIndex;

            DialogResult = true;
        }


        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }


        /// <summary>
        /// Creates new character.
        /// </summary>
        private void AddCharacter(object sender, RoutedEventArgs e)
        {
            Character newCharacter = new Character();

            CharacterEditor characterEditor = new CharacterEditor(true);

            characterEditor.DataContext = newCharacter;

            characterEditor.ShowDialog();

            characterEditor.Focus();

            if (characterEditor.DialogResult == true)
            {

                // Reset character color if out of bounds.
                if (currentCharacterColorIndex > RTFUtilities.CharacterColors.Count - 1)
                {
                    currentCharacterColorIndex = 0;
                }

                // Set color to black if is a title.
                if (newCharacter.Name.ToUpper().Contains(Settings.ApplicationSettings.TitleString.ToUpper()))
                {
                    newCharacter.CharacterColor = System.Windows.Media.Colors.Black;
                }

                // Set color to blue if it is narrator.
                else if (newCharacter.Name.ToUpper().Contains("NARRADOR"))
                {
                    newCharacter.CharacterColor = System.Windows.Media.Colors.Blue;
                }

                // Set to one of the predefined colors.
                else
                {
                    newCharacter.CharacterColor = RTFUtilities.CharacterColors[currentCharacterColorIndex];
                }

                // Increment color index.
                currentCharacterColorIndex++;

                // Remove gender for titles, groups and generics.
                if (newCharacter.Name.ToUpper().Contains(Settings.ApplicationSettings.TitleString.ToUpper()) ||
                    newCharacter.Name.ToUpper().Contains("GRUPO") ||
                    newCharacter.Name.ToUpper().Contains("HOMBRE") ||
                    newCharacter.Name.ToUpper().Contains("MUJER") ||
                    newCharacter.Name.ToUpper().Contains("NIÑO") ||
                    newCharacter.Name.ToUpper().Contains("NIÑA"))
                {
                    newCharacter.Gender = CharacterGender.NONE;
                }

                // Flag.
                bool characterExists = false;

                // Check if character already exists.
                foreach (Character c in (ObservableCollection<Character>)DataContext)
                {
                    if (c.Name.ToUpper() == newCharacter.Name.ToUpper())
                    {
                        characterExists = true;

                        break;
                    }
                }

                // Add to characters list if it doesn't exist.
                if (!characterExists)
                {
                    ((ObservableCollection<Character>)DataContext).Add(newCharacter);
                }
                else
                {
                    MessageBox.Show("Character already exists.", 
                                    "SyncLoop", 
                                    MessageBoxButton.OK, MessageBoxImage.Information);
                }

                // Select new character.
                SelectCharacter(newCharacter.Name);

                // Save characters to disk.
                SaveCharacters();
            }
        }


        /// <summary>
        /// Edits character.
        /// </summary>
        private void EditCharacter(object sender, RoutedEventArgs e)
        {
            // First we must check for no character selected in the list.
            if(CharactersList.SelectedItem != null)
            {
                // Get the character to edit from the list.
                Character characterToEdit = ((ObservableCollection<Character>)DataContext).Single(i => i.Name == ((Character)CharactersList.SelectedItem).Name);
                // Save old name for replacing in document.
                string oldName = characterToEdit.Name;
                // Create new object and copy original character to compare for duplicates later.
                Character editedCharacter = new Character()
                {
                    Name = characterToEdit.Name,
                    TitleName = characterToEdit.TitleName,
                    Title = characterToEdit.Title,
                    Gender = characterToEdit.Gender
                };

                CharacterEditor characterEditor = new CharacterEditor()
                {
                    DataContext = editedCharacter
                };

                characterEditor.ShowDialog();

                characterEditor.Focus();

                if (characterEditor.DialogResult == true)
                {
                    // Check for duplicate character.
                    // Allow case of same character but editing other character's fields.
                    foreach (Character c in (ObservableCollection<Character>)DataContext)
                    {
                        if (c.Name.ToUpper() == editedCharacter.Name.ToUpper() &&
                            c.Name.ToUpper() != oldName.ToUpper())
                        {
                            MessageBox.Show("Character already exists.", 
                                            "SyncLoop", 
                                            MessageBoxButton.OK, MessageBoxImage.Information);

                            return;
                        }
                    }

                    // Get info.
                    characterToEdit.Name = editedCharacter.Name.ToUpper();

                    characterToEdit.TitleName = editedCharacter.TitleName;

                    characterToEdit.Title = editedCharacter.Title;

                    // Set gender.
                    if ( !characterToEdit.Name.ToUpper().Contains(Settings.ApplicationSettings.TitleString.ToUpper()) &&
                         !characterToEdit.Name.ToUpper().Contains("GRUPO"))
                    {
                        if ((bool)characterEditor.Male.IsChecked)
                        {
                            characterToEdit.Gender = CharacterGender.MASCULINO;
                        }
                        else if ((bool)characterEditor.Female.IsChecked)
                        {
                            characterToEdit.Gender = CharacterGender.FEMENINO;
                        }
                        else
                        {
                            characterToEdit.Gender = CharacterGender.NONE;
                        }
                    }

                    else
                    {
                        characterToEdit.Gender = CharacterGender.NONE;
                    }

                    SaveCharacters();

                    // Ask for replacement.
                    if (oldName.ToUpper() != characterToEdit.Name.ToUpper())
                    {
                        if (MessageBox.Show("Do you want to rename the character in the current document?",
                                           "SyncLoop",
                                           MessageBoxButton.YesNo,
                                           MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            // Create renamer.
                            FindAndReplaceManager replacer = new FindAndReplaceManager(((TextEditor)Application.Current.Windows[0]).Editor.Document);
                            // Replace all.
                            replacer.ReplaceAll(oldName, characterToEdit.Name, FindOptions.MatchCase, null);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Deletes character.
        /// </summary>
        private void DeleteCharacter(object sender, RoutedEventArgs e)
        {
            if (CharactersList.Items.Count > 0)
            {
                // Select character directly from list of characters provided as data context.
                Character characterToEdit = ((ObservableCollection<Character>)DataContext).Single(i => i.Name == ((Character)CharactersList.SelectedItem).Name);

                if (characterToEdit != null)
                {
                    MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete character {characterToEdit.Name}?", 
                                                               "Delete character", 
                                                               MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        ((ObservableCollection<Character>)DataContext).Remove(characterToEdit);
                        // Save.
                        SaveCharacters();
                    }
                }
            }
        }

        #endregion

    }
}
