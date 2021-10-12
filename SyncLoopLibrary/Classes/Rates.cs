namespace SyncLoopLibrary
{
    /// <summary>
    /// Rates information.
    /// </summary>
    public class Rates : Notifier
    {

        #region FIELDS

        private decimal normal;
        private decimal rush;
        private decimal lessThan48Hours;
        private decimal iva;

        #endregion


        #region PROPERTIES

        /// <summary>
        /// Normal rate.
        /// </summary>
        public decimal Normal
        {
            get { return normal; }
            set
            {
                normal = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Rush rate.
        /// </summary>
        public decimal Rush
        {
            get { return rush; }
            set
            {
                rush = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Less than 48 hours rate.
        /// </summary>
        public decimal LessThan48Hours
        {
            get { return lessThan48Hours; }
            set
            {
                lessThan48Hours = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Iva rate.
        /// </summary>
        public decimal IVA
        {
            get { return iva; }
            set
            {
                iva = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

    }
}
