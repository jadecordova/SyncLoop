using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows;

namespace SyncLoopLibrary
{
    /// <summary>
    /// Program information.
    /// </summary>
    public class ProgramInfo : Notifier
    {

        #region FIELDS

        private long id;
        private Channel episodeChannel;
        private Series episodeSeries;
        private string episodeCode;
        private string episodeNumber;
        private string episodeNameEnglish;
        private string episodeNameSpanish;
        private string projectDirectory;
        private string documentName;
        private DateTime dateDue;
        private DateTime dateDelivered;
        private RateType rate;
        private decimal rateAmount;
        private long periodID;
        private int duration;
        private decimal amount;

        #endregion



        #region PROPERTIES

        /// <summary>
        /// Program ID.
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
        /// Episode channel.
        /// </summary>
        public Channel EpisodeChannel
        {
            get { return episodeChannel; }
            set
            {
                episodeChannel = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Episode series.
        /// </summary>
        public Series EpisodeSeries
        {
            get { return episodeSeries; }
            set
            {
                episodeSeries = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Episode code.
        /// </summary>
        public string EpisodeCode
        {
            get { return episodeCode; }
            set
            {
                episodeCode = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Episode number.
        /// </summary>
        public string EpisodeNumber
        {
            get { return episodeNumber; }
            set
            {
                episodeNumber = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Episode name in English.
        /// </summary>
        public string EpisodeNameEnglish
        {
            get { return episodeNameEnglish; }
            set
            {
                episodeNameEnglish = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Episode name in Spanish.
        /// </summary>
        public string EpisodeNameSpanish
        {
            get { return episodeNameSpanish; }
            set
            {
                episodeNameSpanish = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Current project directory.
        /// </summary>
        public string ProjectDirectory
        {
            get { return projectDirectory; }
            set
            {
                projectDirectory = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Document name.
        /// </summary>
        public string DocumentName
        {
            get { return documentName; }
            set
            {
                documentName = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Program due date.
        /// </summary>
        public DateTime DateDue
        {
            get { return dateDue; }
            set
            {
                dateDue = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Program delivered date.
        /// </summary>
        public DateTime DateDelivered
        {
            get { return dateDelivered; }
            set
            {
                dateDelivered = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Program rate type.
        /// </summary>
        public RateType Rate
        {
            get { return rate; }
            set
            {
                rate = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Program rate amount.
        /// </summary>
        public decimal RateAmount
        {
            get { return rateAmount; }
            set
            {
                rateAmount = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Program period ID.
        /// </summary>
        public long PeriodID
        {
            get { return periodID; }
            set
            {
                periodID = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Program duration in minutes.
        /// </summary>
        public int Duration
        {
            get { return duration; }
            set
            {
                duration = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Program total amount.
        /// </summary>
        public decimal Amount
        {
            get { return amount; }
            set {
                amount = value;
                NotifyPropertyChanged();
            }
        }


        #endregion



        #region CONSTRUCTORS

        /// <summary>
        /// Default.
        /// </summary>
        public ProgramInfo()
        {

        }

        #endregion



        #region METHODS

        /// <summary>
        /// Calculates programa total price.
        /// </summary>
        public void UpdateProgram(Rates rates)
        {
            switch (Rate)
            {
                case RateType.Normal:
                    RateAmount = rates.Normal;
                    break;
                case RateType.Rush:
                    RateAmount = rates.Rush;
                    break;
                case RateType.Less_than_48_hours:
                    RateAmount = rates.LessThan48Hours;
                    break;
            }

            Amount = RateAmount * Duration;

            Database.UpdateProgram(this);
        }

        /// <summary>
        /// Saves program data to file and DB.
        /// </summary>
        public async void Save(string path)
        {
            if (!String.IsNullOrEmpty(path))
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(path))
                    {
                        await writer.WriteAsync(JsonConvert.SerializeObject(this, Formatting.Indented));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"There was an error writing the program info file: {ex.Message}", 
                                     "SyncLoop",
                                     MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        /// <summary>
        /// Loads program info and all related files.
        /// </summary>
        public void Load(string file)
        {
            string json;
            // Return object.
            ProgramInfo info = null;

            // Check if file is valid.
            if (!String.IsNullOrEmpty(file))
            {
                if (File.Exists(file))
                {
                    try
                    {
                        using (StreamReader reader = new StreamReader(file))
                        {
                            json = reader.ReadToEnd();
                        }
                        if (!String.IsNullOrEmpty(json))
                        {
                            try
                            {
                                info = JsonConvert.DeserializeObject<ProgramInfo>(json);
                                // SET VALUES.
                                EpisodeChannel = info.EpisodeChannel;
                                EpisodeCode = info.EpisodeCode;
                                EpisodeNumber = info.EpisodeNumber;
                                EpisodeNameEnglish = info.EpisodeNameEnglish;
                                EpisodeNameSpanish = info.EpisodeNameSpanish;
                                EpisodeSeries = info.EpisodeSeries;
                                ProjectDirectory = info.ProjectDirectory;
                                DocumentName = info.DocumentName;
                                Rate = info.Rate;
                                RateAmount = info.RateAmount;
                                DateDue = info.DateDue;
                                DateDelivered = info.DateDelivered;
                                PeriodID = info.PeriodID;
                                Duration = info.Duration;
                                Amount = info.Amount;
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show($"There was an error reading the program info file contents: {e.Message}", 
                                                 "SyncLoop", 
                                                 MessageBoxButton.OK, MessageBoxImage.Warning);

                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Program info file is invalid.", 
                                            "SyncLoop", 
                                            MessageBoxButton.OK, MessageBoxImage.Warning);

                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"There was an error reading the program info file: {ex.Message}", 
                                         "SyncLoop",
                                         MessageBoxButton.OK, MessageBoxImage.Warning);

                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Program info file doesn't exist.", 
                                    "SyncLoop", 
                                    MessageBoxButton.OK, MessageBoxImage.Warning);

                    return;
                }
            }
            else
            {
                MessageBox.Show("No program info was loaded.",
                                "SyncLoop",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        #endregion
    }
}
