using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Pixelplacement;

[CreateAssetMenu(menuName ="ShadyPixel/New Transition")]
public class Transition : ScriptableObject
{
    public enum Type
    {Simple, Texture }

    public Type type;

    [ShowIf("type", Type.Texture)]
    public Texture texture;

    public Color fadeColor;

    public float startDelay = 0.25f;
    public float transitionTime = 1.0f;
    public AnimationCurve animationCurve = Tween.EaseInOut;


    public void FadeIn()
    {
        TransitionManager.Instance.SetColor(fadeColor);

        if (type == Type.Simple)
        {
            TransitionManager.Instance.SimpleFade(1.0f, 0.0f, transitionTime, startDelay, animationCurve);
        }
        else
        {
            TransitionManager.Instance.SetTexture(texture);
            TransitionManager.Instance.TextureFade(1.0f, 0.0f, transitionTime, startDelay, animationCurve);
        }
    }

    public void FadeOut()
    {
        TransitionManager.Instance.SetColor(fadeColor);

        if (type == Type.Simple)
        {
            TransitionManager.Instance.SimpleFade(0.0f, 1.0f, transitionTime, startDelay, animationCurve);
        }
        else
        {
            TransitionManager.Instance.SetTexture(texture);
            TransitionManager.Instance.TextureFade(0.0f, 1.0f, transitionTime, startDelay, animationCurve);
        }
    }

}
