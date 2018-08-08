using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ShadyPixel/AmbientLightSettings")]
public class AmbientLightSettings : ScriptableObject {

    public Color lightColor;
    [Range(0f,2f)]
    public float lightIntensity = 1f;
}
