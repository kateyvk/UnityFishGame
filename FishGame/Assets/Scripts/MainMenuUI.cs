using UnityEngine;
using UnityEngine.SceneManagement;  // For scene loading

public class MainMenuUI : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject optionsPanel;
    public GameObject aboutPanel;
    
    

    public void StartGame()
    {
        // Load the scene named "GameScene" (change this to the name of your actual gameplay scene)
        SceneManager.LoadScene("MainScene");
    }

    public void ShowOptions()
    {
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void ShowAbout()
    {
        mainMenuPanel.SetActive(false);
        aboutPanel.SetActive(true);
    }

    public void BackToMenu()
    {
        optionsPanel.SetActive(false);
        aboutPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void QuitGame()
    {
        // Close the game (only works in builds)
        Application.Quit();
        Debug.Log("Quit Game"); // This is just for logging in the editor
    }
}
