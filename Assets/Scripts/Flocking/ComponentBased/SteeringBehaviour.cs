using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringBehaviour : MonoBehaviour
{
    public float priority = 1;
    protected FlockingAgent agent;

    public virtual void OnEnable()
    {
        if (agent == null)
        {
            agent = GetComponent<FlockingAgent>();

            if(agent == null)
            {
                agent = GetComponentInParent<FlockingAgent>();
            }
            Debug.Assert(agent != null, "Flocking Agent Not Found.");
        }
        agent.OnUpdate.AddListener(Tick);
    }

    public virtual void OnDisable()
    {
        agent.OnUpdate.RemoveListener(Tick);
    }

    public virtual void Tick() { }
}

