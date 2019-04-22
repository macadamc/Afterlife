using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickInputMoveBehavior : MovementController2DBehavior
{
    public InputController ic;
    public float moveSpeed = 1f;

    public override void OnUpdate()
    {
        mc2d.AddForce(ic.joystick * moveSpeed);
    }
}
