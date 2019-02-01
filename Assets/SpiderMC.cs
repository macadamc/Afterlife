using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpiderMC : FlockingAgent
{
    public float decel = .5f;
    public float decelDeadZone = .1f;

    protected bool m_LockInput;
    protected Vector2 m_LockedInput;

    public override void Update()
    {
        if (Ic == null)
            return;

        if (PauseManager.Instance != null && PauseManager.Instance.Paused)
            return;

        if (Knockedback || _stunned)
        {
            if (anim != null)
                anim.SetBool(setAnimationBoolToIsMoving, false);

            // sets move vector to zero
            _moveVector = Vector2.Lerp(_moveVector, Vector2.zero, smoothing);

            // check to see if stun time is over and input controller will regain control
            if (Time.time > _nextMoveTime)
                _stunned = false;
        }
        else
        {
            if (m_LockInput == false)
            {
                acceleration = Vector2.zero;
                OnUpdate.Invoke();

                acceleration = Vector2.ClampMagnitude(acceleration, maxAcceleration);
                velocity = acceleration;
                velocity = Vector2.ClampMagnitude(velocity, maxVelocity);
                m_LockedInput = velocity;
                m_LockInput = true;
            }

            // use normal input from InputController.joystick
            _moveVector = Vector2.Lerp(_moveVector, velocity, smoothing);

            if (anim != null)
                anim.SetBool(setAnimationBoolToIsMoving, IsMoving);
        }
    }

    private void LateUpdate()
    {
        if (m_LockInput & velocity.magnitude == 0f)
            m_LockInput = false;

        if(velocity.magnitude > 0f)
        {
            velocity *= (1f - decel * Time.deltaTime);

            if (velocity.magnitude <= decelDeadZone)
                velocity = Vector2.zero;
        }
        
    }
}