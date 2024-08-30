using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;

public class CameraManager : MonoBehaviour
{
    public RawImage cameraPreview;  // UI element to display the camera feed

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
    }

    void OnDisable()
    {
        if (webCamTexture != null && webCamTexture.isPlaying)
        {
            webCamTexture.Stop();  // Stop the camera feed when not needed
        }
    }
}
