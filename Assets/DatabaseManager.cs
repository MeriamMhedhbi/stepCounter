using UnityEngine;
using System.Data;
using Mono.Data.SqliteClient;
using System.IO;
using System.Collections.Generic;


public class DatabaseManager : MonoBehaviour
{

    private static DatabaseManager _instance;
    public static DatabaseManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<DatabaseManager>();
                if (_instance == null)
                {
                    GameObject container = new GameObject("DatabaseManager");
                    _instance = container.AddComponent<DatabaseManager>();
                }
            }
            return _instance;
        }
    }

    private string dbPath;

    public void Start()
    {
        Debug.Log("Start method called.");
        dbPath = GetDatabasePath("StepCounterDB.sqlite");
        Debug.Log("Database path: " + dbPath);
        CreateDatabase();
        CreateTables();

       /* // Test inserting data
        InsertUserData("2024-08-28", 1000, 1.5f, 50f);

        // Test retrieving data
        GetUserStepsForDate("2024-08-28");*/

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

                // Create the UserInfo table
                command.CommandText = @"CREATE TABLE IF NOT EXISTS UserInfo (
                                    ID INTEGER PRIMARY KEY AUTOINCREMENT,
                                    Gender TEXT NOT NULL,
                                    Height REAL NOT NULL,
                                    Weight REAL NOT NULL
                                    );";
                command.ExecuteNonQuery();

                Debug.Log("Table created or verified.");
            }
        }
    }


    public void InsertUserData(string date, int steps, float distance, float calories)
    {
        string connectionString = "URI=file:" + dbPath;
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
        string connectionString = "URI=file:" + dbPath;
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

    public void InsertUserInfo(string gender, float height, float weight)
    {
        string connectionString = "URI=file:" + dbPath;

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO UserInfo (Gender, Height, Weight) VALUES (@gender, @height, @weight)";

                var genderParam = command.CreateParameter();
                genderParam.ParameterName = "@gender";
                genderParam.Value = gender;
                command.Parameters.Add(genderParam);

                var heightParam = command.CreateParameter();
                heightParam.ParameterName = "@height";
                heightParam.Value = height;
                command.Parameters.Add(heightParam);

                var weightParam = command.CreateParameter();
                weightParam.ParameterName = "@weight";
                weightParam.Value = weight;
                command.Parameters.Add(weightParam);

                command.ExecuteNonQuery();
                Debug.Log("User information inserted into the database.");
            }
        }
    }

    public (int id, string gender, float height, float weight) GetUserInfo()
    {
        dbPath = GetDatabasePath("StepCounterDB.sqlite");
        string connectionString = "URI=file:" + dbPath;
        Debug.Log("Connection string for query: " + connectionString); // Print the connection string

        int id = 0;
        string gender = "";
        float height = 0f;
        float weight = 0f;

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT ID,Gender, Height, Weight FROM UserInfo ORDER BY ID DESC LIMIT 1";

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        id = reader.GetInt32(0);
                        gender = reader.GetString(1);
                        height = reader.GetFloat(2);
                        weight = reader.GetFloat(3);

                        Debug.Log($"--------------------------------User Info: Gender = {gender}, Height = {height}, Weight = {weight}");
                    }
                    else
                    {
                        Debug.Log("No user information found.");
                    }
                }
            }
        }

        return (id, gender, height, weight);
    }



    public void UpdateUserInfo(int id, string gender, float height, float weight)
    {
        // Log to verify that the method is being called
        Debug.Log("-************************** updateUser");

        // Get the database path
        dbPath = GetDatabasePath("StepCounterDB.sqlite");
        Debug.Log("Database path: " + dbPath);  // Log the database path for debugging

        // Define the connection string for SQLite
        string connectionString = "URI=file:" + dbPath;
        Debug.Log("Connection string: " + connectionString);  // Log the connection string for debugging

        // Open a connection to the database
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();  // Open the database connection
            Debug.Log("Database connection opened.");  // Log to ensure connection is opened

            using (var command = connection.CreateCommand())
            {
                // Update the existing user info based on the ID
                command.CommandText = @"UPDATE UserInfo 
                                    SET Gender = @gender, Height = @height, Weight = @weight 
                                    WHERE ID = @id";
                Debug.Log("SQL Command: " + command.CommandText);  // Log the SQL query for debugging

                // Add parameters for ID, gender, height, and weight
                var idParam = command.CreateParameter();
                idParam.ParameterName = "@id";
                idParam.Value = id;
                command.Parameters.Add(idParam);
                Debug.Log("ID parameter added: " + id);  // Log the ID parameter

                var genderParam = command.CreateParameter();
                genderParam.ParameterName = "@gender";
                genderParam.Value = gender;
                command.Parameters.Add(genderParam);
                Debug.Log("Gender parameter added: " + gender);  // Log the gender parameter

                var heightParam = command.CreateParameter();
                heightParam.ParameterName = "@height";
                heightParam.Value = height;
                command.Parameters.Add(heightParam);
                Debug.Log("Height parameter added: " + height);  // Log the height parameter

                var weightParam = command.CreateParameter();
                weightParam.ParameterName = "@weight";
                weightParam.Value = weight;
                command.Parameters.Add(weightParam);
                Debug.Log("Weight parameter added: " + weight);  // Log the weight parameter

                // Execute the update command and log the result
                int rowsAffected = command.ExecuteNonQuery();
                Debug.Log("Rows affected: " + rowsAffected);  // Log the number of rows affected by the query


            }
        }
    }


    public List<(string date, int steps, float distance, float calories)> GetLastThreeEntries()
    {
        dbPath = GetDatabasePath("StepCounterDB.sqlite");
        string connectionString = "URI=file:" + dbPath;
        List<(string date, int steps, float distance, float calories)> history = new List<(string, int, float, float)>();

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT Date, Steps, Distance, Calories FROM UserSteps ORDER BY Date DESC LIMIT 3";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string date = reader.GetString(0);
                        int steps = reader.GetInt32(1);
                        float distance = reader.GetFloat(2);
                        float calories = reader.GetFloat(3);

                        history.Add((date, steps, distance, calories));
                    }
                }
            }
        }
        return history;
    }





}
