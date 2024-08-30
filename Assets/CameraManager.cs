using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
using System.IO;

public class CameraManager : MonoBehaviour
{
    public RawImage cameraPreview;  // UI element to display the camera feed
    public string savePath = "capturedImage.png";  // Path where the image will be saved

    private WebCamTexture webCamTexture;

    void Start()
    {
        if (cameraPreview == null)
        {
            Debug.LogError("RawImage for camera preview is not assigned.");
            return;
        }

        if (Application.platform == RuntimePlatform.Android)
        {
            // Check if the Camera permission is already granted
            if (Permission.HasUserAuthorizedPermission(Permission.Camera))
            {
                StartWebCam();
            }
            else
            {
                // Request the Camera permission
                Permission.RequestUserPermission(Permission.Camera);
            }
        }
        else
        {
            Debug.Log("Camera access is not supported on this platform.");
        }
    }

    void Update()
    {
        // Check if the Camera permission was granted after the request
        if (Application.platform == RuntimePlatform.Android && Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            StartWebCam();
        }
    }

    void StartWebCam()
    {
        if (webCamTexture == null)
        {
            webCamTexture = new WebCamTexture();
            cameraPreview.texture = webCamTexture;
        }

        if (!webCamTexture.isPlaying)
        {
            webCamTexture.Play();  // Start the camera feed
        }

        // Adjust the size of the RawImage to make it larger
        cameraPreview.rectTransform.sizeDelta = new Vector2(Screen.width * 1.5f, Screen.height * 1.5f);

        AdjustCameraPreviewOrientation();
    }

    void OnDisable()
    {
        if (webCamTexture != null && webCamTexture.isPlaying)
        {
            webCamTexture.Stop();  // Stop the camera feed when not needed
        }
    }

    public void CapturePhoto()
    {
        if (webCamTexture == null || !webCamTexture.isPlaying)
        {
            Debug.LogError("WebCamTexture is not initialized or not playing.");
            return;
        }

        // Create a Texture2D from the WebCamTexture
        Texture2D photo = new Texture2D(webCamTexture.width, webCamTexture.height);
        photo.SetPixels(webCamTexture.GetPixels());
        photo.Apply();

        // Encode the texture to PNG
        byte[] photoBytes = photo.EncodeToPNG();

        // Generate a unique filename based on the current timestamp
        string timestamp = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string uniqueFilename = $"capturedImage_{timestamp}.png";

        // Save the image to the file system
        string path = Path.Combine(Application.persistentDataPath, uniqueFilename);
        File.WriteAllBytes(path, photoBytes);

        Debug.Log($"Photo saved to {path}");
    }

    private void AdjustCameraPreviewOrientation()
    {
        if (webCamTexture == null)
        {
            return;
        }

        // Adjust the rotation based on WebCamTexture's videoRotationAngle
        cameraPreview.rectTransform.localEulerAngles = new Vector3(0, 0, -webCamTexture.videoRotationAngle);

        // Adjust the scale to flip the image if necessary
        if (webCamTexture.videoVerticallyMirrored)
        {
            cameraPreview.rectTransform.localScale = new Vector3(1, -1, 1);
        }
        else
        {
            cameraPreview.rectTransform.localScale = new Vector3(1, 1, 1);
        }
    }
}
