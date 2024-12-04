using System.Collections.ObjectModel;
using UnityEngine;

public class GalleryViewModel : MonoBehaviour
{
    public Sprite[] galleryImages;  // Array to store the images in the gallery
    public ObservableCollection<ImageData> Images { get; private set; }
    public ObservableCollection<ImageData> FavoriteImages { get; private set; }

    private void Awake()
    {
        Images = new ObservableCollection<ImageData>();
        FavoriteImages = new ObservableCollection<ImageData>();

        foreach (Sprite sprite in galleryImages)
        {
            Images.Add(new ImageData(sprite));
        }
    }

    public void AddToFavorites(ImageData image)
    {
        if (!image.IsFavorite)
        {
            image.IsFavorite = true;
            FavoriteImages.Add(image);
            EventService.RaiseFavoriteAdded(image);
        }
    }

    public void RemoveFromFavorites(ImageData image)
    {
        if (image.IsFavorite)
        {
            image.IsFavorite = false;
            FavoriteImages.Remove(image);
            EventService.RaiseFavoriteRemoved(image);
        }
    }
}
