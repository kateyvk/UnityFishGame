using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class FishingManager : MonoBehaviour
{
    public AnyStateAnimator animator;
    public Player player;
    
    [SerializeField] private float minBiteTime = 2f;
    [SerializeField] private float maxBiteTime = 6f;

    private bool isFishing;
    private bool fishOnHook;

    private Coroutine fishingCoroutine;

    void Update()
    {
        if (isFishing && fishOnHook && Keyboard.current.rKey.wasPressedThisFrame)
        {
            CatchFish();
        }
    }

    public void StartFishing()
    {
        if (isFishing) return;

        isFishing = true;
        fishOnHook = false;
        player.DisableMovement();
        animator.TryPlayAnimation("Casting");

        // Delay transitioning to cast idle
        StartCoroutine(TransitionToCastIdle());
    }
    public void StartReeling()
{
    // Stop other animations except Reeling
    animator.TryPlayAnimation("Reeling");
    
    // Optional: disable player movement during reeling
    player.DisableMovement();
}


    private IEnumerator TransitionToCastIdle()
    {
        yield return new WaitForSeconds(1.0f); // Wait for casting animation to play
        animator.TryPlayAnimation("CastIdle");

        fishingCoroutine = StartCoroutine(FishingWait());
    }

    private IEnumerator FishingWait()
    {
        float waitTime = Random.Range(minBiteTime, maxBiteTime);
        yield return new WaitForSeconds(waitTime);

        if (isFishing)
        {
            fishOnHook = true;
            Debug.Log("🎣 Fish on! Press 'R' to reel in!");
            // TODO: play a bite animation or UI feedback
        }
    }

    public void CatchFish()
    {
        if (!fishOnHook) return;

        fishOnHook = false;
        animator.TryPlayAnimation("Reeling");
        Debug.Log("🐟 You caught a fish!");

        StartCoroutine(EndFishingAfterDelay());
    }

    private IEnumerator EndFishingAfterDelay()
    {
        yield return new WaitForSeconds(1.0f); // Wait for reel-in animation
        StopFishing();
    }

    public void StopFishing()
    {
        if (fishingCoroutine != null)
            StopCoroutine(fishingCoroutine);

        animator.TryPlayAnimation("Stand");
        isFishing = false;
        player.EnableMovement();
    }
}
