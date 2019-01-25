using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cohesion : SteeringBehaviour
{
    public float cohesionRadius;

    public override void Tick()
    {
        Vector2 cohesionVector = new Vector2();
        int countAgents = 0;

        var neighbors = FlockingAgentManager.Instance.GetNeighbors(agent, cohesionRadius);

        foreach (var agent in neighbors)
        {
            if (agent.IsInFOV(agent.transform.position))
            {
                cohesionVector += (Vector2)agent.transform.position;
                countAgents++;
            }
        }
        if (countAgents != 0)
        {
            cohesionVector /= countAgents;
            cohesionVector = cohesionVector - (Vector2)transform.position;
            cohesionVector.Normalize();
        }
        agent.AddSteeringForce(cohesionVector, priority);
    }
}
