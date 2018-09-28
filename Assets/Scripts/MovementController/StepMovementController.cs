using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepMovementController : MovementController
{
    public Vector2 stepLength;
    public Vector2 waitLength;
    private float changeStateTime;
    private bool inStep;
    private Vector2 input = Vector2.zero;

    public override void Update()
    {
        if (Ic == null)
            return;

        if (PauseManager.Instance != null && PauseManager.Instance.Paused)
            return;

        if (Knockedback || _stunned)
        {
            anim?.SetBool(setAnimationBoolToIsMoving, false);

            // sets move vector to zero
            _moveVector = Vector2.Lerp(_moveVector, Vector2.zero * moveSpeed, smoothing);

            // check to see if stun time is over and input controller will regain control
            if (Time.time > _nextMoveTime)
                _stunned = false;
        }
        else
        {
            if(Time.time > changeStateTime)
            {
                inStep = !inStep;

                if (inStep)
                {
                    SetStateTimer(stepLength);
                    input = Ic.joystick;
                }
                else
                    SetStateTimer(waitLength);

            }
            else
            {
                if (inStep && input.magnitude > 0)
                {
                    _moveVector = Vector2.Lerp(_moveVector, input * moveSpeed, smoothing);
                    anim?.SetBool(setAnimationBoolToIsMoving, true);
                }
                else
                {
                    _moveVector = Vector2.Lerp(_moveVector, Vector2.zero, smoothing);
                    anim?.SetBool(setAnimationBoolToIsMoving, false);
                }
            }
        }
    }

    void SetStateTimer(Vector2 range)
    {
        changeStateTime = Time.time + Random.Range(range.x, range.y);
    }
}
