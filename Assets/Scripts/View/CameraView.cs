using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CameraView : MonoBehaviour
{
    public RawImage cameraPreview;
    public Button takePhotoButton;
    private WebCamTexture webCamTexture;
    private List<Texture2D> takenPhotos = new List<Texture2D>();

    private void Start()
    {
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
            // Create a Texture2D with the same dimensions as the WebCamTexture
            Texture2D photo = new Texture2D(webCamTexture.width, webCamTexture.height);
            photo.SetPixels(webCamTexture.GetPixels());
            photo.Apply();

            // Create a new ImageData object
            ImageData imageData = new ImageData(Sprite.Create(photo, new Rect(0, 0, photo.width, photo.height), new Vector2(0.5f, 0.5f)));
            imageData.IsFavorite = false;

            // Add the photo to the list of taken photos
            takenPhotos.Add(photo);

            // Raise the OnPhotoTaken event
            EventService.CreateImaage(imageData);

        }
    }

}
