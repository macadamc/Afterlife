using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringBehaviourState : ShadyPixel.StateMachine.State
{
    public float priority;
    protected FlockingAgent agent;

    protected override void OnEnable()
    {
        base.OnEnable();

        if (agent == null)
        {
            agent = GetComponent<FlockingAgent>();

            if (agent == null)
            {
                agent = GetComponentInParent<FlockingAgent>();
            }
            Debug.Assert(agent != null, "Flocking Agent Not Found.");
        }
        agent.OnUpdate.AddListener(Tick);
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        agent.OnUpdate.RemoveListener(Tick);
    }

    public virtual void Tick() { }
}