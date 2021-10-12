using System;
using System.Data.SQLite;
using System.Diagnostics;

namespace SyncLoopLibrary
{
    public static partial class Database
    {

        /// <summary>
        /// Inserts program into database.
        /// </summary>
        /// <param name="series">The program object to insert.</param>
        /// <returns>ID of inserted program.</returns>
        public static long InsertProgram(ProgramInfo program, int length, Period period)
        {

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                // OPEN CONNECTION.
                connection.Open();

                // CHECK FOR NULLS
                if (program.EpisodeCode == null) program.EpisodeCode = String.Empty;
                if (program.EpisodeNameEnglish == null) program.EpisodeNameEnglish = String.Empty;
                if (program.EpisodeNameSpanish == null) program.EpisodeNameSpanish = String.Empty;
                if (program.EpisodeNumber == null) program.EpisodeNumber = String.Empty;
                if (program.DateDue == null) program.DateDue = DateTime.Now;

                // CREATE QUERY.
                string sql = $"INSERT INTO Programs (SeriesID, ChannelID, Code, NameEnglish, NameSpanish, Number, Length, DateDue, DateDelivered, Rate, RateAmount, Amount, PeriodID) VALUES (";
                sql +=  $"{program.EpisodeSeries.ID}, " +
                        $"{program.EpisodeSeries.ChannelID}, " +
                        $"'{program.EpisodeCode}', " +
                        $"'{program.EpisodeNameEnglish.Replace("'", "''")}', " +
                        $"'{program.EpisodeNameSpanish.Replace("'", "''")}', " +
                        $"'{program.EpisodeNumber}', " +
                        $"{length}, " +
                        $"'{program.DateDue.ToString()}', " +
                        $"'{DateTime.Now.ToString()}', " +
                        $"{(int)program.Rate}, " +
                        $"{program.RateAmount}, " +
                        $"{program.Amount}, " +
                        $"{period.ID})";

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
