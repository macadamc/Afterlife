using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.StateMachine;
using Sirenix.OdinInspector;

public class RandomMoveState : State {

    [MinMaxSlider(0f, 10f, true)]
    public Vector2 moveTime;

    [MinMaxSlider(0f, 10f, true)]
    public Vector2 delayTime;

    public float stateTime = 3f;

    public InputController inputController;
    public GameObject forceNextState;

    private float _nextMoveTime;
    private bool _delayed;
    private bool _joystickSet;
    private bool _delayedSet;
    private float _endState;

    protected override void OnEnable()
    {
        base.OnEnable();
        Init();
    }

    protected override void OnDisable()
    {
        Init();
        base.OnDisable();
    }

    private void Init()
    {
        if (inputController == null)
            inputController = GetComponentInChildren<InputController>();

        inputController.joystick = Vector2.zero;
        inputController.input.SetValue(false);
        _endState = Time.time + stateTime;
    }

    private void Start()
    {
        if (Random.Range(0f, 1f) > 0.5f)
        {
            _delayed = true;
        }
        else
        {
            _delayed = false;
        }

    }

    private void Update()
    {
        if (_delayed)
        {
            // if delay input has not been set
            if (!_delayedSet)
            {
                _delayedSet = true;
                _joystickSet = false;
                inputController.joystick = Vector2.zero;
                UpdateMoveTime(delayTime.x, delayTime.y);
            }
        }
        else
        {
            // if movement input has not been set yet
            if (!_joystickSet)
            {
                inputController.joystick = Random.insideUnitCircle;
                _joystickSet = true;
                _delayedSet = false;
                UpdateMoveTime(moveTime.x, moveTime.y);
            }
        }

        // if it is time for the delayed bool to flip
        if (Time.time >= _nextMoveTime)
        {
            _delayed = !_delayed;


            if (Time.time >= _endState)
            {
                if (forceNextState != null)
                    ChangeState(forceNextState);
                else
                    Next();
            }
        }

    }

    private void UpdateMoveTime(float min, float max)
    {
        _nextMoveTime = Time.time + Random.Range(min, max);
    }

    private void UpdateMoveTime(float time)
    {
        _nextMoveTime = Time.time + time;
    }

}
