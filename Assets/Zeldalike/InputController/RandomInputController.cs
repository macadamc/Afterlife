using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class RandomInputController : InputController
{
    [MinMaxSlider(0f, 10f, true)]
    public Vector2 moveTime;

    [MinMaxSlider(0f, 10f, true)]
    public Vector2 delayTime;

    private float _nextMoveTime;
    private bool _delayed;
    private bool _joystickSet;
    private bool _delayedSet;

    // Use this for initialization
    void Start ()
    {
        if(Random.Range(0f,1f) > 0.5f)
        {
            _delayed = true;
        }
        else
        {
            _delayed = false;
        }

	}
	
	void Update ()
    {

        OnUpdateMoveInput();

	}

    // has move input logic
    private void OnUpdateMoveInput()
    {
        if (_delayed)
        {
            // if delay input has not been set
            if (!_delayedSet)
            {
                _delayedSet = true;
                _joystickSet = false;
                joystick = Vector2.zero;
                UpdateMoveTime(delayTime.x, delayTime.y);
            }
        }
        else
        {
            // if movement input has not been set yet
            if (!_joystickSet)
            {
                joystick = Random.insideUnitCircle;
                _joystickSet = true;
                _delayedSet = false;
                UpdateMoveTime(moveTime.x, moveTime.y);
            }
        }

        // if it is time for the delayed bool to flip
        if (Time.time >= _nextMoveTime)
        {
            _delayed = !_delayed;
        }

    }

    /// <summary>
    /// adds a random range to the next move time
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    private void UpdateMoveTime(float min, float max)
    {
        _nextMoveTime = Time.time + Random.Range(min, max);
    }

    /// <summary>
    /// adds a set number to the next move time
    /// </summary>
    /// <param name="time"></param>
    private void UpdateMoveTime(float time)
    {
        _nextMoveTime = Time.time + time;
    }
}

