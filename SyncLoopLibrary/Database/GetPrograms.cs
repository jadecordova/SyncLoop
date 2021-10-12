using System;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;

namespace SyncLoopLibrary
{
    public static partial class Database
    {

        /// <summary>
        /// Gets programs from database.
        /// </summary>
        /// <returns>Programs.</returns>
        public static ObservableCollection<ProgramInfo> GetPrograms(long channelID,
                                                                    ObservableCollection<Channel> channels,
                                                                    ObservableCollection<Series> series,
                                                                    Period period)
        {

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                ObservableCollection<ProgramInfo> programs = new ObservableCollection<ProgramInfo>();
                // Open connection.
                connection.Open();
                // Create sql string.
                string sql = $"SELECT * FROM Programs WHERE ChannelID = {channelID} AND PeriodID = {period.ID} ORDER BY DateDelivered";
                // Create command.
                SQLiteCommand command = new SQLiteCommand(sql, connection);
                // Execute it.
                SQLiteDataReader reader = command.ExecuteReader();
                // Iterate.

                if(reader.HasRows)
                {
                    while (reader.Read())
                    {
                        // GET OBJECTS.
                        Channel episodeChannel = channels.Single(x => x.ID == channelID);
                        Series episodeSeries = series.Single(x => x.ID == Convert.ToInt64(reader["SeriesID"]));

                        programs.Add(new ProgramInfo
                        {
                            ID = Convert.ToInt64(reader["ID"]),
                            EpisodeSeries = episodeSeries,
                            EpisodeChannel = episodeChannel,
                            PeriodID = Convert.ToInt64(reader["PeriodID"]),
                            EpisodeCode = Convert.ToString(reader["Code"]),
                            EpisodeNameEnglish = Convert.ToString(reader["NameEnglish"]),
                            EpisodeNumber = Convert.ToString(reader["Number"]),
                            Duration = Convert.ToInt32(reader["Length"]),
                            DateDue = Convert.ToDateTime(reader["DateDue"]),
                            DateDelivered = Convert.ToDateTime(reader["DateDelivered"]),
                            Rate = (RateType)Convert.ToInt32(reader["Rate"]),
                            RateAmount = Convert.ToDecimal(reader["RateAmount"]),
                            Amount = Convert.ToDecimal(reader["Amount"])
                        });
                    }
                    return programs;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
