using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentJoyStick : SteeringBehaviour
{
    public override void Tick()
    {
        agent.AddSteeringForce(agent.Ic.joystick * agent.moveSpeed, priority);
    }
}