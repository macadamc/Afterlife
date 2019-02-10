using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.StateMachine;

public class StrafeLock : State {

    public InputController ic;

    protected override void OnEnable()
    {
        base.OnEnable();
        ic.strafe = true;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        ic.strafe = false;
    }
}
