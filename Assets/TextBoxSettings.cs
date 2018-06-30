using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.Audio;

[CreateAssetMenu()]
public class TextBoxSettings : ScriptableObject {

	public float delay;
    public float lineDelay;
    public SoundEffect sfx;
    public Font font;
    public int fontSize;
}
