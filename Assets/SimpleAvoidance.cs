using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;

public class SimpleAvoidance : MonoBehaviour {
    public LayerMask mask;
    public float avoidRange = 5f;
    public float steerForce = 1f;
    MovementController mc;

    [ReadOnly]
    public Vector2 value;
	
	// Update is called once per frame
	void Update ()
    {
        // find closest hit.
        var closestHit = Physics2D.CircleCastAll(transform.position, avoidRange, Vector2.zero, 0f, mask.value).OrderBy((RaycastHit2D hit) => { return hit.distance; }).First();

        if(closestHit != null)
        {
            // move away from closestHit.point.
            value = (new Vector3(closestHit.point.x, closestHit.point.y) - transform.position).normalized * steerForce;

            // todo: rework how the look direction and joystick work. so they can be used additively for steering behaviours.
            //mc.Ic.joystick += value;
        }
    }
}
