using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace SyncLoopLibrary
{
    /// <summary>
    /// Interaction logic for ChannelReport.xaml
    /// </summary>
    public partial class ChannelReport : UserControl
    {

        #region MEMBERS

        ObservableCollection<Channel> channels;

        ObservableCollection<Series> series;

        #endregion



        #region DEPENDENCY PROPERTIES

        public static DependencyProperty ProgramsProperty;
        public static DependencyProperty ChannelProperty;
        public static DependencyProperty SubtotalProperty;
        public static DependencyProperty IvaProperty;
        public static DependencyProperty IvaAmountProperty;
        public static DependencyProperty TotalProperty;

        #endregion



        #region ROUTED EVENTS

        public static readonly RoutedEvent IvaChangedEvent;
        public static readonly RoutedEvent SubtotalChangedEvent;

        #endregion



        #region PROPERTY WRAPPERS

        /// <summary>
        /// Collection of program info objects.
        /// </summary>
        public ObservableCollection<ProgramInfo> Programs
        {
            get { return (ObservableCollection<ProgramInfo>)GetValue(ProgramsProperty); }
            set { SetValue(ProgramsProperty, value); }
        }

        /// <summary>
        /// Channel.
        /// </summary>
        public Channel Channel
        {
            get { return (Channel)GetValue(ChannelProperty); }
            set { SetValue(ChannelProperty, value); }
        }

        /// <summary>
        /// Subtotal.
        /// </summary>
        public decimal Subtotal
        {
            get { return (decimal)GetValue(SubtotalProperty); }
            set { SetValue(SubtotalProperty, value); }
        }

        /// <summary>
        /// IVA rate.
        /// </summary>
        public decimal IVA
        {
            get { return (decimal)GetValue(IvaProperty); }
            set { SetValue(IvaProperty, value); }
        }

        /// <summary>
        /// IVA amount.
        /// </summary>
        public decimal IVAamount
        {
            get { return (decimal)GetValue(IvaAmountProperty); }
            set { SetValue(IvaAmountProperty, value); }
        }

        /// <summary>
        /// Grand total.
        /// </summary>
        public decimal Total
        {
            get { return (decimal)GetValue(TotalProperty); }
            set { SetValue(TotalProperty, value); }
        }

        #endregion



        #region EVENT WRAPPERS

        /// <summary>
        /// IVA changed event.
        /// </summary>
        public event RoutedPropertyChangedEventHandler<ProgramInfo> IvaChanged
        {
            add { AddHandler(IvaChangedEvent, value); }
            remove { RemoveHandler(IvaChangedEvent, value); }
        }

        /// <summary>
        /// Subtotal IVA changed event.
        /// </summary>
        public event RoutedPropertyChangedEventHandler<ProgramInfo> SubtotalIvaChanged
        {
            add { AddHandler(SubtotalChangedEvent, value); }
            remove { RemoveHandler(SubtotalChangedEvent, value); }
        }

        #endregion



        #region CONSTRUCTORS

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="channels">Channels collection.</param>
        /// <param name="series">Series collection.</param>
        /// <param name="programs">Program info collection.</param>
        public ChannelReport(ObservableCollection<Channel> channels, 
                             ObservableCollection<Series> series,
                             ObservableCollection<ProgramInfo> programs)
        {
            InitializeComponent();
            // SAVE PARAMETERS.
            this.channels = channels;
            this.series = series;
            Programs = programs;
            DataContext = programs;
            // SUBSCRIBE.
            Loaded += ChannelReportLoaded;
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        static ChannelReport()
        {
            // REGISTER DEPENDENCY PROPERTIES
            ProgramsProperty = DependencyProperty.Register(
                "Programs",
                typeof(ObservableCollection<ProgramInfo>),
                typeof(ChannelReport),
                new FrameworkPropertyMetadata(new PropertyChangedCallback(OnProgramsChanged)));

            ChannelProperty = DependencyProperty.Register(
                "Channel",
                typeof(Channel),
                typeof(ChannelReport),
                new FrameworkPropertyMetadata(new PropertyChangedCallback(OnChannelChanged)));

            SubtotalProperty = DependencyProperty.Register(
                "Subtotal",
                typeof(decimal),
                typeof(ChannelReport),
                new FrameworkPropertyMetadata(new PropertyChangedCallback(OnSubtotalChanged)));

            IvaProperty = DependencyProperty.Register(
                "IVA",
                typeof(decimal),
                typeof(ChannelReport),
                new FrameworkPropertyMetadata(new PropertyChangedCallback(OnIvaChanged)));

            IvaAmountProperty = DependencyProperty.Register(
                "IVAamount",
                typeof(decimal),
                typeof(ChannelReport),
                new FrameworkPropertyMetadata(new PropertyChangedCallback(OnIvaAmountChanged)));

            TotalProperty = DependencyProperty.Register(
                "Total",
                typeof(decimal),
                typeof(ChannelReport),
                new FrameworkPropertyMetadata(new PropertyChangedCallback(OnTotalChanged)));

            // REGISTER ROUTED EVENTS
            IvaChangedEvent = EventManager.RegisterRoutedEvent(
                "ProgramChanged", 
                RoutingStrategy.Bubble,
                typeof(RoutedPropertyChangedEventHandler<ProgramInfo>),
                typeof(ChannelReport));

            SubtotalChangedEvent = EventManager.RegisterRoutedEvent(
                "SubtotalChanged", 
                RoutingStrategy.Bubble,
                typeof(RoutedPropertyChangedEventHandler<decimal>),
                typeof(ChannelReport));
        }

        #endregion



        #region EVENT HANDLERS

        private void ChannelReportLoaded(object sender, RoutedEventArgs e)
        {
            // CALCULATE TOTALS.
            GetTotals();
            // SET SOURCE.
            ReportGrid.ItemsSource = (ObservableCollection<ProgramInfo>)DataContext;
            ChannelsColumn.ItemsSource = channels;
            SeriesColumn.ItemsSource = series;
            SubtotalBlock.DataContext = Subtotal;
            IvaBlock.DataContext = IVAamount;
            TotalBlock.DataContext = Total;
        }

        #endregion



        #region CALLBACKS

        private static void OnProgramsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
        }

        private static void OnChannelChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {

        }
        private static void OnSubtotalChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
           ((ChannelReport)sender).SubtotalBlock.Text = String.Format("{0:N}", e.NewValue);
        }

        private static void OnIvaChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
        }

        private static void OnIvaAmountChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((ChannelReport)sender).IvaBlock.Text = String.Format("{0:N}", e.NewValue);
        }

        private static void OnTotalChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((ChannelReport)sender).TotalBlock.Text = String.Format("{0:N}", e.NewValue);
        }

        #endregion



        #region METHODS

        /// <summary>
        /// Gets totals.
        /// </summary>
        public void GetTotals()
        {
            // RESET.
            Subtotal = 0;
            IVAamount = 0;
            Total = 0;
            foreach (ProgramInfo program in (ObservableCollection<ProgramInfo>)DataContext)
            {
                Subtotal += program.Amount;
            }
            IVAamount += Subtotal * (IVA / 100);
            Total += Subtotal + IVAamount;
        }

        #endregion
    }
}
