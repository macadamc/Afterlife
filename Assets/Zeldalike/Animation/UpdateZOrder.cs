using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class UpdateZOrder : MonoBehaviour
{
    public SpriteRenderer[] Sr
    {
        get
        {
            if (_spriteRenderer == null || _spriteRenderer.Length == 0)
                _spriteRenderer = GetComponentsInChildren<SpriteRenderer>();

            return _spriteRenderer;
        }
        set
        {
            _spriteRenderer = value;
        }
    }
    public int ppu = 16;
    public int offset;
    Vector2 _lastKnownPosition;
    SpriteRenderer[] _spriteRenderer;
    bool _inEditor;

    private void Start()
    {
        if(!Application.isPlaying)
        {
            _inEditor = true;

            if (_lastKnownPosition != (Vector2)transform.position)
            {
                UpdateZ();
            }

        }
    }

    private void LateUpdate()
    {
        if (_lastKnownPosition != (Vector2)transform.position)
        {
            UpdateZ();
        }
    }

    private void UpdateZ()
    {
        _lastKnownPosition = transform.position;

        foreach(SpriteRenderer s in Sr)
        {
            s.sortingOrder = -Mathf.RoundToInt(transform.position.y * ppu) + offset;
        }
    }
}
