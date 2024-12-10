using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CameraView : MonoBehaviour
{
    public RawImage cameraPreview;
    public Button takePhotoButton;
    private WebCamTexture webCamTexture;

    private void Start()
    {
          foreach (var device in WebCamTexture.devices)
        {
            Debug.Log($"Device: {device.name}");
        }

        // Use the virtual camera (replace "OBS Virtual Camera" with your virtual cam name)
        string virtualCamName = "OBS Virtual Camera";
        webCamTexture = new WebCamTexture(virtualCamName);
        cameraPreview.texture = webCamTexture;
        cameraPreview.material.mainTexture = webCamTexture;

        // Start the virtual camera feed
        webCamTexture.Play();
        takePhotoButton.onClick.AddListener(TakePhoto);
    }

    private void OnDestroy()
    {
        takePhotoButton.onClick.RemoveListener(TakePhoto);
        StopWebCam();
    }

    private void OnEnable()
    {
        StartCoroutine(InitializeWebCamWithDelay());
    }

    private void OnDisable()
    {
        StopWebCam();
    }

    private IEnumerator InitializeWebCamWithDelay()
    {
        yield return new WaitForEndOfFrame(); // This waits until the next frame, giving the ViewModel time to initialize

        if (webCamTexture == null)
        {
            webCamTexture = new WebCamTexture();
        }

        cameraPreview.texture = webCamTexture;
        webCamTexture.Play();
    }

    private void StartWebCam()
    {
        if (webCamTexture == null)
        {
            webCamTexture = new WebCamTexture();
        }

        cameraPreview.texture = webCamTexture;
        webCamTexture.Play();
    }

    private void StopWebCam()
    {
        if (webCamTexture != null && webCamTexture.isPlaying)
        {
            webCamTexture.Stop();
        }
    }

    public void TakePhoto()
    {
        if (webCamTexture != null && webCamTexture.isPlaying)
        {

            //Play the camera shutter sound
            SoundManager.instance.PlaySound(SoundManager.instance.GetAudioClip(SoundManager.Sound.CameraShutter));

            // Create a Texture2D with the same dimensions as the WebCamTexture
            Texture2D photo = new Texture2D(webCamTexture.width, webCamTexture.height);
            photo.SetPixels(webCamTexture.GetPixels());
            photo.Apply();

            // Create a new ImageData object
            ImageData imageData = new ImageData(Sprite.Create(photo, new Rect(0, 0, photo.width, photo.height), new Vector2(0.5f, 0.5f)));
            imageData.IsFavorite = false;
            
            // Raise the OnPhotoTaken event
            EventService.CreateImaage(imageData);

        }
    }

}
