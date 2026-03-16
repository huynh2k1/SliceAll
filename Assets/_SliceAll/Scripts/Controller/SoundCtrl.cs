using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum TypeSFX
{
    CLICK,
    WIN,
    LOSE,
    DEAD,
    HITGROUND,
    DRAGBOW,
    SHOOT,
    EXPLOSION
}

public class SoundCtrl : MonoBehaviour
{
    public static SoundCtrl I;

    [Header("MUSIC")]
    [SerializeField] AudioSource _musicSource;

    [Header("SOUNDS")]
    [SerializeField] AudioSource[] _soundSources;
    private Queue<AudioSource> _queueSounds;

    [Header("AUDIO CLIPS")]
    [SerializeField] AudioClip _bgMusic;
    [SerializeField]
    AudioClip _click, _win, _lose, _dead, _hitGround, _dragBow, _shoot, _explosion;
    private void Awake()
    {
        I = this;
        _queueSounds = new Queue<AudioSource>(_soundSources);
    }

    public void OnVolumeMusicChange()
    {
        _musicSource.volume = DataPrefs.Music;
    }

    public void PlayMusic()
    {
        _musicSource.volume = DataPrefs.Music;
        _musicSource.clip = _bgMusic;
        _musicSource.Play();
    }

    public void PlaySFXByType(TypeSFX type)
    {
        switch (type)
        {
            case TypeSFX.CLICK:
                PlaySound(_click);
                break;
            case TypeSFX.WIN:
                PlaySound(_win);
                break;
            case TypeSFX.LOSE:
                PlaySound(_lose);
                break;
            case TypeSFX.DEAD:
                PlaySound(_dead);
                break;
            case TypeSFX.HITGROUND:
                PlaySound(_hitGround);
                break;
            case TypeSFX.DRAGBOW:
                PlaySound(_dragBow);
                break;
            case TypeSFX.SHOOT:
                PlaySound(_shoot);
                break;
            case TypeSFX.EXPLOSION:
                PlaySound(_explosion);
                break;
        }
    }

    public void PlaySound(AudioClip clip)
    {
        if (_queueSounds.Count == 0) return;

        AudioSource source = _queueSounds.Dequeue();
        source.volume = DataPrefs.Sound;
        source.PlayOneShot(clip);
        StartCoroutine(ReturnToQueueWhenFinished(source));
    }

    private System.Collections.IEnumerator ReturnToQueueWhenFinished(AudioSource source)
    {
        yield return new WaitUntil(() => !source.isPlaying);
        _queueSounds.Enqueue(source);
    }

    public void StopMusic()
    {
        _musicSource.Stop();
    }
}

