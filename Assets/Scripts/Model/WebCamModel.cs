using UnityEngine;

public class WebCamModel
{
    public WebCamTexture WebCamTexture { get; set; }

    public WebCamModel()
    {
        WebCamTexture = new WebCamTexture();
    }
}
