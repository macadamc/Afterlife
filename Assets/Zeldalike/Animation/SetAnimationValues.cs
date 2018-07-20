using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAnimationValues : MonoBehaviour
{
    public Animator Animator
    {
        get
        {
            if (_animator == null)
                _animator = GetComponentInChildren<Animator>();

            return _animator;
        }
    }

    Animator _animator;

    public MovementController Mc
    {
        get
        {
            if (_movementController == null)
                _movementController = GetComponentInChildren<MovementController>();

            return _movementController;
        }
    }

    MovementController _movementController;

    private void Update()
    {
        if (PauseManager.Instance != null && PauseManager.Instance.Paused)
            return;

        Animator.SetBool("isMoving", Mc.IsMoving);
    }
}
