using System.Collections;
using System.Collections.Generic;
using FGUFW;
using UnityEngine;

public class AnimatorTest : MonoBehaviour
{
    public AnimationClip[] Clips;
    private int _clipIndex;
    public Animator Animator;

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        _clipIndex++;
        var clip = Clips[_clipIndex%Clips.Length];
        this.Animator.ReplaceClip(clip);
        enabled = false;
    }

}
