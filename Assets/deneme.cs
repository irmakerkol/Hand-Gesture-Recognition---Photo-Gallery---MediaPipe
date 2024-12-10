using UnityEngine;
using UnityEngine.UI;

public class deneme : MonoBehaviour
{
    public RawImage rawImage; // Assign the RawImage in the Inspector
    private WebCamTexture webCamTexture;

    void Start()
    {
        // List available devices (useful for debugging)
        foreach (var device in WebCamTexture.devices)
        {
            Debug.Log($"Device: {device.name}");
        }

        // Use the virtual camera (replace "OBS Virtual Camera" with your virtual cam name)
        string virtualCamName = "OBS Virtual Camera";
        webCamTexture = new WebCamTexture(virtualCamName);
        rawImage.texture = webCamTexture;
        rawImage.material.mainTexture = webCamTexture;

        // Start the virtual camera feed
        webCamTexture.Play();
    }

    void OnDestroy()
    {
        if (webCamTexture != null && webCamTexture.isPlaying)
        {
            webCamTexture.Stop();
        }
    }
}
