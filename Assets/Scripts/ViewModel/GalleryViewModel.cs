using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class GalleryViewModel : MonoBehaviour
{
    public List<Sprite> galleryImages;  // List to store the images in the gallery
    public ObservableCollection<ImageData> Images { get; private set; }
    public ObservableCollection<ImageData> FavoriteImages { get; private set; }

    private void Awake()
    {
        Images = new ObservableCollection<ImageData>();
        FavoriteImages = new ObservableCollection<ImageData>();

        if (galleryImages != null)
        {
            foreach (Sprite image in galleryImages)
            {
                Images.Add(new ImageData(image));
            }
        }
    }

    private void Start() 
    {
        EventService.OnPhotoTaken += OnPhotoTaken;
    }

    private void OnDestroy() 
    {
        EventService.OnPhotoTaken -= OnPhotoTaken;
    }

    private void OnPhotoTaken(ImageData imageData)
    {
        galleryImages.Add(imageData.ImageSprite);
        Images.Add(imageData);
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
