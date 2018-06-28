using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.Singleton;
using Pixelplacement;

public class TransitionManager : Singleton<TransitionManager>
{
    public delegate void OnTransitionEnd();
    public OnTransitionEnd onTransitionEnd;
    public Transition startTransition;

    public Material transitionMaterial;

    public bool Transitioning
    {
        get
        {
            return _transitioning;
        }
    }

    bool _transitioning;

    public void SimpleFade(float startValue, float endValue, float duration, float delay, AnimationCurve curve)
    {
        //PauseManager.Instance.Paused = true;
        transitionMaterial.SetFloat("_Cutoff", 1.0f);
        Tween.ShaderFloat(transitionMaterial, "_Fade", startValue, endValue, duration, delay, curve, Tween.LoopType.None, null, EndTransition, false);
        _transitioning = true;
    }

    public void TextureFade(float startValue, float endValue, float duration, float delay, AnimationCurve curve)
    {
        transitionMaterial.SetFloat("_Fade", 1.0f);
        Tween.ShaderFloat(transitionMaterial, "_Cutoff", startValue, endValue, duration, delay, curve, Tween.LoopType.None, null, EndTransition, false);
        _transitioning = true;

    }

    private void OnEnable()
    {
        Initialize(this);
    }

    private void Start()
    {
        startTransition.FadeIn();
    }

    private void OnDisable()
    {
        transitionMaterial.SetFloat("_Cutoff", 0.0f);
        transitionMaterial.SetFloat("_Fade", 0.0f);
    }

    private void EndTransition()
    {
        //PauseManager.Instance.Paused = false;
        _transitioning = false;
        if (onTransitionEnd != null)
            onTransitionEnd.Invoke();
    }

    public void SetColor(Color color)
    {
        transitionMaterial.SetColor("_Color", color);
    }

    public void SetTexture(Texture texture)
    {
        transitionMaterial.SetTexture("_TransitionTex", texture);
    }
}
