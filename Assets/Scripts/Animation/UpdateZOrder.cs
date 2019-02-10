using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
[RequireComponent(typeof(SortingGroup))]
public class UpdateZOrder : MonoBehaviour
{
    public SortingGroup SortingGroup
    {
        get
        {
            if (_sortingGroup == null)
                _sortingGroup = GetComponent<SortingGroup>();

            return _sortingGroup;
        }
    }
    
    public int ppu = 16;
    Vector2 _lastKnownPosition;
    SpriteRenderer[] _spriteRenderer;
    SortingGroup _sortingGroup;
    //bool _inEditor;

    private void Start()
    {
        if(!Application.isPlaying)
        {
            //_inEditor = true;

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
        int n = -Mathf.RoundToInt(transform.position.y * ppu);

        if(SortingGroup != null)
            SortingGroup.sortingOrder = n;
    }
}
