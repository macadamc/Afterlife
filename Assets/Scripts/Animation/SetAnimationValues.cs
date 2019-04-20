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

    public Health Health
    {
        get
        {
            if (_health == null)
                _health = GetComponentInChildren<Health>();

            return _health;
        }
    }

    Health _health;

    private void OnEnable()
    {
        if(Health!=null)
            Health.onHealthChanged += OnHealthChange;
    }

    private void OnDisable()
    {
        if (Health != null)
            Health.onHealthChanged -= OnHealthChange;
    }

    public void OnHealthChange(int change)
    {
        if(change < 0)
        {
            Animator.SetTrigger("hurt");
        }

    }

    private void Update()
    {
        if (PauseManager.Instance != null && PauseManager.Instance.Paused)
            return;

        Animator.SetBool("isMoving", Mc.IsMoving);

        if(Health != null)
        {

            if (Health.currentHealth.Value <= 0)
                Animator.SetTrigger("death");
        }
    }

    private void LateUpdate()
    {
        Animator.ResetTrigger("hurt");
    }
}
