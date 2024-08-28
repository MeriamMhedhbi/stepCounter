using UnityEngine;
using System.Data;
using Mono.Data.SqliteClient;
using System.IO;

public class DatabaseManager : MonoBehaviour
{
    private string dbPath;

    private void Start()
    {
        Debug.Log("Start method called.");
        dbPath = GetDatabasePath("StepCounterDB.sqlite");
        Debug.Log("Database path: " + dbPath);
        CreateDatabase();
        CreateTables();
    }

    private string GetDatabasePath(string dbName)
    {
        // Path to the database file
        string path = Path.Combine(Application.persistentDataPath, dbName);
        // Ensure no extra backslashes are added
        return path.Replace('\\', '/'); // Replace backslashes with forward slashes
    }

    private void CreateDatabase()
    {
        // This method is for initializing the database if needed
        if (!File.Exists(dbPath))
        {
            // Create an empty file which SQLite will recognize as a database
            using (FileStream fs = File.Create(dbPath))
            {
                fs.Close();
            }
            Debug.Log("Database created at: " + dbPath);
        }
        else
        {
            Debug.Log("Database already exists at: " + dbPath);
        }
    }

private void CreateTables()
{
    // Update connection string to URI format
    string connectionString = "URI=file:" + dbPath;
    Debug.Log("Connection string: " + connectionString); // Print the connection string

    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();
        using (var command = connection.CreateCommand())
        {
            command.CommandText = @"CREATE TABLE IF NOT EXISTS UserSteps (
                                    ID INTEGER PRIMARY KEY AUTOINCREMENT,
                                    Date TEXT NOT NULL,
                                    Steps INTEGER NOT NULL,
                                    Distance REAL NOT NULL,
                                    Calories REAL NOT NULL
                                    );";
            command.ExecuteNonQuery();
            Debug.Log("Table created or verified.");
        }
    }
}


    public void InsertUserData(string date, int steps, float distance, float calories)
    {
        string connectionString = "Data Source=" + dbPath;
        Debug.Log("Connection string for insert: " + connectionString); // Print the connection string

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO UserSteps (Date, Steps, Distance, Calories) VALUES (@date, @steps, @distance, @calories)";

                // Add parameters explicitly
                var dateParam = command.CreateParameter();
                dateParam.ParameterName = "@date";
                dateParam.Value = date;
                command.Parameters.Add(dateParam);

                var stepsParam = command.CreateParameter();
                stepsParam.ParameterName = "@steps";
                stepsParam.Value = steps;
                command.Parameters.Add(stepsParam);

                var distanceParam = command.CreateParameter();
                distanceParam.ParameterName = "@distance";
                distanceParam.Value = distance;
                command.Parameters.Add(distanceParam);

                var caloriesParam = command.CreateParameter();
                caloriesParam.ParameterName = "@calories";
                caloriesParam.Value = calories;
                command.Parameters.Add(caloriesParam);

                command.ExecuteNonQuery();
                Debug.Log("Data inserted into the database.");
            }
        }
    }

    public void GetUserStepsForDate(string date)
    {
        string connectionString = "Data Source=" + dbPath;
        Debug.Log("Connection string for query: " + connectionString); // Print the connection string

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT Steps, Distance, Calories FROM UserSteps WHERE Date = @date";

                // Add parameters explicitly
                var dateParam = command.CreateParameter();
                dateParam.ParameterName = "@date";
                dateParam.Value = date;
                command.Parameters.Add(dateParam);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int steps = reader.GetInt32(0);
                        float distance = reader.GetFloat(1);
                        float calories = reader.GetFloat(2);

                        Debug.Log($"On {date}: Steps = {steps}, Distance = {distance}, Calories = {calories}");
                    }
                    else
                    {
                        Debug.Log("No data found for this date.");
                    }
                }
            }
        }
    }
}
