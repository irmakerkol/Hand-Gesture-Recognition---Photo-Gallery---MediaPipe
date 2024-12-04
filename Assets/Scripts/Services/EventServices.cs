using System;
public static class EventService
{
    public static event Action<ImageData> OnFavoriteAdded;
    public static void RaiseFavoriteAdded(ImageData imageData)
    {
        OnFavoriteAdded?.Invoke(imageData);
    }

    public static event Action<ImageData> OnFavoriteRemoved;
    public static void RaiseFavoriteRemoved(ImageData imageData)
    {
        OnFavoriteRemoved?.Invoke(imageData);
    }

    public static event Action<ImageData> OnPhotoTaken;
    public static void CreateImaage(ImageData imageData)
    {
        OnPhotoTaken?.Invoke(imageData);
    }

    public static event Action OnGalleryOpen;
    public static void OpenGallery()
    {
        OnGalleryOpen?.Invoke();
    }

}