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

}
