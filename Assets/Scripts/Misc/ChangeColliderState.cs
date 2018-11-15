using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColliderState : MonoBehaviour {


    public Transform T
    {
        get
        {
            if (transformToUse == null)
                return transform;
            else
                return transformToUse;
        }
    }

    public Transform transformToUse;
    public enum ColliderState { On, Off};
    public ColliderState colliderState;
    public bool changeStateOnEnable = true;

    Collider2D[] _colliders;

    private void OnEnable()
    {
        if (changeStateOnEnable)
            ChangeState();
    }


    private void ChangeState()
    {
        _colliders = T.GetComponentsInChildren<Collider2D>();
        if(colliderState == ColliderState.On)
        {
            foreach (Collider2D c in _colliders)
            {
                c.enabled = true;
            }
        }
        else
        {
            foreach (Collider2D c in _colliders)
            {
                c.enabled = false;
            }
        }
    }
    private void ChangeState(bool state)
    {
        _colliders = T.GetComponentsInChildren<Collider2D>();
        if (state == true)
        {
            foreach (Collider2D c in _colliders)
            {
                c.enabled = true;
            }
        }
        else
        {
            foreach (Collider2D c in _colliders)
            {
                c.enabled = false;
            }
        }
    }
}
