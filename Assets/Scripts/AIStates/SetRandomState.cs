using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.StateMachine;

public class SetRandomState : State
{
    public List<GameObject> states = new List<GameObject>();

    public bool activateOnEnable = true;

    protected override void OnEnable()
    {
        base.OnEnable();

        if (activateOnEnable)
            SetState();

    }

    public void SetState()
    {
        StateMachine.SetState(states[Random.Range(0, states.Count)]);
    }

}
