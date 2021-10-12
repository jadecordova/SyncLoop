namespace SyncLoopLibrary
{
    /// <summary>
    /// Series information.
    /// </summary>
    public class Series : Notifier
    {
        #region MEMBERS

        private long id;
        private long channelID;
        private string nameEnglish;
        private string nameSpanish;

        #endregion



        #region PROPERTIES

        /// <summary>
        /// Series ID.
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
        /// Channel ID.
        /// </summary>
        public long ChannelID
        {
            get { return channelID; }
            set
            {
                channelID = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Series name in English.
        /// </summary>
        public string NameEnglish
        {
            get { return nameEnglish; }
            set
            {
                nameEnglish = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Series name in Spanish.
        /// </summary>
        public string NameSpanish
        {
            get { return nameSpanish; }
            set
            {
                nameSpanish = value;
                NotifyPropertyChanged();
            }
        }

        #endregion
    }
}
