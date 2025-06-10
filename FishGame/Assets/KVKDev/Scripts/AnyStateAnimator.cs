using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
using Unity.VisualScripting;
using System.Linq;
using UnityEngine.UIElements;

public class AnyStateAnimator : MonoBehaviour
{
    private Animator animator;
    //tracks all animations
    private Dictionary<string, AnyStateAnimation> anyStateAnimations = new Dictionary<string, AnyStateAnimation>();

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Animate();

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
                if (anyStateAnimations[animName].IsPlaying == true)
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
            foreach (string animName in anyStateAnimations.Keys.ToList())
            {
                anyStateAnimations[animName].IsPlaying = false;
            }
            anyStateAnimations[animationName].IsPlaying = true;
        }
    }

    public void AddAnimation(params AnyStateAnimation[] animations)
    {
        for (int i = 0; i < animations.Length; i++)
        {
            this.anyStateAnimations.Add(animations[i].AnimationName, animations[i]);
        }
    }

    private Dictionary<string, bool> lastAnimationStates = new Dictionary<string, bool>();

    private void Animate()
    {
        foreach (var key in anyStateAnimations.Keys)
        {
            bool current = anyStateAnimations[key].IsPlaying;

            if (!lastAnimationStates.ContainsKey(key) || lastAnimationStates[key] != current)
            {
                animator.SetBool(key, current);
                lastAnimationStates[key] = current;
            }
        }
    }


    public void OnAnimationDone(string animationName)
    {
        anyStateAnimations[animationName].IsPlaying = false;
    }
    
    

   
}
