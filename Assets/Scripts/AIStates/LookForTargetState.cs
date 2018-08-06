using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.StateMachine;

public class LookForTargetState : State
{
    Vision _vision;
    public GameObject forceState;

    protected override void OnEnable()
    {
        _vision = GetComponentInParent<Vision>();
        base.OnEnable();
    }

    private void Update()
    {
        if(_vision.targets.transforms.Count > 0)
        {
            if (forceState)
                ChangeState(forceState);
            else
                Next();
        }
    }
}
