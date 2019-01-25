using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avoidance : SteeringBehaviour
{
    public float obstacleAvoidanceRadius;
    public LayerMask layerMask;

    public override void Tick()
    {

        Vector2 avoidVector = new Vector2();
        var enemyList = FlockingAgentManager.Instance.GetObstacles(agent, obstacleAvoidanceRadius, layerMask);
        foreach (var enemy in enemyList)
        {
            avoidVector += RunAway(enemy.transform.position);
        }
        agent.AddSteeringForce(avoidVector.normalized, priority);
    }

    Vector2 RunAway(Vector2 target)
    {
        Vector2 neededVelocity = ((Vector2)transform.position - target).normalized * agent.moveSpeed;
        return neededVelocity - agent.velocity;
    }
}