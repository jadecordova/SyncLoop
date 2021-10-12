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
        public static string GetSVGTemplate()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                string template = String.Empty;
                // Open connection.
                connection.Open();
                // Create sql string.
                string sql = $"SELECT Value FROM Utilities WHERE Resource='SVG Template'";
                // Create command.
                SQLiteCommand command = new SQLiteCommand(sql, connection);
                // Execute it.
                SQLiteDataReader reader = command.ExecuteReader();
                // Iterate.
                while (reader.Read())
                {
                    template = Convert.ToString(reader["Value"]);
                }

                return template;
            }
        }
    }
}
