using System.Data.SQLite;
using System.Diagnostics;

namespace SyncLoopLibrary
{
    public static partial class Database
    {

        /// <summary>
        /// Updates channel in DB.
        /// </summary>
        /// <param name="program">The channel to update.</param>
        public static void UpdateProgram(ProgramInfo program)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                // OPEN CONNECTION.
                connection.Open();
                // CHECK FOR SINGLE QUOTES AND ESCAPE THEM.
                // ALSO CHECK FOR NULL STRINGS.
                string episodeCode = string.Empty;
                string nameEnglish = string.Empty;
                string nameSpanish = string.Empty;
                string episodeNumber = string.Empty;

                if (program.EpisodeCode != null)
                {
                    episodeCode = program.EpisodeCode.Replace("'", "''");
                }

                if(program.EpisodeNameEnglish != null)
                {
                    nameEnglish = program.EpisodeNameEnglish.Replace("'", "''");
                }
                
                if(program.EpisodeNameSpanish != null)
                {
                    nameSpanish = program.EpisodeNameSpanish.Replace("'", "''");
                }
                
                if(program.EpisodeNumber != null)
                {
                    episodeNumber = program.EpisodeNumber.Replace("'", "''");
                }

                // CREATE QUERY.
                string sql = $"UPDATE Programs SET " +
                    $"SeriesID = {program.EpisodeSeries.ID}, " +
                    $"ChannelID = {program.EpisodeChannel.ID}, " +
                    $"Code = '{episodeCode}', " +
                    $"NameEnglish = '{nameEnglish}', " +
                    $"NameSpanish = '{nameSpanish}', " +
                    $"Number = '{episodeNumber}', " +
                    $"Length = {program.Duration}, " +
                    $"DateDue = '{program.DateDue}', " +
                    $"DateDelivered = '{program.DateDelivered}', " +
                    $"Rate = {(int)program.Rate}, " +
                    $"RateAmount = {program.RateAmount}, " +
                    $"Amount = {program.Amount} " +
                    $"WHERE ID={program.ID}";

                Debug.WriteLine(sql);
                // CREATE COMMAND.
                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    // EXECUTE IT.
                    command.ExecuteNonQuery();
                    // CLOSE CONNECTION.
                    connection.Close();
                }
            }
        }
    }
}
