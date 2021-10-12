using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SyncLoopLibrary
{
    public static partial class Database
    {

        /// <summary>
        /// Inserts series into database.
        /// </summary>
        /// <param name="channel">The channel object to insert.</param>
        /// <returns>ID of inserted channel.</returns>
        public static long InsertChannel(Channel channel)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                // OPEN CONNECTION.
                connection.Open();
                // CHECK FOR SPECIAL CHARACTERS.
                string code = channel.Code.Replace("'", "''");
                string name = channel.Name.Replace("'", "''");
                // CREATE QUERY.
                string sql = $"INSERT INTO Channels (Code, Name) VALUES (";
                sql += $"'{code}', '{name}')";

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
