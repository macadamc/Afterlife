using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.StateMachine;


public class ChangeState : State {

    public GameObject forceState;

    public void Change()
    {
        if (forceState == null)
            Next();
        else
            ChangeState(forceState);
    }
}
