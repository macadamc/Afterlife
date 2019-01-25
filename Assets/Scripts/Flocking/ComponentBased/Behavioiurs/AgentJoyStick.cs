using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentJoyStick : SteeringBehaviour
{
    InputController ic;

    public override void OnEnable()
    {
        base.OnEnable();
        if (ic == null)
        {
            ic = agent.GetComponent<InputController>();
            Debug.Assert(ic != null, "inputController Not Found.");
        }
    }

    public override void Tick()
    {
        agent.AddSteeringForce(ic.joystick, priority);
    }
}