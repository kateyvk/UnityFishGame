using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnyStateAnimation
{
    public string AnimationName { get; set; }
    public bool IsPlaying { get; set; }
    public string [] HigherPrio { get; set; }
    public AnyStateAnimation(string animationName, params string[] higherPrio){
        this.AnimationName = animationName;
        this.HigherPrio = higherPrio;
    }
}
