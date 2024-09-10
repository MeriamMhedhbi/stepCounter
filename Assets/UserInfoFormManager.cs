using UnityEngine;
using TMPro;

public class UserInfoFormManager : MonoBehaviour
{
    public TMP_Dropdown genderDropdown;      // Dropdown for gender
    public TMP_InputField heightInputField;  // InputField for height
    public TMP_InputField weightInputField;  // InputField for weight
    public TMP_Text statusText;              // To display save/update status (optional)

    private DatabaseManager databaseManager;

    void Start()
    {
        Debug.Log("-**************************heyyyyyyyyyyyyyyyyyyyy ");
        // Initialize the database manager
        databaseManager = DatabaseManager.Instance;
        Debug.Log("-**************************data from query ");
        // Load existing user info
        LoadUserInfo();
    }

    void LoadUserInfo()
    {
        databaseManager.GetUserInfo();
        // Get user info from the database
        var (id, gender, height, weight) = databaseManager.GetUserInfo();

        // Populate the UI fields
        if (!string.IsNullOrEmpty(gender))
        {
            genderDropdown.value = genderDropdown.options.FindIndex(option => option.text == gender);
        }
        heightInputField.text = height.ToString();
        weightInputField.text = weight.ToString();

        Debug.Log("-**************************data from query " + height.ToString() + weight.ToString());
    }

    public void SaveUserInfo()
    {
        Debug.Log("-**************************clicked update ");
        // Get data from the UI fields
        var (id,gender, height, weight) = databaseManager.GetUserInfo();
        string newGender = genderDropdown.options[genderDropdown.value].text;
        float newHeight = float.Parse(heightInputField.text);
        float newWeight = float.Parse(weightInputField.text);
        Debug.Log("-**************************");
        Debug.Log(newGender);
        Debug.Log(newHeight);
        Debug.Log(newWeight);
        Debug.Log("-**************************");
        // Save or update the info in the database
        databaseManager.UpdateUserInfo(id, newGender, newHeight, newWeight);

    }
}
