using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Allignment : SteeringBehaviour
{
    public float allignmentRadius;

    public override void Tick()
    {
        Vector2 allignVector = new Vector2();
        var agents = FlockingAgentManager.Instance.GetNeighbors(agent, allignmentRadius);

        foreach (var agent in agents)
        {
            if (agent.IsInFOV(agent.transform.position))
            {
                allignVector += agent.velocity;
            }
        }

        agent.AddSteeringForce(allignVector.normalized, priority);
    }
}