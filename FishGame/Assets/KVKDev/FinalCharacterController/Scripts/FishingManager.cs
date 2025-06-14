using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class FishingManager : MonoBehaviour
{
    public Player player;
    public AnyStateAnimator animator;
    public FishingMinigame minigame;


    //min and max time before a fish will bite
    [SerializeField] private float minBiteTime = 10f;
    [SerializeField] private float maxBiteTime = 30f;

    private Coroutine biteCoroutine; //storing the bite timer so it can be stopped as needed
    private FishingState currentState = FishingState.Idle; //current fishing state 

    private enum FishingState
    {
        Idle,
        Casting,
        Waiting,
        Bite,
        Minigame,
        Success,
        Fail
    }

    void Update()
    {
        // you can escape the fishing with esc
        if (currentState != FishingState.Idle && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            StopAllCoroutines(); //stop casting or bite timer
            StopFishing(); //reset state to idle
            Debug.Log("Fishing cancelled.");
        }
    }

    public void StartFishing()
    {
        //if the player is in any other state you cannot call this method
        if (currentState != FishingState.Idle) return;

        currentState = FishingState.Casting;
        player.DisableMovement(); //prevents the player from moving when casting
        animator.TryPlayAnimation("Casting");

        StartCoroutine(CastingSequence());
    }

    private IEnumerator CastingSequence()
    {
        yield return new WaitForSeconds(1f); // wait for cast animation to finish
        animator.TryPlayAnimation("CastIdle");// switch back to idle after the cast
        

        currentState = FishingState.Waiting; //start the waiting for fish to bite state 
        biteCoroutine = StartCoroutine(BiteWait()); //randomize wait time 
    }
    //wait random duration before starting a bite
    private IEnumerator BiteWait()
    {
        float waitTime = Random.Range(minBiteTime, maxBiteTime);
        Debug.Log($"Bite will happen in {waitTime:F2} seconds");
        minigame.ShowAlert("Waiting for a bite...");
        yield return new WaitForSeconds(waitTime);
        
        minigame.ShowAlert($"waitTime");
        
        if (currentState == FishingState.Waiting)
        {
            currentState = FishingState.Bite;
            //Debug.Log("Fish On!!!! Press 'R' to reel it in!");


            StartMinigame(); //start  the minigame
        }
    }

    private void StartMinigame()
    {
        currentState = FishingState.Minigame;
        minigame.StartMinigame(OnMinigameComplete);
    }
    //forwarded from player after cast called, then starts the fishing sequence
    public void OnCast(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            StartFishing();
        }
    }

    //
    private void OnMinigameComplete(bool success)
    {
        if (success)
        {

            currentState = FishingState.Success;
            minigame.ShowAlert("You caught the fish!");
        }
        else
        {

            currentState = FishingState.Fail;
            minigame.ShowAlert(" The fish got away...");
        }

        StartCoroutine(FinishFishing());
    }


    //play reeling animation and tehn end the fishing sequence
    private IEnumerator FinishFishing()
    {
        animator.TryPlayAnimation("Reeling");
        yield return new WaitForSeconds(3f); // Reeling animation duration

        StopFishing();
    }
    //return to idle state after fishing sequence
    public void StopFishing()
    {
        if (biteCoroutine != null)
            StopCoroutine(biteCoroutine);

        currentState = FishingState.Idle;
        player.EnableMovement();
        animator.TryPlayAnimation("Stand");
    }
}
