using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
/*
public class SpiderMovementController : MovementController
{
    public bool Grounded { get; private set; }

    [MinMaxSlider(0,2, true)]
    public Vector2 groundedWaitTime;

    [MinMaxSlider(0, 2, true)]
    public Vector2 airWaitTime;

    [ReadOnly]
    public float actionTime = 0f;

    public override void FixedUpdate()
    {
        if(PauseManager.Instance != null && PauseManager.Instance.Paused)
            return;

        // toggle grounded/InAir states
        if(Time.time >= actionTime)
        {
            UpdateState();
        }

        if(Grounded)
        {
            if (TileReference != null)
            {
            _tile = TileReference.GetTile(transform.position);
            }

            _moveVector = Vector2.zero;
        }       

        if (_moveVector.magnitude > 0.1)
        {
            if (Time.time > _nextStepTime)
                Step();
        }
        else
            _nextStepTime = Time.time;

        Rb.velocity = _moveVector + _knockbackVector;
        UpdateKnockback();
    }

    public override void Update()
    {
        if (Ic == null)
            return;

        if (PauseManager.Instance != null && PauseManager.Instance.Paused)
            return;

        if (Knockedback || _stunned)
        {
            // sets move vector to zero
            _moveVector = Vector2.Lerp(_moveVector, Vector2.zero * moveSpeed, smoothing);

            // check to see if stun time is over and input controller will regain control
            if (Time.time > _nextMoveTime)
                _stunned = false;
        }
        else
        {
            // use normal input from InputController.joystick
            _moveVector = Vector2.Lerp(_moveVector, Ic.joystick * moveSpeed, smoothing);
        }
    }

    public void setActionTime(Vector2 delay)
    {
        actionTime = Time.time + Random.Range(delay.x, delay.y);
    }

    public void UpdateState()
    {
        Grounded = !Grounded;
        if (Grounded)
        {
            setActionTime(groundedWaitTime);
        }
        else
        {
            setActionTime(airWaitTime);
        }
    }
}
*/

public class SpiderMovementController : MovementController
{
    FakeZAxis m_fakeZ;
    bool m_LockInput;
    //Vector2 m_LockedInput;

    public float jumpStrength;

    private void Awake()
    {
        m_fakeZ = GetComponent<FakeZAxis>();
    }

    public bool OnGround { get { return m_fakeZ.height <= .1f; } }

    public override void FixedUpdate()
    {
        if(OnGround && Ic.joystick != Vector2.zero && m_LockInput == false)
        {
            m_LockInput = true;
            //m_LockedInput = Ic.joystick;
            m_fakeZ.velocity = -jumpStrength;
        }
        base.FixedUpdate();
    }
    private void LateUpdate()
    {
        if(OnGround && m_LockInput == true && m_fakeZ.velocity == 0f)
            m_LockInput = false;


        base.Update();
    }
}
