using UnityEngine;
using UnityEngine.UI;

public class GalleryView : MonoBehaviour
{
    public GameObject galleryContent;
    public GameObject favoritesContent;
    public GameObject imagePrefab;
    public GalleryViewModel viewModel;

    private void Start()
    {
        foreach(ImageData imageData in viewModel.Images)
        {
            Debug.Log("Image data: " + imageData);
            CreateImageItem(imageData, galleryContent, imageData.IsFavorite);
        }
        EventService.OnFavoriteAdded += OnFavoriteAdded;
        EventService.OnFavoriteRemoved += OnFavoriteRemoved;
        EventService.OnGalleryOpen += OnGalleryOpen;
    }

    private void OnDestroy()
    {
        EventService.OnFavoriteAdded -= OnFavoriteAdded;
        EventService.OnFavoriteRemoved -= OnFavoriteRemoved;
        EventService.OnGalleryOpen -= OnGalleryOpen;
    }

    private void OnGalleryOpen()
    {
        // Clear the gallery content
        foreach (Transform child in galleryContent.transform)
        {
            Destroy(child.gameObject);
        }
        
        foreach(ImageData imageData in viewModel.Images)
        {
            CreateImageItem(imageData, galleryContent, imageData.IsFavorite);
        }
   
    }

    private void OnFavoriteAdded(ImageData newFavorite)
    {
        CreateImageItem(newFavorite, favoritesContent, true);
    }

    private void OnFavoriteRemoved(ImageData removedFavorite)
    {
        // Remove the corresponding favorite image from the favorites content
        foreach (Transform child in favoritesContent.transform)
        {
            Image imageComponent = child.GetComponent<Image>();
            if (imageComponent != null && imageComponent.sprite == removedFavorite.ImageSprite)
            {
                Destroy(child.gameObject);
                break;
            }
        }
    }

    private void CreateImageItem(ImageData imageData, GameObject parent, bool isFavorite)
    {
        GameObject newImage = Instantiate(imagePrefab, parent.transform);
        newImage.GetComponent<Image>().sprite = imageData.ImageSprite;

        if (isFavorite)
        {
            Button removeButton = newImage.GetComponentInChildren<Button>();
            if (removeButton != null)
            {
                removeButton.onClick.AddListener(() => viewModel.RemoveFromFavorites(imageData));
            }
        }
        else
        {
            Button favoriteButton = newImage.GetComponentInChildren<Button>();
            if (favoriteButton != null)
            {
                favoriteButton.onClick.AddListener(() => viewModel.AddToFavorites(imageData));
            }
        }
    }
}
