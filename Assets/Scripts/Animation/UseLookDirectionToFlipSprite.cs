using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseLookDirectionToFlipSprite : MonoBehaviour
{
    public Transform FlipTransform
    {
        get
        {
            if (transformToUse != null)
                return transformToUse;
            else
                return transform;
        }
    }


    public InputController inputController;
    public Transform transformToUse;
    SpriteRenderer[] _spriteRenderers;

    private void Start()
    {
        _spriteRenderers = FlipTransform.GetComponentsInChildren<SpriteRenderer>();
        if (inputController == null)
            inputController = GetComponentInChildren<InputController>();
    }

    private void Update()
    {
        if (PauseManager.Instance.Paused)
            return;

        foreach (SpriteRenderer s in _spriteRenderers)
        {
            if (inputController.lookDirection.x > 0)
                s.flipX = false;

            if (inputController.lookDirection.x < 0)
                s.flipX = true;
        }
    }
}
