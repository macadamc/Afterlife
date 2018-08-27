using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.Variables;
using UnityEngine.Tilemaps;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public class FlyingMovementController : MovementController
{
    public PIDController PID;
    public float targetFlyingHeight = 1f;
    public float force;

    FakeZAxis m_FakeZAxis;

    public override void OnEnable()
    {
        base.OnEnable();
        m_FakeZAxis = GetComponent<FakeZAxis>();
    }
    [ShowInInspector]
    public float SpeedPercentage { get {  return Mathf.Clamp(Mathf.Abs(_moveVector.x) + Mathf.Abs(_moveVector.y), 0, moveSpeed.Value) / moveSpeed.Value; } }

    public override bool IsMoving{ get {return base.IsMoving || m_FakeZAxis.height > 0.1f;}}

    public override void FixedUpdate()
    {
        if(Ic.joystick != Vector2.zero) { }

        m_FakeZAxis.velocity = PID.Seek(targetFlyingHeight, m_FakeZAxis.height) * force;

        base.FixedUpdate();
    }
}

