using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchTargetSteeringVelocity : SteeringBehaviour
{
    FlockingAgent target_Agent;
    Vision _vision;
    public bool invert;

    public override void OnEnable()
    {
        base.OnEnable();

        if (_vision == null)
            _vision = agent.GetComponent<Vision>();

        if (_vision.HasTargets)
            target_Agent = _vision.targets[0].GetComponentInParent<FlockingAgent>();//_vision.targets.transforms[0].GetComponentInParent<FlockingAgent>();
    }

    public override void Tick()
    {
        if (target_Agent == null)
            return;

        if (!invert)
            agent.AddSteeringForce(target_Agent.velocity, priority);
        else
            agent.AddSteeringForce(-target_Agent.velocity, priority);
    }
}