using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.StateMachine;

public class StrafeLock : State {

    public InputController ic;
    private void OnEnable()
    {
        ic.strafe = true;
    }

    private void OnDisable()
    {
        ic.strafe = false;
    }
}
