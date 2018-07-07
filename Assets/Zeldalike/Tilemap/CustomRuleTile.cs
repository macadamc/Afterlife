using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using ShadyPixel.Audio;
using System;

[CreateAssetMenu]
public class CustomRuleTile : RuleTile<CustomRuleTile.Neighbor> {

    public List<TileBase> sibings = new List<TileBase>();
    public class Neighbor : RuleTile.TilingRule.Neighbor
    {
        public const int Sibing = 3;
    }
    public override bool RuleMatch(int neighbor, TileBase tile)
    {
        switch (neighbor)
        {
            case Neighbor.Sibing: return sibings.Contains(tile);
        }
        return base.RuleMatch(neighbor, tile);
    }

#if UNITY_EDITOR
    public override void RuleOnGUI(Rect rect, Vector2Int pos, int neighbor)
{
    base.RuleOnGUI(rect, pos, neighbor);
}


#endif
}
