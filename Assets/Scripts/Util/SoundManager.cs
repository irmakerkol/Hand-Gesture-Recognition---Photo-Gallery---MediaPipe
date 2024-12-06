using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource audioSource;

    public static SoundManager instance;

    public AudioClip cameraShutterSound;

    public enum Sound
    {
        CameraShutter
    }

    public AudioClip GetAudioClip(Sound sound)
    {
        switch (sound)
        {
            case Sound.CameraShutter:
                return cameraShutterSound;
            default:
                return null;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlaySound(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }

}