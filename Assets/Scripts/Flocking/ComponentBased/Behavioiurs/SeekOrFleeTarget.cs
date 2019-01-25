using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekOrFleeTarget : SteeringBehaviour
{
    public bool isSeek = true;

    protected Vision vision;
    protected Transform target;

    public override void OnEnable()
    {
        base.OnEnable();
        if (vision == null)
            vision = agent.GetComponent<Vision>();

        if (target == null && vision.targets.transforms.Count > 0)
            target = vision.targets.transforms[0];
    }

    public override void Tick()
    {
        if (vision.targets.transforms.Count == 0)
            return;

        Vector3 targetPosition = vision.targets.transforms[0].position;

        Vector2 neededVelocity = (targetPosition - agent.transform.position).normalized * agent.moveSpeed;
        neededVelocity = neededVelocity - agent.velocity;

        agent.AddSteeringForce(neededVelocity, priority);
    }

}
