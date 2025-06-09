using UnityEngine;
using System.Collections.Generic;

public class AnyStateAnimator : MonoBehaviour
{
    private Animator animator;
    private Dictionary<string, AnyStateAnimation> anyStateAnimations = new Dictionary<string, AnyStateAnimation>();

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Animate();
        HandleCastingInput();
    }
    public bool IsPlaying(string animationName)
    {
        return anyStateAnimations.ContainsKey(animationName) && anyStateAnimations[animationName].IsPlaying;
    }


    public void TryPlayAnimation(string animationName)
    {
        bool startAnimation = true;
        if (anyStateAnimations[animationName].HigherPrio == null)
        {
            StartAnimation();
        }
        else
        {
            foreach (string animName in anyStateAnimations[animationName].HigherPrio)
            {
                if (anyStateAnimations[animName].IsPlaying)
                {
                    startAnimation = false;
                    break;
                }
            }
            if (startAnimation)
            {
                StartAnimation();
            }
        }

        void StartAnimation()
        {
            foreach (string animName in anyStateAnimations.Keys)
            {
                anyStateAnimations[animName].IsPlaying = false;
            }
            anyStateAnimations[animationName].IsPlaying = true;
        }
    }

    public void AddAnimation(params AnyStateAnimation[] animations)
    {
        foreach (var anim in animations)
        {
            anyStateAnimations.Add(anim.AnimationName, anim);
        }
    }

    private void Animate()
    {
        foreach (var key in anyStateAnimations.Keys)
        {
            animator.SetBool(key, anyStateAnimations[key].IsPlaying);
        }
    }

    public void OnAnimationDone(string animationName)
    {
        anyStateAnimations[animationName].IsPlaying = false;
    }

    private void HandleCastingInput()
    {
        if (Input.GetKeyDown(KeyCode.C)) // Replace with your desired key
        {
            TryPlayAnimation("Casting");
        }
        else if (Input.GetKeyUp(KeyCode.C)) // Replace with your desired key
        {
            TryPlayAnimation("CastIdle");
        }
    }
}
