using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "ShadyPixel/Tiles/New Tile List")]
public class TileList : ScriptableObject
{
    public List<TileBase> tiles;
}
