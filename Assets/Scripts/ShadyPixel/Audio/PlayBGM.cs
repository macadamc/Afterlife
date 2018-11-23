using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBGM : MonoBehaviour
{
    public bool playOnStart = true;
    public AudioClip clip;
    public bool loopWhenDone;


    AudioManager Manager
    {
        get
        {
            if (_audioManager == null)
                _audioManager = FindObjectOfType<AudioManager>();

            return _audioManager;
        }
    }
    AudioManager _audioManager;

    private void Start()
    {
        if (playOnStart)
            Play();
    }

    public void Play()
    {
        if (Manager == null)
        {
            Debug.LogWarning("There is no Audio Manager that could be found.");
            return;
        }

        if (Manager.source_BGM.isPlaying)
        {
            Manager.FadeToBGM(clip, loopWhenDone);
        }
        else
        {
            Manager.PlayBGM(clip, loopWhenDone);
        }
    }
}
