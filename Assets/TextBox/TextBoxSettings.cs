using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.Audio;
using Sirenix.OdinInspector;

[CreateAssetMenu()]
public class TextBoxSettings : ScriptableObject
{

    public bool useDelay;
    [ShowIf("useDelay")]
    public float delay;
    public float lineDelay;
    public SoundEffect sfx;
    public Font font;
    public int fontSize;
}
