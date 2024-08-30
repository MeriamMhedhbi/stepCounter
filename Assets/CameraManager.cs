using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;

public class CameraManager : MonoBehaviour
{
    public Button cameraButton;
    public RawImage cameraPreview;  // UI element to display the camera feed

    private WebCamTexture webCamTexture;

    void Start()
    {
        if (cameraButton != null)
        {
            cameraButton.onClick.AddListener(OpenCamera);
        }
        else
        {
            Debug.LogError("Camera Button is not assigned.");
        }

        if (cameraPreview == null)
        {
            Debug.LogError("RawImage for camera preview is not assigned.");
        }
    }

    void OpenCamera()
    {
        Debug.Log("Camera Button Clicked");

        if (Application.platform == RuntimePlatform.Android)
        {
            if (Permission.HasUserAuthorizedPermission(Permission.Camera))
            {
                StartWebCam();
            }
            else
            {
                Permission.RequestUserPermission(Permission.Camera);
            }
        }
        else
        {
            Debug.Log("Camera access is not supported on this platform.");
        }
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus && Application.platform == RuntimePlatform.Android)
        {
            // Check if camera permission is granted and cameraPreview and webCamTexture are initialized
            if (Permission.HasUserAuthorizedPermission(Permission.Camera))
            {
                if (cameraPreview != null && webCamTexture != null)
                {
                    StartWebCam();
                }
                else
                {
                    Debug.LogError("RawImage or WebCamTexture is not initialized.");
                }
            }
            else
            {
                Debug.Log("Camera permission denied.");
            }
        }
    }

    void StartWebCam()
    {
        if (cameraPreview == null)
        {
            Debug.LogError("RawImage for camera preview is not assigned.");
            return;
        }

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
