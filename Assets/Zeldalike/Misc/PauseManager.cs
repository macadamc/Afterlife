using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.Singleton;
using Pixelplacement;

public class PauseManager : Singleton<PauseManager>
{
    public CanvasGroup pauseScreenCanvasGroup;
    bool _gamePaused;
    bool _playerPaused;

    bool _tween;

    public bool Paused
    {
        get
        {
            return (_gamePaused || _playerPaused);
        }
        set
        {
            _gamePaused = value;

            if(!PlayerPaused)
            {
                if (value == true)
                    Time.timeScale = 0.0f;
                else
                    Time.timeScale = 1.0f;
            }

        }

    }

    public bool PlayerPaused
    {
        get
        {
            return _playerPaused;
        }
        set
        {
            _playerPaused = value;

            StopAllCoroutines();
            if(value == true)
            {
                Time.timeScale = 0.0f;
                Tween.CanvasGroupAlpha(pauseScreenCanvasGroup, 0.0f, 1.0f, 0.25f, 0.0f, Tween.EaseInOut, Tween.LoopType.None, null, null, false);
            }
            else
            {
                Time.timeScale = 1.0f;
                Tween.CanvasGroupAlpha(pauseScreenCanvasGroup, 1.0f, 0.0f, 0.25f, 0.0f, Tween.EaseInOut, Tween.LoopType.None, null, null, false);
            }
        }
    }


    public void ChangeTimeScale(float newTimeScale)
    {
        Time.timeScale = newTimeScale;
    }

    private void OnEnable()
    {
        Initialize(this);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !_gamePaused)
        {
            PlayerPaused = !PlayerPaused;
        }
    }
}
