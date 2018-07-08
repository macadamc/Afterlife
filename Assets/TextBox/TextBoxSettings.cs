using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.Audio;
using Sirenix.OdinInspector;
using Pixelplacement.TweenSystem;

[CreateAssetMenu()]
public class TextBoxSettings : ScriptableObject
{
    public bool useDelay;
    [ShowIf("useDelay")]
    public float delay;

    public SoundEffect sfx;

    public Font font;
    public int fontSize;

    public enum InputType { Player, Passive }
    public InputType inputType;

    [ShowIf("IsPassive")]
    public float passiveLineDelay;
    bool IsPassive() { return inputType == InputType.Passive; }

    public bool autoSize;
    [ShowIf("autoSize"), Indent]
    public Vector2 maxSize;
    [ShowIf("autoSize"), Indent]
    public Vector2 padding;

    public bool useTween = true;
}
