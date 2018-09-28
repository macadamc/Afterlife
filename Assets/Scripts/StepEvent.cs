using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class StepEvent : MonoBehaviour
{
    public UnityEvent onStep;
    /*
    public TileRef TileReference
    {
        get
        {
            if (_tileRef == null)
                _tileRef = GetComponentInParent<TileRef>();

            return _tileRef;
        }
    }
    protected TileRef _tileRef;
    protected TileBase _tile;
    */

    public void Step()
    {
        /*
        _tile = TileReference.GetTile(transform.position);

        if (_tile != null)
        {
            if (AudioManager.Instance.tilemapSFX.soundEffects.ContainsKey(_tile))
            {
                AudioSource.PlayClipAtPoint(AudioManager.Instance.tilemapSFX.soundEffects[_tile].clip, transform.position);
            }
        }
        */
        onStep?.Invoke();
    }
}
