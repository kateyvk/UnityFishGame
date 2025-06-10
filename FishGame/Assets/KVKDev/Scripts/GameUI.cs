using UnityEngine;
using UnityEngine.SceneManagement;


public class GameUI : MonoBehaviour
{

    public GameObject gamePanel;

    public AudioSource clickSound;

    public void PlayClickSound()
    {
        clickSound?.Play();
    }
    public void BackHome()
    {
        PlayClickSound();
        SceneManager.LoadScene("MainMenu");
    }
}
