using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;

namespace SyncLoopLibrary
{
    public static partial class Database
    {

        /// <summary>
        /// Gets period data from database.
        /// </summary>
        /// <returns>Number of programs and total amount.</returns>
        public static Tuple<int, decimal> GetPeriodData(long period)
        {
            // Let's create the return object.
            Tuple<int, decimal> result = null;
            // Temporal variables.
            int programs = 0;
            decimal total = 0;

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                // Open connection.
                connection.Open();
                // Create sql string.
                string sql = $"SELECT count(*) as NumberOfPrograms, sum(Amount) as AmountBs from Programs WHERE PeriodID = {period}";
                // Create command.
                SQLiteCommand command = new SQLiteCommand(sql, connection);
                // Execute it.
                SQLiteDataReader reader = command.ExecuteReader();
                // Iterate.
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        programs  = Convert.ToInt32(reader["NumberOfPrograms"]);
                        total = Convert.ToDecimal(reader["AmountBs"]);
                    }
                    result = new Tuple<int, decimal>(programs, total);
                }
            }

            return result;
        }
    }
}
