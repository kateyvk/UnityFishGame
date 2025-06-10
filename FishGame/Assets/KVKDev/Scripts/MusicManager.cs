using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [SerializeField] private AudioSource musicSource;

    private void Awake()
    {
        // If there's already an instance, destroy this duplicate
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Persist between scenes

        ApplySavedVolume();
    }

    private void Start()
    {
        if (!musicSource.isPlaying)
        {
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    private void ApplySavedVolume()
    {
        float savedVolume = PlayerPrefs.GetFloat("musicVolume", 1f);
        musicSource.volume = savedVolume;
        AudioListener.volume = savedVolume; // Also applies to global audio if needed
    }

    public void UpdateVolume(float newVolume)
    {
        musicSource.volume = newVolume;
        AudioListener.volume = newVolume;
        PlayerPrefs.SetFloat("musicVolume", newVolume);
    }
}
