namespace SyncLoopLibrary
{
    /// <summary>
    /// Channel information.
    /// </summary>
    public class Channel : Notifier
    {

        #region FIELDS

        private long id;
        private string code;
        private string name;

        #endregion



        #region PROPERTIES

        /// <summary>
        /// Channel ID
        /// </summary>
        public long ID
        {
            get { return id; }
            set
            {
                id = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Channel code.
        /// </summary>

        public string Code
        {
            get { return code; }
            set
            {
                code = value;
                NotifyPropertyChanged();
            }
        }
        /// <summary>
        /// Channel name.
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

        #endregion
    }
}
