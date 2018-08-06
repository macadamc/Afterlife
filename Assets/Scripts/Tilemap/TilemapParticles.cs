using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Tilemaps;
using ShadyPixel.Audio;

[CreateAssetMenu(menuName ="ShadyPixel/New Tilemap Particle Dictionary")]
public class TilemapParticles : SerializedScriptableObject
{
    public Dictionary<TileBase, GameObject> particles = new Dictionary<TileBase, GameObject>();
}
