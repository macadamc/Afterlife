using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.Singleton;
using Pixelplacement;
using UnityEngine.Tilemaps;
using ShadyPixel.Audio;
using UnityEditor;



public class AudioManager : Singleton<AudioManager>
{
    public AudioSource source_BGM;
    public TilemapSoundEffects tilemapSFX;

    Tilemap _tilemap;
    AudioClip queuedClip;
    bool queuedLoop;


    private void OnEnable()
    {
        Initialize(this);
    }

    public void PlaySFX(SoundEffect sfx)
    {
        if (sfx.clip == null)
            return;

        AudioSource source = new GameObject().AddComponent<AudioSource>();
        source.clip = sfx.clip;
        source.volume = Random.Range(sfx.volume.x, sfx.volume.y);
        source.pitch = Random.Range(sfx.pitch.x, sfx.pitch.y);
        source.outputAudioMixerGroup = sfx.mixerGroup;

        source.Play();
        StartCoroutine(DestroyOnFinish(source.gameObject, source.clip.length + 0.1f));
    }

    public IEnumerator DestroyOnFinish(GameObject gameObject, float delay)
    {
        yield return new WaitForSeconds(delay);

        Debug.Log("cleanup");

        if(Application.isEditor)
            DestroyImmediate(gameObject);
        else
            Destroy(gameObject);
        
    }

    #region BGM

    public void SetBGMVolume(float value)
    {
        source_BGM.volume = value;
    }

    public void PlayBGM(AudioClip clip, bool loop)
    {
        source_BGM.clip = clip;
        source_BGM.loop = loop;
        source_BGM.Play();
        Tween.Value(0.0f, 1.0f, SetBGMVolume, 1.0f, 0.0f, Tween.EaseIn);
    }

    public void PlayBGM(AudioClip clip)
    {
        source_BGM.clip = clip;
        source_BGM.loop = false;
        source_BGM.Play();
        Tween.Value(0.0f, 1.0f, SetBGMVolume, 1.0f, 0.0f, Tween.EaseIn);
    }

    public void FadeToBGM(AudioClip clip, bool loop)
    {
        queuedClip = clip;
        queuedLoop = loop;
        Tween.Value(source_BGM.volume, 0.0f, SetBGMVolume, 1.0f, 0.0f, Tween.EaseIn, Tween.LoopType.None, null, OnFadeOut, true);
    }

    public void FadeToBGM(AudioClip clip)
    {
        queuedClip = clip;
        queuedLoop = false;
        Tween.Value(source_BGM.volume, 0.0f, SetBGMVolume, 1.0f, 0.0f, Tween.EaseIn, Tween.LoopType.None, null, OnFadeOut, true);
    }

    private void OnFadeOut()
    {
        PlayBGM(queuedClip, queuedLoop);
        queuedClip = null;
        queuedLoop = false;
    }

    #endregion
}
