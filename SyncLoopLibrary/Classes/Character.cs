using System.Windows.Media;

namespace SyncLoopLibrary
{
    /// <summary>
    /// Character information.
    /// </summary>
    public class Character : Notifier
    {

        #region FIELDS

        private string name;
        private string titleName;
        private string title;
        private CharacterGender gender;
        private Color color;
        private int lines;

        #endregion



        #region PROPERTIES

        /// <summary>
        /// Name of character.
        /// </summary>
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Name of character as it appears in title.
        /// </summary>
        public string TitleName
        {
            get { return titleName; }
            set
            {
                titleName = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Title of character.
        /// </summary>
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gender of character
        /// </summary>
        public CharacterGender Gender
        {
            get { return gender; }
            set
            {
                gender = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Color of loop string for making easy the character identification in RTF file for correction.
        /// </summary>
        public Color CharacterColor
        {
            get { return color; }
            set
            {
                color = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Number of lines spoken.
        /// </summary>
        public int Lines
        {
            get { return lines; }
            set
            {
                lines = value;
                NotifyPropertyChanged();
            }
        }

        #endregion



        #region OVERRIDES

        /// <summary>
        /// Returns character's name.
        /// </summary>
        /// <returns>Character name.</returns>
        public override string ToString()
        {
            return Name;
        }

        #endregion
    }
}
