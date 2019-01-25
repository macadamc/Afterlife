using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnitism : SteeringBehaviour
{
    public float playerMagnitismRadius;

    GameObject player;

    public override void Tick()
    {
        if (player == null)
            player = Player.Instance.gameObject;

        Vector2 magnitismVector = new Vector2();
        float distance = Vector2.Distance(player.transform.position, transform.position);
        if (distance > playerMagnitismRadius)
        {
            agent.AddSteeringForce(magnitismVector, priority);
        }
        else
        {
            magnitismVector = (player.transform.position - transform.position);
            agent.AddSteeringForce(magnitismVector.normalized, priority);
        }
    }
}