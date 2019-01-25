using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : SteeringBehaviour
{
    public float jitter;
    float _jitter;
    public float distance;
    public float radius;
    Vector2 wanderTarget;

    public override void Tick()
    {
        float _jitter = jitter * Time.deltaTime;
        wanderTarget += new Vector2(agent.RandomBinomial * jitter, agent.RandomBinomial * jitter);
        wanderTarget.Normalize();
        wanderTarget *= radius;
        Vector2 targetInWorldSpace = transform.TransformPoint(wanderTarget);
        targetInWorldSpace -= (Vector2)transform.position;

        agent.AddSteeringForce(targetInWorldSpace, priority);
    }
}
