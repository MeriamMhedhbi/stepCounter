using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StepCounter : MonoBehaviour
{
    public Text StepsText;
    public Text DistanceText;
    public Text CaloriesText;

    private static StepCounter _instance;
    public static StepCounter Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<StepCounter>();
                if (_instance == null)
                {
                    GameObject container = new GameObject("StepCounter");
                    _instance = container.AddComponent<StepCounter>();
                }
            }
            return _instance;
        }
    }

    [Header("Configuration")]
    public StepCounterConfig config;
    [Header("Runtime Variables")]
    [SerializeField] private float distanceWalked = 0f;
    [SerializeField] private int stepCount = 0;
    [SerializeField] private float caloriesCountVal = 0f;

    private Vector3 acceleration;
    private Vector3 prevAcceleration;

    private Vector2 prevGPSPosition;
    private bool isGPSInitialized = false;

    private void Start()
    {
        if (config == null)
        {
            Debug.LogError("Oops! StepCounterConfig is missing!");
            return;
        }

        prevAcceleration = Input.acceleration;

        StepDataHandler.Instance.CheckForNewDay();

        StartCoroutine(InitializeGPS());
        StartCoroutine(UpdateRoutine());
    }

    private IEnumerator InitializeGPS()
    {
        if (!Input.location.isEnabledByUser)
        {
            Debug.LogError("GPS not enabled by user!");
            yield break;
        }

        Input.location.Start();
        
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (maxWait <= 0 || Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.LogError("GPS initialization failed.");
            yield break;
        }

        isGPSInitialized = true;
        prevGPSPosition = new Vector2(Input.location.lastData.latitude, Input.location.lastData.longitude);
        Debug.Log("GPS initialized.");
    }

    private IEnumerator UpdateRoutine()
    {
        while (true)
        {
            if (config == null) yield break;

            DetectSteps();
            CalculateDistance();
            CalculateCalories();
            StepDataHandler.Instance.SaveDailySteps(stepCount);

            try
            {
                StepsText.text = stepCount.ToString();
                DistanceText.text = distanceWalked.ToString("F2");
                CaloriesText.text = caloriesCountVal.ToString("F2");
            }
            catch { }

            yield return new WaitForSeconds(1f);
        }
    }

    private void DetectSteps()
    {
        acceleration = Input.acceleration;
        float delta = (acceleration - prevAcceleration).magnitude;
        if (delta > config.threshold)
        {
            stepCount++;
            Debug.Log($"Step detected! Count: {stepCount}");
        }
        prevAcceleration = acceleration;
    }

    private void CalculateDistance()
    {
        // Calculate distance from step count
        float distanceFromSteps = stepCount * config.stepLength;

        // Calculate GPS-based distance
        float gpsDistance = 0f;
        if (isGPSInitialized)
        {
            Vector2 currentGPSPosition = new Vector2(Input.location.lastData.latitude, Input.location.lastData.longitude);
            gpsDistance = Vector2.Distance(prevGPSPosition, currentGPSPosition) * 1000f; // Distance in meters
            prevGPSPosition = currentGPSPosition;
        }

        // Combine both methods for a more accurate distance measurement
        distanceWalked = distanceFromSteps + gpsDistance;
    }

    private void CalculateCalories()
    {
        float caloriesPerStep = 0.05f;
        caloriesCountVal = stepCount * caloriesPerStep;
    }

    public void CalibrateStepLength(float newStepLength)
    {
        if (newStepLength > 0)
        {
            config.stepLength = newStepLength;
            Debug.Log($"Step length calibrated to: {config.stepLength} meters");
        }
        else
        {
            Debug.LogWarning("Whoops! That's not a valid step length.");
        }
    }

    public float GetDistanceWalked() => distanceWalked;
    public int GetStepCount() => stepCount;
    public float GetCaloriesCount() => caloriesCountVal;
    public void ResetStepData()
    {
        stepCount = 0;
        distanceWalked = 0f;
        caloriesCountVal = 0f;
    }

    public void LoadStepData(int loadedStepCount)
    {
        stepCount = loadedStepCount;
        CalculateDistance();
        CalculateCalories();
    }
}
