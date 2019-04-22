using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ConstrainMovementFromPoint : SteeringBehaviour {

    public Transform point;
    public float distanceFromPoint = 1f;
    [MinMaxSlider(0, 20, ShowFields = true)]
    public Vector2 minMaxPriority;

    public bool useArrival;
    [ShowIf("useArrival")]
    public float arrivalRadius = 1f;

    public override void Tick()
    {
        var dist = Mathf.Clamp((point.position - agent.transform.position).magnitude, 0, distanceFromPoint);
        priority = Mathf.Lerp(minMaxPriority.x, minMaxPriority.y, (dist / distanceFromPoint));
        agent.AddSteeringForce(agent.Seek(point.position, useArrival ? arrivalRadius : 0f), priority);
    }
}
