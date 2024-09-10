using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StepHistoryDisplay : MonoBehaviour
{
    public TextMeshProUGUI dateText;
    public TextMeshProUGUI stepsText;
    public TextMeshProUGUI distanceText;
    public TextMeshProUGUI caloriesText;

    public TextMeshProUGUI dateText1;
    public TextMeshProUGUI stepsText1;
    public TextMeshProUGUI distanceText1;
    public TextMeshProUGUI caloriesText1;

    public TextMeshProUGUI dateText2;
    public TextMeshProUGUI stepsText2;
    public TextMeshProUGUI distanceText2;
    public TextMeshProUGUI caloriesText2;

    void Start()
    {
        DisplayLastFiveEntries();
    }

    void DisplayLastFiveEntries()
    {
        List<(string date, int steps, float distance, float calories)> entries = DatabaseManager.Instance.GetLastThreeEntries();

        if (entries.Count > 0)
        {
            dateText.text = entries[0].date;
            stepsText.text = entries[0].steps.ToString();
            distanceText.text = entries[0].distance.ToString("F2");
            caloriesText.text = entries[0].calories.ToString("F2");

            dateText1.text = entries[1].date;
            stepsText1.text = entries[1].steps.ToString();
            distanceText1.text = entries[1].distance.ToString("F2");
            caloriesText1.text = entries[1].calories.ToString("F2");

            dateText2.text = entries[2].date;
            stepsText2.text = entries[2].steps.ToString();
            distanceText2.text = entries[2].distance.ToString("F2");
            caloriesText2.text = entries[2].calories.ToString("F2");
        }
        else
        {
            dateText.text = "No data";
            stepsText.text = "";
            distanceText.text = "";
            caloriesText.text = "";
        }
    }
}
