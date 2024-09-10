using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 
using UnityEngine.SceneManagement;

public class UserInfoForm : MonoBehaviour
{
    public TMP_Dropdown genderDropdown;       // Dropdown for gender (TextMeshPro version)
    public TMP_InputField heightInputField;   // InputField for height (TextMeshPro version)
    public TMP_InputField weightInputField;
    public Button submitButton;

    private void Start()
    {
        submitButton.onClick.AddListener(OnSubmit);
    }

    private void OnSubmit()
    {
        string gender = genderDropdown.options[genderDropdown.value].text;
        float height = float.Parse(heightInputField.text);
        float weight = float.Parse(weightInputField.text);
        Debug.Log("clicked clicked clicked");
        // Insert user info into the database
        DatabaseManager.Instance.Start();
        DatabaseManager.Instance.InsertUserInfo(gender, height, weight);

        Debug.Log("User info submitted: " + gender + ", Height: " + height + ", Weight: " + weight);
        SceneManager.LoadScene("MainMenu");
    }
}
