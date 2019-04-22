using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


public class SetLookDirectionToAgentVelocity : MonoBehaviour {
    public bool useOrthognial;
    protected FlockingAgent agent;
    public bool useFrameBuffer;
    [ShowIf("useFrameBuffer")]
    public int frames = 1;
    int _frames = 0;
	// Use this for initialization
	void Start ()
    {
        agent = GetComponentInParent<FlockingAgent>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (useFrameBuffer)
        {
            _frames++;
            if (_frames <= frames)
                return;
            else
                _frames = 0;
        }

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

        agent.Ic.SetLookDirection(dir);
    }
}
