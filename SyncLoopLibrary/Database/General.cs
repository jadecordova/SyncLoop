using System;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace SyncLoopLibrary
{
    public static partial class Database
    {

        #region MEMBERS

        private static string connectionString = $"Data Source={Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SyncLoop.sqlite")};Version=3";

        #endregion



        #region METHODS

        /// <summary>
        /// Inserts single value into specified table.
        /// </summary>
        /// <param name="table">Table name.</param>
        /// <param name="field">Field names.</param>
        /// <param name="value">Value.</param>
        /// <returns>Rows inserted.</returns>
        public static int Insert(string table, string field, string value, bool Unique = true)
        {

            SQLiteCommand command;
            string sql;

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                // Open connection.
                connection.Open();
                // If unique value is required...
                if (Unique)
                {
                    // Create sql string.
                    sql = $"SELECT * FROM {table} WHERE {field}='{value}'";
                    // Create command.
                    using (command = new SQLiteCommand(sql, connection))
                    {
                        // Execute it.
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                // Close connection.
                                connection.Close();
                                // Inform.
                                MessageBox.Show($"The database already contains {value}", "SyncLoop", MessageBoxButton.OK, MessageBoxImage.Warning);
                                // Return.
                                return 0;
                            }
                        }
                    }

                }
                // Create sql string.
                sql = $"insert into {table} ({field}) values ('{value}')";
                // Create command.
                using (command = new SQLiteCommand(sql, connection))
                {
                    // Execute it.
                    int result = command.ExecuteNonQuery();
                    // Close connection.
                    connection.Close();

                    Debug.WriteLine($"ROWS INSERTED: {result}");
                    return result;
                }
            }
        }

        /// <summary>
        /// Inserts multiple values into specified table.
        /// </summary>
        /// <param name="table">Table name.</param>
        /// <param name="fields">Array of field names.</param>
        /// <param name="values">Array of values. Must match the number of fields.</param>
        /// <returns>Rows inserted.</returns>
        public static int Insert(string table, string[] fields, string[] values)
        {
            // If number of values and fields doesn't match...
            if (fields.Length != values.Length)
            {
                // Inform...
                MessageBox.Show("Values number does not match fields number.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                // ...and return 0.
                return 0;
            }
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                // Open connection.
                connection.Open();
                // Create sql string.
                string sql = $"insert into {table} (";
                // Flag for skipping firs item comma.
                int itemNumber = 0;
                // Iterate through fields.
                foreach (string field in fields)
                {
                    if (itemNumber > 0) sql += ",";
                    sql += $"{field}";
                    itemNumber++;
                }
                // Insert values.
                sql += ") values (";
                // Reset counter.
                itemNumber = 0;
                // Iterate.
                foreach (string value in values)
                {
                    if (itemNumber > 0) sql += ",";
                    sql += $"'{value}'";
                    itemNumber++;
                }
                // Close string.
                sql += ")";

                Debug.WriteLine(sql);
                // Create command.
                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    // Execute it.
                    int result = command.ExecuteNonQuery();
                    // Close connection.
                    connection.Close();

                    return result;
                }
            }
        }

        /// <summary>
        /// Deletes record from database.
        /// </summary>
        /// <param name="table">Affected table.</param>
        /// <param name="condition">Condition for deletion.</param>
        /// <returns>Number of affected rows.</returns>
        public static int Delete(string table, string condition)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                // Open connection.
                connection.Open();
                // Create sql string.
                string sql = $"DELETE FROM {table} WHERE {condition}";
                // Create command.
                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    try
                    {
                        int result = command.ExecuteNonQuery();
                        return result;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"There was an error deleting the record. {ex.Message}", "SyncLoop", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return 0;
                    }
                }
            }
        }

        #endregion
    }
}
