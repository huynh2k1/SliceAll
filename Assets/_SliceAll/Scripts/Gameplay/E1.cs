using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1 : BaseEnemy
{
    [SerializeField] AnimationClip[] clips;

    void OnEnable()
    {
        PlayRandomClip();
    }

    void PlayRandomClip()
    {
        if (clips.Length == 0) return;

        AnimationClip clip = clips[Random.Range(0, clips.Length)];
        _animator.Play(clip.name);
    }
}