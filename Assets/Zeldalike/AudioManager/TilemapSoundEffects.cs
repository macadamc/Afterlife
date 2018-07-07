using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Tilemaps;
using ShadyPixel.Audio;

[CreateAssetMenu(menuName ="ShadyPixel/New Tilemap Sound Effects")]
public class TilemapSoundEffects : SerializedScriptableObject
{
    public Dictionary<TileBase, SoundEffect> soundEffects;
}
