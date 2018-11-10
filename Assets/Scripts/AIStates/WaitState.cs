using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using ShadyPixel.StateMachine;

public class WaitState : State
{
    GameObject ForcedState;
    [MinMaxSlider(0, 30, true)]
    public Vector2 WaitTime;
    protected float nextTime;


    protected override void OnEnable()
    {
        base.OnEnable();
        nextTime = Time.time + Random.Range(WaitTime.x, WaitTime.y);
    }

    private void Update()
    {
        if (Time.time >= nextTime)
        {
            if (ForcedState)
                ChangeState(ForcedState);
            else
                Next();
        }
    }
}
