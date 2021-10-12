using System;
using System.Collections.ObjectModel;
using System.Data.SQLite;

namespace SyncLoopLibrary
{
    public static partial class Database
    {

        /// <summary>
        /// Gets most recent period from database.
        /// </summary>
        /// <returns>Most recent period.</returns>
        public static Period GetPeriod()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                Period period = new Period();
                // Open connection.
                connection.Open();
                // Create sql string.
                string sql = $"SELECT * FROM Periods ORDER BY ID DESC LIMIT 1";
                // Create command.
                SQLiteCommand command = new SQLiteCommand(sql, connection);
                // Execute it.
                SQLiteDataReader reader = command.ExecuteReader();
                // CHECK FOR ROWS.
                if (reader.HasRows)
                {
                    reader.Read();
                    period.ID = Convert.ToInt64(reader["ID"]);
                    period.StartDate = Convert.ToDateTime(reader["StartDate"]);
                    period.EndDate = Convert.ToDateTime(reader["EndDate"]);
                    return period;
                }
                return null;
            }
        }
    }
}
