using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.Singleton;
using Pixelplacement;
using UnityEngine.EventSystems;

public class PauseManager : Singleton<PauseManager>
{
    bool _gamePaused;
    bool _tween;

    public bool Paused
    {
        get
        {
            return _gamePaused;
        }
        set
        {
            _gamePaused = value;

            if (value == true)
                Time.timeScale = 0.0f;
            else
                Time.timeScale = 1.0f;

        }

    }

    public void ChangeTimeScale(float newTimeScale)
    {
        Time.timeScale = newTimeScale;
    }

    public IEnumerator LerpTimeScaleOverTime(float startScale, float endScale, float time)
    {
        float elapsedTime = 0f;
        float currentScale = startScale;

        ChangeTimeScale(startScale);

        while (elapsedTime < time)
        {
            currentScale = Mathf.Lerp(startScale, endScale, (elapsedTime / time));
            elapsedTime += Time.unscaledDeltaTime;
            ChangeTimeScale(currentScale);
            yield return null;
        }

        ChangeTimeScale(endScale);
    }

    private void OnEnable()
    {
        Initialize(this);
    }

    public IEnumerator FreezeFrame(float scale, float time)
    {
        ChangeTimeScale(scale);
        yield return new WaitForSecondsRealtime(time);
        ChangeTimeScale(1f);
    }

}
