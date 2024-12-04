using System;
public static class EventService
{
    public static event Action<ImageData> OnFavoriteAdded;

    public static void RaiseFavoriteAdded(ImageData imageData)
    {
        OnFavoriteAdded?.Invoke(imageData);
    }
}