using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class FishingMinigame : MonoBehaviour
{
    private Action<bool> onComplete;

    [Header("Minigame Settings")]
    public float duration = 5f;            // Total time allowed
    public int requiredPresses = 15;       // Presses needed to win

    private int currentPresses = 0;
    private float timer = 0f;
    private bool isPlaying = false;

    public void StartMinigame(Action<bool> callback)
    {
        onComplete = callback;
        currentPresses = 0;
        timer = 0f;
        isPlaying = true;

        Debug.Log("🎮 Mash 'R' to reel in the fish!");
    }

    private void Update()
    {
        if (!isPlaying) return;

        timer += Time.deltaTime;

        // Count each R key press
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            currentPresses++;
            Debug.Log($"🎯 Reel presses: {currentPresses}/{requiredPresses}");
        }

        // Success
        if (currentPresses >= requiredPresses)
        {
            CompleteMinigame(true);
        }
        // Fail if time runs out
        else if (timer >= duration)
        {
            CompleteMinigame(false);
        }
    }

    private void CompleteMinigame(bool success)
    {
        isPlaying = false;
        onComplete?.Invoke(success);
    }
}
