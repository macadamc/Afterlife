﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : SteeringBehaviour
{
    public float jitter;
    public bool resetWanderTargetOnDisable;
    float _jitter;
    //public float distance;
    //public float radius;
    Vector2 wanderTarget;


    public override void OnDisable()
    {
        base.OnDisable();
        if(resetWanderTargetOnDisable)
            wanderTarget = Vector2.zero;
    }

    public override void Tick()
    {
        float _jitter = jitter * Time.deltaTime;
        wanderTarget += new Vector2(agent.RandomBinomial * _jitter, agent.RandomBinomial * _jitter);
        wanderTarget.Normalize();
        wanderTarget *= agent.moveSpeed;

        Vector2 targetInWorldSpace = transform.TransformPoint(wanderTarget);
        targetInWorldSpace -= (Vector2)transform.position;

        agent.AddSteeringForce(targetInWorldSpace, priority);
    }
}