using System.Data.SQLite;
using System.Diagnostics;

namespace SyncLoopLibrary
{
    public static partial class Database
    {

        /// <summary>
        /// Inserts period data into database.
        /// </summary>
        /// <param name="month">Current month.</param>
        /// <param name="year">Current year.</param>
        /// <param name="amount">Total amount in Bs.</param>
        /// <param name="programs">Total number of programs.</param>
        /// <param name="dollar">Current dollar price.</param>
        /// <returns>Record ID.</returns>
        public static long InsertPeriodData(int month, int year, decimal amount, int programs, decimal dollar)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                // OPEN CONNECTION.
                connection.Open();
                // CREATE QUERY.
                string sql = $"INSERT INTO Chart (Month, Year, Amount, Programs, Dollar) " +
                             $"VALUES ({month}, {year}, {amount}, {programs}, {dollar})";

                Debug.WriteLine(sql);
                // CREATE COMMAND.
                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    // EXECUTE IT.
                    command.ExecuteNonQuery();
                    // GET ID.
                    long result = connection.LastInsertRowId;
                    // CLOSE CONNECTION.
                    connection.Close();
                    // RETURN ID.
                    return result;
                }
            }
        }
    }
}
