using UnityEngine;
using UnityEngine.UI;

public class PanelController : MonoBehaviour
{
    public GameObject mainMenuPanel,galleryPanel,favoritesPanel,cameraPanel;
    public Button galleryButton,favoritesButton,cameraButton;

    private void Start()
    {
        galleryButton.onClick.AddListener(ShowGallery);
        favoritesButton.onClick.AddListener(ShowFavorites);
        cameraButton.onClick.AddListener(ShowTakePhoto);
    }

    private void OnDestroy() {
        galleryButton.onClick.RemoveListener(ShowGallery);
        favoritesButton.onClick.RemoveListener(ShowFavorites);
        cameraButton.onClick.RemoveListener(ShowTakePhoto);
    }
    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.M))
        {
            ShowMainMenu();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            ExitApplication();
        }

    }

    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        galleryPanel.SetActive(false);
        favoritesPanel.SetActive(false);
        cameraPanel.SetActive(false);
    }

    public void ShowGallery()
    {
        mainMenuPanel.SetActive(false);
        galleryPanel.SetActive(true);
        favoritesPanel.SetActive(false);
        cameraPanel.SetActive(false);

        EventService.OpenGallery();
    }

    public void ShowFavorites()
    {
        mainMenuPanel.SetActive(false);
        galleryPanel.SetActive(false);
        favoritesPanel.SetActive(true);
        cameraPanel.SetActive(false);
    }

    public void ShowTakePhoto()
    {
        mainMenuPanel.SetActive(false);
        galleryPanel.SetActive(false);
        favoritesPanel.SetActive(false);
        cameraPanel.SetActive(true);
    }

    public void ExitApplication()
    {
        Application.Quit();
        Debug.Log("Application has been quit.");  // Note: This will not work in the editor, but it works in a build.
    }
}