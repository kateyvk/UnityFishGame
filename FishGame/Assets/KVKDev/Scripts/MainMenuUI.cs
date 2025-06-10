using System.Data.Common;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;
    public GameObject mainMenuPanel;
    public GameObject optionsPanel;
    public GameObject aboutPanel;
    public AudioSource clickSound;

    public void PlayClickSound()
    {
        clickSound?.Play();
    }

    public void StartGame()
    {
        PlayClickSound();
        SceneManager.LoadScene("MainScene");
    }

    public void ShowOptions()
    {
        PlayClickSound();
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void ShowAbout()
    {
        PlayClickSound();
        mainMenuPanel.SetActive(false);
        aboutPanel.SetActive(true);
    }

    public void BackToMenu()
    {
        PlayClickSound();
        optionsPanel.SetActive(false);
        aboutPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void QuitGame()
    {
        PlayClickSound();
        Application.Quit();
        Debug.Log("Quit Game");
    }
    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        SaveVol();
    }
    private void SaveVol()
    {
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
    }
    private void LoadVol()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }
    void Start()
    {
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
            LoadVol();
        }
        else
        {
            LoadVol();
        }
        
    }
}
