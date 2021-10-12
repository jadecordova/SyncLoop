namespace SyncLoopLibrary
{
    /// <summary>
    /// Invoice data.
    /// </summary>
    public class Invoice : Notifier
    {
        #region FIELDS

        private Channel channel;
        private decimal subtotal;
        private decimal iva;
        private decimal ivaAmount;
        private decimal total;

        #endregion



        #region

        /// <summary>
        /// Invoice channel.
        /// </summary>
        public Channel Channel
        {
            get { return channel; }
            set {
                channel = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Invoice subtotal.
        /// </summary>
        public decimal Subtotal
        {
            get { return subtotal; }
            set
            {
                subtotal = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Invoice IVA rate.
        /// </summary>
        public decimal IVA
        {
            get { return iva; }
            set {
                iva = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Invoice IVA amount.
        /// </summary>
        public decimal IvaAmount
        {
            get { return ivaAmount; }
            set
            {
                ivaAmount = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Invoice total.
        /// </summary>
        public decimal Total
        {
            get { return total; }
            set
            {
                total = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

    }
}
