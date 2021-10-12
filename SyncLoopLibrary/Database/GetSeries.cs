using System;
using System.Collections.ObjectModel;
using System.Data.SQLite;

namespace SyncLoopLibrary
{
    public static partial class Database
    {

        /// <summary>
        /// Gets channels from database.
        /// </summary>
        /// <returns>Channels.</returns>
        public static ObservableCollection<Series> GetSeries()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                ObservableCollection<Series> series = new ObservableCollection<Series>();
                // Open connection.
                connection.Open();
                // Create sql string.
                string sql = $"SELECT * FROM Series";
                // Create command.
                SQLiteCommand command = new SQLiteCommand(sql, connection);
                // Execute it.
                SQLiteDataReader reader = command.ExecuteReader();
                // Iterate.
                while (reader.Read())
                {
                    series.Add(new Series
                    {
                        ID = Convert.ToInt64(reader["ID"]),
                        ChannelID = Convert.ToInt64(reader["ChannelID"]),
                        NameEnglish = Convert.ToString(reader["NameEnglish"]),
                        NameSpanish = Convert.ToString(reader["NameSpanish"])
                    });
                }

                return series;
            }
        }
    }
}
