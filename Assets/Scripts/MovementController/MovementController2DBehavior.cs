using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementController2DBehavior : MonoBehaviour
{
    public MovementController2D mc2d;

    void OnEnable()
    {
        mc2d.onUpdate += OnUpdate;
    }

    void OnDisable()
    {
        mc2d.onUpdate -= OnUpdate;
    }

    public abstract void OnUpdate();
}
