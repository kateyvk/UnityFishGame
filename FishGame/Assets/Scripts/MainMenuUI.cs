using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
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
}
