using UnityEngine;

public class CameraViewModel : MonoBehaviour
{
    public WebCamModel WebCamModel { get; private set; }

    private void Awake()
    {
        WebCamModel = new WebCamModel();
    }

    public void StartWebCam()
    {
        if (WebCamModel.WebCamTexture != null && !WebCamModel.WebCamTexture.isPlaying)
        {
            WebCamModel.WebCamTexture.Play();
        }
    }

    public void StopWebCam()
    {
        if (WebCamModel.WebCamTexture != null && WebCamModel.WebCamTexture.isPlaying)
        {
            WebCamModel.WebCamTexture.Stop();
        }
    }

    public Texture2D TakePhoto()
    {
        if (WebCamModel.WebCamTexture != null && WebCamModel.WebCamTexture.isPlaying)
        {
            Texture2D photo = new Texture2D(WebCamModel.WebCamTexture.width, WebCamModel.WebCamTexture.height);
            photo.SetPixels(WebCamModel.WebCamTexture.GetPixels());
            photo.Apply();
            return photo;
        }
        return null;
    }
}
