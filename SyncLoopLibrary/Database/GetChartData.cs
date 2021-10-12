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
        /// Gets chart data from database.
        /// </summary>
        /// <returns>ChartData object.</returns>
        public static ChartData GetChartData()
        {
            // Let's create the return object.
            ChartData result = new ChartData();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                // Open connection.
                connection.Open();
                // Create sql string.
                string sql = $"SELECT * FROM Chart";
                // Create command.
                SQLiteCommand command = new SQLiteCommand(sql, connection);
                // Execute it.
                SQLiteDataReader reader = command.ExecuteReader();
                // Create data lists.
                List<int> month = new List<int>();
                List<int> year = new List<int>();
                List<int> programs = new List<int>();
                List<decimal> amounts = new List<decimal>();
                List<decimal> dollars = new List<decimal>();
                List<string> labels = new List<string>();
                // Iterate.
                while (reader.Read())
                {
                    // Add months.
                    month.Add(Convert.ToInt32(reader["Month"]));
                    // Add years.
                    year.Add(Convert.ToInt32(reader["Year"]));
                    // Add amounts.
                    amounts.Add(Convert.ToDecimal(reader["Amount"]));
                    // Add programs.
                    programs.Add(Convert.ToInt32(reader["Programs"]));
                    // Add dollar.
                    dollars.Add(Convert.ToDecimal(reader["Amount"]) / Convert.ToDecimal(reader["Dollar"]));
                    // Labels.
                    labels.Add($"{reader["Month"]}-{reader["Year"]}");
                }
                result.Data.Add(new ColumnSeries
                {
                    Title = "Programs",
                    Values = new ChartValues<int>(programs),
                    Stroke = System.Windows.Media.Brushes.Black,
                    Fill = System.Windows.Media.Brushes.Black,
                    DataLabels = true,
                    ScalesYAt = 0
                });
                result.Data.Add(new ColumnSeries
                {
                    Title = "Bs.",
                    Values = new ChartValues<decimal>(amounts),
                    Stroke = System.Windows.Media.Brushes.Brown,
                    Fill = System.Windows.Media.Brushes.Brown,
                    ScalesYAt = 1
                });
                result.Data.Add(new ColumnSeries
                {
                    Title = "$",
                    Values = new ChartValues<decimal>(dollars),
                    Stroke = System.Windows.Media.Brushes.DarkGreen,
                    Fill = System.Windows.Media.Brushes.DarkGreen,
                    DataLabels = true,
                    ScalesYAt = 2
                });
                result.Labels = labels.ToArray();
            }

            return result;
        }
    }
}
