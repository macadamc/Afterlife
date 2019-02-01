using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLookDirectionToAgentVelocity : MonoBehaviour {
    public bool useOrthognial;
    protected FlockingAgent agent;
    protected InputController input;
	// Use this for initialization
	void Start ()
    {
        agent = GetComponentInParent<FlockingAgent>();
        input = agent.GetComponent<InputController>(); ;
	}
	
	// Update is called once per frame
	void Update ()
    {
        var dir = agent.velocity.normalized;

        if (useOrthognial)
        {
            if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            {
                dir.y = 0;
            }
            else
            {
                dir.x = 0;
            }
        }

        input.SetLookDirection(dir);
    }
}
