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
                if (startingController != null)
                    _inputController = startingController;
                else
                    _inputController = GetComponentInChildren<InputController>();
            }

            return _inputController;
        }
        set
        {
            _inputController = value;
        }
    }
    public ItemController Itc
    {
        get
        {
            if (_itemController == null)
                _itemController = GetComponent<ItemController>();

            return _itemController;
        }
    }

    public InputController startingController;

    InputController _inputController;
    ItemController _itemController;
    SpriteRenderer[] _spriteRenderers;

    private void Start()
    {
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        if (PauseManager.Instance.Paused)
            return;

        if (Itc != null)
        {
            foreach (SpriteRenderer s in _spriteRenderers)
            {
                if (Itc.lookDirection.x > 0)
                    s.flipX = false;

                if (Itc.lookDirection.x < 0)
                    s.flipX = true;
            }
        }
        else
        {
            // if item controller is null this gets called
            if (Inc.joystick.x > 0 || Inc.joystick.x < 0)
            {
                foreach (SpriteRenderer s in _spriteRenderers)
                {
                    if (Inc.joystick.x > 0)
                        s.flipX = false;

                    if (Inc.joystick.x < 0)
                        s.flipX = true;
                }
            }
        }

    }
}
