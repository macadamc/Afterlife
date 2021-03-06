﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.StateMachine;


public class RandomNextState : State
    {
    public List<GameObject> states;
    protected override void OnEnable()
    {
        if (states == null)
            states = new List<GameObject>();

        StateMachine.ChangeState(states[Random.Range(0, states.Count)]);
    }
}
