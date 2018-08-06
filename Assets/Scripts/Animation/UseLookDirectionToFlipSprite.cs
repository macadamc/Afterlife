using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseLookDirectionToFlipSprite : MonoBehaviour
{
	public InputController Inc
    {
        get
        {
            if (_inputController == null)
            {
                _inputController = GetComponentInChildren<InputController>();
            }

            return _inputController;
        }
        set
        {
            _inputController = value;
        }
    }


    InputController _inputController;
    SpriteRenderer[] _spriteRenderers;

    private void Start()
    {
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        if (PauseManager.Instance.Paused)
            return;


        if (Inc.lookDirection.x > 0 || Inc.lookDirection.x < 0)
        {
            foreach (SpriteRenderer s in _spriteRenderers)
            {
                if (Inc.lookDirection.x > 0)
                    s.flipX = false;

                if (Inc.lookDirection.x < 0)
                    s.flipX = true;
            }
        }

    }
}
