using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel;
using UnityEngine.Tilemaps;

public class SwapTile : TileRef 
{
    public TileBase tile;
    protected TileBase last;

    private void OnEnable()
    {
        Vector2 pos = new Vector2(transform.position.x, transform.position.y);

        last = GetTile(pos);
        SetTile(pos, tile);
    }

    private void OnDisable()
    {
        SetTile(new Vector2(transform.position.x, transform.position.y), last);
    }
}
