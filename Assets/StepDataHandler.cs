/*using UnityEngine;

public class StepDataHandler : MonoBehaviour
{
    // Singleton setup
    private static StepDataHandler _instance;
    public static StepDataHandler Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<StepDataHandler>();
                if (_instance == null)
                {
                    GameObject container = new GameObject("StepDataHandler");
                    _instance = container.AddComponent<StepDataHandler>();
                }
            }
            return _instance;
        }
    }

    private const string lastRecordedDateKey = "LastRecordedDate";

    // Reference to DatabaseManager
    private DatabaseManager databaseManager;

    private void Awake()
    {
        // Assign the database manager in Awake to ensure it's ready when needed
        databaseManager = DatabaseManager.Instance;
        if (databaseManager == null)
        {
            Debug.LogError("DatabaseManager instance not found!");
        }
    }

    public void SaveDailySteps(int stepCount)
    {
        string currentDateString = System.DateTime.Now.ToString("yyyy-MM-dd");
        databaseManager.InsertUserData(
            currentDateString,
            stepCount,
            StepCounter.Instance.GetDistanceWalked(),
            StepCounter.Instance.GetCaloriesCount()
        );
    }

    /*public void CheckForNewDay()
    {
        string currentDateString = System.DateTime.Now.ToString("yyyyMMdd");
        string lastRecordedDate = PlayerPrefs.GetString(lastRecordedDateKey, currentDateString);

       if (currentDateString != lastRecordedDate)
        {
            ResetDailySteps();
            PlayerPrefs.SetString(lastRecordedDateKey, currentDateString);
       }
        else
        {
            LoadDailySteps();
        }
    }*/

    /*private void ResetDailySteps()
    {
        PlayerPrefs.SetInt("DailySteps", 0);
        StepCounter.Instance.ResetStepData();
        Debug.Log("New day, new steps! Counter reset.");
    }*/

    /*private void LoadDailySteps()
    {

        // Debug.Log("Loading daily steps.");
        string currentDateString = System.DateTime.Now.ToString("yyyy-MM-dd");

        databaseManager.Start();
        // Test inserting data
        databaseManager.InsertUserData("2024-08-29", 1000, 1.5f, 50f);
        Debug.Log("test test test");
        databaseManager.GetUserStepsForDate("2024-08-29");
    }*/
//}



/*using UnityEngine;


public class StepDataHandler : MonoBehaviour
{
    // Singleton setup (similar to StepCounter)
    private static StepDataHandler _instance;
    public static StepDataHandler Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<StepDataHandler>();
                if (_instance == null)
                {
                    GameObject container = new GameObject("StepDataHandler");
                    _instance = container.AddComponent<StepDataHandler>();
                }
            }
            return _instance;
        }
    }

private const string lastRecordedDateKey = "LastRecordedDate";
    private const string dailyStepsKey = "DailySteps";
    public void SaveDailySteps(int stepCount)
    {
        PlayerPrefs.SetInt(dailyStepsKey, stepCount);
    }
    public void CheckForNewDay()
    {
        string currentDateString = System.DateTime.Now.ToString("yyyyMMdd");
        string lastRecordedDate = PlayerPrefs.GetString(lastRecordedDateKey, currentDateString);
        if (currentDateString != lastRecordedDate)
        {
            ResetDailySteps();
            PlayerPrefs.SetString(lastRecordedDateKey, currentDateString);
        }
        else
        {
            LoadDailySteps();
        }
    }
    private void ResetDailySteps()
    {
        PlayerPrefs.SetInt(dailyStepsKey, 0);
        StepCounter.Instance.ResetStepData();
        Debug.Log("New day, new steps! Counter reset.");
    }
    private void LoadDailySteps()
    {
        int stepCount = PlayerPrefs.GetInt(dailyStepsKey, 0);
        StepCounter.Instance.LoadStepData(stepCount);
        Debug.Log("Loaded steps from your last adventure.");
    }
}
*/



using UnityEngine;

public class StepDataHandler : MonoBehaviour
{
    // Singleton setup (similar to StepCounter)
    private static StepDataHandler _instance;
    public static StepDataHandler Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<StepDataHandler>();
                if (_instance == null)
                {
                    GameObject container = new GameObject("StepDataHandler");
                    _instance = container.AddComponent<StepDataHandler>();
                }
            }
            return _instance;
        }
    }

    private DatabaseManager databaseManager;

    private void Start()
    {
        // Initialize DatabaseManager and test inserting/retrieving data
        databaseManager = DatabaseManager.Instance;
        DatabaseManager.Instance.Start(); // This will initialize the database and create the tables

        // Test data insertion
        Debug.Log("Inserting test data into the database...");
        databaseManager.InsertUserData("2024-09-01", 1500, 2.5f, 75f); // Adjust the test values as needed
        Debug.Log("Test data inserted.");

        // Test data retrieval
        Debug.Log("Retrieving data for date '2024-09-01'...");
        databaseManager.GetUserStepsForDate("2024-09-01"); // Adjust the date for testing
    }

    private const string lastRecordedDateKey = "LastRecordedDate";
    private const string dailyStepsKey = "DailySteps";

    public void SaveDailySteps(int stepCount)
    {
        PlayerPrefs.SetInt(dailyStepsKey, stepCount);
    }

    public void CheckForNewDay()
    {
        string currentDateString = System.DateTime.Now.ToString("yyyyMMdd");
        string lastRecordedDate = PlayerPrefs.GetString(lastRecordedDateKey, currentDateString);
        if (currentDateString != lastRecordedDate)
        {
            ResetDailySteps();
            PlayerPrefs.SetString(lastRecordedDateKey, currentDateString);
        }
        else
        {
            LoadDailySteps();
        }
    }

    private void ResetDailySteps()
    {
        PlayerPrefs.SetInt(dailyStepsKey, 0);
        StepCounter.Instance.ResetStepData();
        Debug.Log("New day, new steps! Counter reset.");
    }

    private void LoadDailySteps()
    {
        int stepCount = PlayerPrefs.GetInt(dailyStepsKey, 0); 
        StepCounter.Instance.LoadStepData(stepCount);
        Debug.Log("Loaded steps from your last adventure.");
    }
}
