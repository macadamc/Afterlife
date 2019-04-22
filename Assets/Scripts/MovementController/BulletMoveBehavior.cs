using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMoveBehavior : MovementController2DBehavior
{
    public Vector2 velocity;

    public override void OnUpdate()
    {
        mc2d.AddForce(velocity);
    }
}
