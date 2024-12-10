using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GalleryView : MonoBehaviour
{
    public GameObject galleryContent;
    public GameObject favoritesContent;
    public GameObject imagePrefab;
    public GalleryViewModel viewModel;

    private float lastClickTime = 0f; // To track the time of the last click
    private float doubleClickThreshold = 1f; // Time threshold for double-click

    private void Start()
    {
        foreach (ImageData imageData in viewModel.Images)
        {
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

    private void Update()
    {
        // Check for mouse clicks
        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            float timeSinceLastClick = Time.time - lastClickTime;

            if (timeSinceLastClick <= doubleClickThreshold)
            {
                ProcessDoubleClick();
            }

            lastClickTime = Time.time;
        }
    }

    private void ProcessDoubleClick()
    {
        // Perform a raycast to detect the UI element under the cursor
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition // Position of the cursor
        };

        var raycastResults = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, raycastResults);

        foreach (RaycastResult result in raycastResults)
        {
            Image imageComponent = result.gameObject.GetComponent<Image>();
            if (imageComponent != null)
            {
                // Find the corresponding ImageData
                foreach (ImageData imageData in viewModel.Images)
                {
                    if (imageData.ImageSprite == imageComponent.sprite)
                    {
                        if (imageData.IsFavorite)
                        {
                            viewModel.RemoveFromFavorites(imageData);
                        }
                        else
                        {
                            viewModel.AddToFavorites(imageData);
                        }

                        Debug.Log($"Processed like for: {imageData.ImageSprite.name}");
                        return;
                    }
                }
            }
        }
    }

    private void OnGalleryOpen()
    {
        // Clear the gallery content
        foreach (Transform child in galleryContent.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (ImageData imageData in viewModel.Images)
        {
            CreateImageItem(imageData, galleryContent, imageData.IsFavorite);
        }
    }

    private void OnFavoriteAdded(ImageData newFavorite)
    {
        CreateImageItem(newFavorite, favoritesContent, true);
        Debug.Log("Favorite added: " + newFavorite);
        OnGalleryOpen();
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

        OnGalleryOpen();
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
                Debug.Log("Remove button found");
                removeButton.onClick.AddListener(() => viewModel.RemoveFromFavorites(imageData));
            }
        }
        else
        {
            Button favoriteButton = newImage.GetComponentInChildren<Button>();
            favoriteButton.GetComponent<Image>().color = Color.gray;
            if (favoriteButton != null)
            {
                favoriteButton.onClick.AddListener(() => viewModel.AddToFavorites(imageData));
            }
        }
    }
}
