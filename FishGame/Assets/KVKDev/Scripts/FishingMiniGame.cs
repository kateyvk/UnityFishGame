using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;
using TMPro;

public class FishingMinigame : MonoBehaviour
{
    private Action<bool> onComplete;


    public float duration = 15f;            // Total time to complete the minigame
    public float decayRate = 30f;          // How fast progress drops per second
    public float fillPerPress = 10f;       // How much each R button press fills the bar
    public Slider progressBar;             // UI slider to show progress

    private float timer = 0f;
    private float progress = 0f;
    private bool isPlaying = false;
    public GameObject minigamePanel;
    public TMP_Text minigameAlerts;
    public TMP_Text gameTimer;
    public void StartMinigame(Action<bool> callback)
    {
        onComplete = callback;
        progress = 0f;
        timer = 0f;
        isPlaying = true;
        minigamePanel.gameObject.SetActive(true);
        progressBar.gameObject.SetActive(true);
        minigameAlerts.gameObject.SetActive(true);
        gameTimer.gameObject.SetActive(true);
        progressBar.value = 0f;



        ShowAlert("There's a fish on the line, start mashing 'R' to reel it in!!");
    }

    private void Update()
    {
        if (!isPlaying) return;

        timer += Time.deltaTime;

        // Decay progress over time
        progress -= decayRate * Time.deltaTime;

        // Clamp progress
        progress = Mathf.Clamp(progress, 0f, 100f);

        float timeLeft = Mathf.Max(0, duration - timer);
        if (gameTimer != null)
        {
            gameTimer.text = $"Time Left: {timeLeft:F1}s";
        }
        // Detect R key press
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            progress += fillPerPress;
            ShowAlert($" Progress: {progress}/100 Keep smashing that Button!!!");
        }

        // Update UI
        progressBar.value = progress / 100f;

        // Check for win
        if (progress >= 100f)
        {
            CompleteMinigame(true);
        }
        // Check for fail
        else if (timer >= duration)
        {
            CompleteMinigame(false);
        }
    }

    private void CompleteMinigame(bool success)
    {
        isPlaying = false;


        onComplete?.Invoke(success);
        ShowAlert(success ? "Congrats you reeled in a Fish!!" : " The Fish got away!! Keep trying to reel them in");
        minigameAlerts.gameObject.SetActive(false);
        gameTimer.gameObject.SetActive(false);
        progressBar.gameObject.SetActive(false);
        minigamePanel.gameObject.SetActive(false);
    }

    public void ShowAlert(string msg)
    {
        if (minigameAlerts != null)
        {
            minigameAlerts.text = msg;
        }

        //Debug.Log(msg);
    }
    //helping manage ui activation
    public void ShowUI()
    {
        if (minigamePanel != null)
        {
            minigamePanel.SetActive(true);
            ShowAlert(""); // make sure message is cleared

        }
    }

    public void HideUI()
    {
        if (minigamePanel != null)
        {
            minigamePanel.SetActive(false);
        }
    }
}
