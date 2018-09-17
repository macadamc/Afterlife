using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using ShadyPixel.StateMachine;

public class ChangeTargetZHeight : State
{

    public FlyingMovementController mc;
    public FakeZAxis fakeZ;
    public bool useSpeedPercentage = false;
    public bool SetLastTargetHeightOnDisable;
    
    [HideIf("useSpeedPercentage")]
    public float newHeight;
    [HideIf("useSpeedPercentage")]
    public bool changeStateAtTargetHeight;
    [ShowIf("useSpeedPercentage")]
    public float maxHeight;

    float _lastHeight;

    protected override void OnEnable()
    {
        _lastHeight = mc.targetFlyingHeight;

        mc.targetFlyingHeight = newHeight;

        base.OnEnable();
    }

    protected override void OnDisable()
    {
        if(SetLastTargetHeightOnDisable)
            mc.targetFlyingHeight = _lastHeight;

        base.OnDisable();
    }

    public void Update()
    {
        if (useSpeedPercentage)
            mc.targetFlyingHeight = mc.SpeedPercentage * maxHeight;
        else
        {
            if (changeStateAtTargetHeight && fakeZ != null)
            {
                if(newHeight <= fakeZ.height + .5f && newHeight >= fakeZ.height - .5f)
                {
                    StateMachine.Next();
                }
            }
                
        }
    }

}
