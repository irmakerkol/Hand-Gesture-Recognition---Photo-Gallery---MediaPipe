using System;
public static class EventService
{
    public static event Action<ImageData> OnFavoriteAdded;
    public static event Action<ImageData> OnFavoriteRemoved;

    public static void RaiseFavoriteAdded(ImageData imageData)
    {
        OnFavoriteAdded?.Invoke(imageData);
    }

    public static void RaiseFavoriteRemoved(ImageData imageData)
    {
        OnFavoriteRemoved?.Invoke(imageData);
    }
}