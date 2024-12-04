using System.Collections.Generic;
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
        // Populate gallery view with images
        foreach (ImageData imageData in viewModel.Images)
        {
            CreateImageItem(imageData, galleryContent, false);
        }

        // Listen for changes in favorites via EventService
        EventService.OnFavoriteAdded += OnFavoriteAdded;
    }

    private void OnDestroy()
    {
        EventService.OnFavoriteAdded -= OnFavoriteAdded;
    }

    private void OnFavoriteAdded(ImageData newFavorite)
    {
        CreateImageItem(newFavorite, favoritesContent, true);
    }
    private void CreateImageItem(ImageData imageData, GameObject parent, bool isFavorite)
    {
        if (imagePrefab == null)
        {
            Debug.LogError("Image prefab is not assigned in the GalleryView script.");
            return;
        }

        GameObject newImage = Instantiate(imagePrefab, parent.transform);
        if (newImage == null)
        {
            Debug.LogError("Failed to instantiate the image prefab.");
            return;
        }

        Image imageComponent = newImage.GetComponent<Image>();
        if (imageComponent == null)
        {
            Debug.LogError("The instantiated prefab does not have an Image component.");
            return;
        }

        imageComponent.sprite = imageData.ImageSprite;

        if (!isFavorite)
        {
            Button favoriteButton = newImage.GetComponentInChildren<Button>();
            if (favoriteButton != null)
            {
                favoriteButton.onClick.AddListener(() => viewModel.AddToFavorites(imageData));
            }
            else
            {
                Debug.LogError("The instantiated prefab does not have a Favorite Button.");
            }
        }
    }


}
