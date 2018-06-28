using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.Singleton;

public class PauseManager : Singleton<PauseManager>
{
    private void OnEnable()
    {
        Initialize(this);
    }

    public bool Paused
    {
        get
        {
            return _gamePaused;
        }
        set
        {
            _gamePaused = value;

            if (_gamePaused)
                Time.timeScale = 0.0f;
            else
                Time.timeScale = 1.0f;
        }

    }

    bool _gamePaused;
}
