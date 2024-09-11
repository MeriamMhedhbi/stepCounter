using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class PhotoGallery : MonoBehaviour
{
    public GameObject photoPrefab;  
    public Transform contentPanel;  

    void Start()
    {
        // Validate assignments
        if (photoPrefab == null)
        {
            Debug.LogError("PhotoPrefab is not assigned.");
            return;
        }

        if (contentPanel == null)
        {
            Debug.LogError("ContentPanel is not assigned.");
            return;
        }

        // Display photos
        DisplayPhotos();
    }

    void DisplayPhotos()
    {
        Debug.Log("Content Panel: " + contentPanel);
        Debug.Log("Photo Prefab: " + photoPrefab);

        // Clear existing photos from content panel
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }

        // Load all photos from the directory
        string[] files = Directory.GetFiles(Application.persistentDataPath, "*.png");
        foreach (string file in files)
        {
            // Instantiate the prefab
            GameObject photoObject = Instantiate(photoPrefab, contentPanel);

            // Ensure the prefab contains a RawImage component
            RawImage rawImage = photoObject.GetComponent<RawImage>();

            if (rawImage == null)
            {
                Debug.LogError("RawImage component not found on photoPrefab.");
                continue;
            }

            // Load the image and set it as the texture for RawImage
            byte[] photoBytes = File.ReadAllBytes(file);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(photoBytes);
            rawImage.texture = texture;
        }
    }
}
