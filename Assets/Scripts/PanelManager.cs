using UnityEngine;
using UnityEngine.UI;

public class PanelController : MonoBehaviour
{
    public GameObject mainMenuPanel,galleryPanel,favoritesPanel,cameraPanel,helpPanel, navBarPanel;
    public Button galleryButton,favoritesButton,cameraButton,helpButton;

    public Button infoButton, menuButton;

    private void Start()
    {
        galleryButton.onClick.AddListener(ShowGallery);
        favoritesButton.onClick.AddListener(ShowFavorites);
        cameraButton.onClick.AddListener(ShowTakePhoto);
        helpButton.onClick.AddListener(ShowHelp);
        infoButton.onClick.AddListener(ShowHelp);
        menuButton.onClick.AddListener(ShowMainMenu);
    }

    private void OnDestroy() 
    {
        galleryButton.onClick.RemoveListener(ShowGallery);
        favoritesButton.onClick.RemoveListener(ShowFavorites);
        cameraButton.onClick.RemoveListener(ShowTakePhoto);
        helpButton.onClick.RemoveListener(ShowHelp);
        infoButton.onClick.RemoveListener(ShowHelp);
        menuButton.onClick.RemoveListener(ShowMainMenu);
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

    public void ShowHelp()
    {
        SoundManager.instance.PlaySound(SoundManager.instance.GetAudioClip(SoundManager.Sound.ButtonClick));
        mainMenuPanel.SetActive(false);
        galleryPanel.SetActive(false);
        favoritesPanel.SetActive(false);
        cameraPanel.SetActive(false);
        helpPanel.SetActive(true);
        navBarPanel.SetActive(false);
    }

    public void ShowMainMenu()
    {
        SoundManager.instance.PlaySound(SoundManager.instance.GetAudioClip(SoundManager.Sound.ButtonClick));
        mainMenuPanel.SetActive(true);
        galleryPanel.SetActive(false);
        favoritesPanel.SetActive(false);
        cameraPanel.SetActive(false);
        helpPanel.SetActive(false);
        navBarPanel.SetActive(false);
    }

    public void ShowGallery()
    {
        SoundManager.instance.PlaySound(SoundManager.instance.GetAudioClip(SoundManager.Sound.ButtonClick));
        mainMenuPanel.SetActive(false);
        galleryPanel.SetActive(true);
        favoritesPanel.SetActive(false);
        cameraPanel.SetActive(false);
        helpPanel.SetActive(false);
        navBarPanel.SetActive(true);

        EventService.OpenGallery();
    }

    public void ShowFavorites()
    {
        SoundManager.instance.PlaySound(SoundManager.instance.GetAudioClip(SoundManager.Sound.ButtonClick));
        mainMenuPanel.SetActive(false);
        galleryPanel.SetActive(false);
        favoritesPanel.SetActive(true);
        cameraPanel.SetActive(false);
        helpPanel.SetActive(false);
        navBarPanel.SetActive(true);
    }

    public void ShowTakePhoto()
    {
        SoundManager.instance.PlaySound(SoundManager.instance.GetAudioClip(SoundManager.Sound.ButtonClick));
        mainMenuPanel.SetActive(false);
        galleryPanel.SetActive(false);
        favoritesPanel.SetActive(false);
        cameraPanel.SetActive(true);
        helpPanel.SetActive(false);
        navBarPanel.SetActive(true);
    }

    public void ExitApplication()
    {
        Application.Quit();
        Debug.Log("Application has been quit.");  // Note: This will not work in the editor, but it works in a build.
    }
}
