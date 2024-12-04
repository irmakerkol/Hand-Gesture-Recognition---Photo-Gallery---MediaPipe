using UnityEngine;

public class ImageData
{
    public Sprite ImageSprite { get; set; }
    public bool IsFavorite { get; set; }

    public ImageData(Sprite imageSprite)
    {
        ImageSprite = imageSprite;
        IsFavorite = false;
    }
}
