using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.Singleton;
using Pixelplacement;
using UnityEngine.Tilemaps;

public class AudioManager : Singleton<AudioManager>
{

    public AudioSource source_BGM;
    public AudioClip debugClip;
    public TilemapSoundEffects tilemapSFX;

    Tilemap _tilemap;
    AudioClip queuedClip;
    bool queuedLoop;

    public void SetVolume(float value)
    {
        source_BGM.volume = value;
    }

    public void PlayBGM(AudioClip clip, bool loop)
    {
        source_BGM.clip = clip;
        source_BGM.loop = loop;
        source_BGM.Play();
        Tween.Value(0.0f, 1.0f, SetVolume, 1.0f, 0.0f, Tween.EaseIn);
    }

    public void FadeToBGM(AudioClip clip, bool loop)
    {
        queuedClip = clip;
        queuedLoop = loop;
        Tween.Value(source_BGM.volume, 0.0f, SetVolume, 1.0f, 0.0f, Tween.EaseIn, Tween.LoopType.None, null, OnFadeOut, true);
    }

    private void OnEnable()
    {
        Initialize(this);
    }

    private void Start()
    {
        PlayBGM(debugClip, true);
    }

    private void OnFadeOut()
    {
        PlayBGM(queuedClip, queuedLoop);
        queuedClip = null;
        queuedLoop = false;
    }
}
