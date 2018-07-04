using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.Singleton;
using Pixelplacement;
using UnityEngine.SceneManagement;

public class TransitionManager : Singleton<TransitionManager>
{
    public delegate void OnTransitionEnd();
    public OnTransitionEnd onTransitionEnd;
    public Transition currentTransition;

    public Material transitionMaterial;

    public bool Transitioning
    {
        get
        {
            return _transitioning;
        }
    }

    bool _transitioning;


    public void FadeIn(Transition transition)
    {
        SetTransition(transition);
        SetColor(currentTransition.fadeColor);

        if (currentTransition.type == Transition.Type.Simple)
        {
            SimpleFade(1.0f, 0.0f, currentTransition.transitionTime, currentTransition.startDelay, currentTransition.animationCurve);
        }
        else
        {
            SetTexture(currentTransition.texture);
            TextureFade(1.0f, 0.0f, currentTransition.transitionTime, currentTransition.startDelay, currentTransition.animationCurve);
        }
    }

    public void FadeOut(Transition transition)
    {
        SetTransition(transition);
        SetColor(currentTransition.fadeColor);

        if (currentTransition.type == Transition.Type.Simple)
        {
            SimpleFade(0.0f, 1.0f, currentTransition.transitionTime, currentTransition.startDelay, currentTransition.animationCurve);
        }
        else
        {
            SetTexture(currentTransition.texture);
            TextureFade(0.0f, 1.0f, currentTransition.transitionTime, currentTransition.startDelay, currentTransition.animationCurve);
        }
    }

    public void SetTransition(Transition transition)
    {
        currentTransition = transition;
    }

    private void SimpleFade(float startValue, float endValue, float duration, float delay, AnimationCurve curve)
    {
        //PauseManager.Instance.Paused = true;
        transitionMaterial.SetFloat("_Cutoff", 1.0f);
        Tween.ShaderFloat(transitionMaterial, "_Fade", startValue, endValue, duration, delay, curve, Tween.LoopType.None, null, EndTransition, false);
        _transitioning = true;
    }

    private void TextureFade(float startValue, float endValue, float duration, float delay, AnimationCurve curve)
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
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        FadeIn(currentTransition);
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        FadeIn(currentTransition);
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
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

    private void SetColor(Color color)
    {
        transitionMaterial.SetColor("_Color", color);
    }

    private void SetTexture(Texture texture)
    {
        transitionMaterial.SetTexture("_TransitionTex", texture);
    }
}
