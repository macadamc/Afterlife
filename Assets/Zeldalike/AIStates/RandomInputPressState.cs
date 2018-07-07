using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.StateMachine;
using Sirenix.OdinInspector;

public class RandomInputPressState : State
{
    public Vector2 holdTime;
    public InputController inputController;
    public GameObject forceState;

    float _endStateTime;
    bool _input;


    protected override void OnEnable()
    {
        base.OnEnable();
        inputController.input.pressed = false;
        inputController.input.held = false;
        inputController.input.released = false;

    }

    protected override void OnDisable()
    {
        _input = false;
        inputController.input.pressed = false;
        inputController.input.held = false;
        inputController.input.released = false;
        base.OnDisable();
    }

    private void Update()
    {
        if(!_input)
        {
            inputController.input.pressed = true;
            _input = true;
            SetReleaseTime();
        }
        else
        {
            inputController.input.pressed = false;
            inputController.input.held = true;

            if(Time.time > _endStateTime)
            {
                inputController.input.held = false;
                inputController.input.released = true;
                if (forceState != null)
                    ChangeState(forceState);
                else
                    Next();
            }
        }
    }

    public void SetReleaseTime()
    {
        _endStateTime = Time.time + Random.Range(holdTime.x,holdTime.y);
    }
}
