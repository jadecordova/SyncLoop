using SyncLoopLibrary;
using System;
using System.Windows;

namespace SyncLoop
{
    /// <summary>
    /// Interaction logic for CharacterEditor.xaml
    /// </summary>
    public partial class CharacterEditor : Window
    {

        #region FIELDS

        // Current character.
        private Character WorkingCharacter;
        // Indicates new character creation.
        private bool IsNewCharacter = false;

        #endregion



        #region CONSTRUCTORS

        /// <summary>
        /// For new character.
        /// </summary>
        public CharacterEditor(bool isNewCharacter = false)
        {
            InitializeComponent();

            Loaded += CharacterEditorLoaded;

            IsNewCharacter = isNewCharacter;
        }

        #endregion



        #region EVENT HANDLERS

        private void CharacterEditorLoaded(object sender, RoutedEventArgs e)
        {
            // Get Data Context.
            WorkingCharacter = (Character)DataContext;

            if (IsNewCharacter)
            {
                // Set default gender.
                WorkingCharacter.Gender = CharacterGender.MASCULINO;
                // Initialize number of lines.
                WorkingCharacter.Lines = 0;
            }

            Name.Focus();

            Name.SelectAll();
        }


        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            // Name must be set.
            if (!String.IsNullOrEmpty(Name.Text))
            {
                // Set properties.
                WorkingCharacter.Name = Name.Text.ToUpper();

                WorkingCharacter.TitleName = TitleName.Text;

                WorkingCharacter.Title = Title.Text;

                // Set gender.
                if ((bool)Male.IsChecked)
                {
                    WorkingCharacter.Gender = CharacterGender.MASCULINO;
                }
                else if ((bool)Female.IsChecked)
                {
                    WorkingCharacter.Gender = CharacterGender.FEMENINO;
                }
                else
                {
                    WorkingCharacter.Gender = CharacterGender.NONE;
                }

                // Accept and close dialog.
                DialogResult = true;
            }
        }


        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        #endregion
    }
}
