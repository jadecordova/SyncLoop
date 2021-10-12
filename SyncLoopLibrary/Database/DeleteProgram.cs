using System.Data.SQLite;
using System.Diagnostics;

namespace SyncLoopLibrary
{
    public static partial class Database
    {

        /// <summary>
        /// Deletes program from database.
        /// </summary>
        /// <param name="programID">The ID of the program to be deleted.</param>
        public static void DeleteProgram(long programID)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                // OPEN CONNECTION.
                connection.Open();
                // CREATE QUERY.
                string sql = $"DELETE FROM Programs WHERE ID={programID}";

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
