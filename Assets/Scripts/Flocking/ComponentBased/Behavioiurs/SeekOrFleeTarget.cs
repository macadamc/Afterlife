using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class SeekOrFleeTarget : SteeringBehaviour
{
    public bool isSeek = true;
    [ShowIf("isSeek"), MinMaxSlider(0, 17, true)]
    public Vector2 arrivalRadius = new Vector2(1f, 1f);
    [ShowIf("isSeek")]
    public UnityEvent OnArrival;

    public UnityEvent noTarget;

    protected Vision vision;
    protected Transform target;
    protected bool last;


    public override void OnEnable()
    {
        base.OnEnable();
        if (vision == null)
            vision = agent.GetComponent<Vision>();

        if (target == null && vision.targets.transforms.Count > 0)
            target = vision.targets.transforms[0];

        vision.onNoTargets.AddListener(OnNoTarget);
    }
    public override void OnDisable()
    {
        base.OnDisable();
        vision.onNoTargets.RemoveListener(OnNoTarget);
    }

    public override void Tick()
    {
        if (vision.targets.transforms.Count == 0)
            return;

        Vector3 targetPosition = vision.targets.transforms[0].position;
        float dist = Vector2.Distance(agent.transform.position, targetPosition);
        bool hasArrived = dist <= arrivalRadius.x;

        if (hasArrived == false)
        {
            Vector2 neededVelocity = (targetPosition - agent.transform.position).normalized * agent.moveSpeed;

            if (dist <= arrivalRadius.y)
                neededVelocity = neededVelocity - agent.velocity;

            agent.AddSteeringForce(neededVelocity, priority);
        }
        else if (last != hasArrived)
            OnArrival.Invoke();


        last = hasArrived;
    }

    protected void OnNoTarget()
    {
        noTarget.Invoke();
    }

}
