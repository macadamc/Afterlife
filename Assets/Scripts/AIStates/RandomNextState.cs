using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.StateMachine;


public class RandomNextState : State
    {
    List<GameObject> states;
    protected override void OnEnable()
    {
        StateMachine.ChangeState(states[Random.Range(0, states.Count)]);
    }
}
