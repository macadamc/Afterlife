using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileInputController : InputController
{
    private void Update()
    {
        joystick = transform.right;
    }
}
