using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seperation : SteeringBehaviour
{
    public float seperationRadius;

    public override void Tick()
    {
        Vector2 seperationVector = new Vector2();
        var agents = FlockingAgentManager.Instance.GetNeighbors(agent, seperationRadius);

        foreach (var agent in agents)
        {
            if (agent.IsInFOV(agent.transform.position))
            {
                Vector2 movingTowards = transform.position - agent.transform.position;
                if (movingTowards.magnitude > 0)
                    seperationVector += movingTowards.normalized / movingTowards.magnitude;
            }
        }

        agent.AddSteeringForce(seperationVector.normalized * agent.moveSpeed, priority);
    }
}