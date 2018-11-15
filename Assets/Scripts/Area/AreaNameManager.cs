using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ShadyPixel.Singleton;
using Pixelplacement;
using Sirenix.OdinInspector;

public class AreaNameManager : Singleton<AreaNameManager> {

    [Required]
    public Text textComponent;
    [Required]
    public CanvasGroup textCanvasGroup;

    public float duration = 2f;
    public float delay = 0.25f;

    private void OnEnable()
    {
        Initialize(this);
    }

    public void SetAreaText(string newAreaName)
    {
        StopAllCoroutines();
        textComponent.text = "";
        Tween.CanvasGroupAlpha(textCanvasGroup, 0f, 1f, duration, delay, Tween.EaseLinear, Tween.LoopType.None, null, FadeOut, false);
        textComponent.text = newAreaName;
    }

    private void FadeOut()
    {
        Tween.CanvasGroupAlpha(textCanvasGroup, 1f, 0f, duration, delay+1f, Tween.EaseInOut, Tween.LoopType.None, null, null, false);
    }
}
