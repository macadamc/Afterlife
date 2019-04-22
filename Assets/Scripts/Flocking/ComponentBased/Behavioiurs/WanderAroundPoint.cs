using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


public class WanderAroundPoint : SteeringBehaviour
{
    [Range(1, 10)]
    public int precision = 2;
    public float wanderJitter;
    public float seekPointJitter;

    public Transform point;
    [MinMaxSlider(0, 27, ShowFields = true)]
    public Vector2 minMaxDistanceFromPoint;
    [MinMaxSlider(-20, 20, ShowFields = true)]
    public Vector2 minMaxPriority;
    public AnimationCurve priorityOverDistance;

    Vector2 f1;
    Vector2 f2;
    float _jitter;
    Vector2 wanderTarget;


    public override void OnDisable()
    {
        base.OnDisable();
        wanderTarget = Vector2.zero;
    }

    public override void Tick()
    {
        var dist = Mathf.Clamp((point.position - agent.transform.position).magnitude, 0, minMaxDistanceFromPoint.y);

        if (dist >= minMaxDistanceFromPoint.x)
        {
            float percentage = (dist - minMaxDistanceFromPoint.x) / (minMaxDistanceFromPoint.y - minMaxDistanceFromPoint.x);
            if (priorityOverDistance != null)
            {
                percentage = priorityOverDistance.Evaluate(percentage);
            }
            
            priority = Mathf.Lerp(minMaxPriority.x, minMaxPriority.y, percentage);
            priority = (float)System.Math.Round(priority, precision);
        }
        else
            priority = 0;


        float _jitter = wanderJitter * Time.deltaTime;
        float _seekPointJitter = seekPointJitter * Time.deltaTime;

        wanderTarget += new Vector2(agent.RandomBinomial * _jitter, agent.RandomBinomial * _jitter);
        wanderTarget.Normalize();
        wanderTarget *= agent.moveSpeed;

        f1 = agent.Seek((Vector2)point.position + new Vector2(agent.RandomBinomial * _seekPointJitter, agent.RandomBinomial * _seekPointJitter) * agent.moveSpeed, minMaxDistanceFromPoint.x);
        f2 = agent.Seek((Vector2)transform.TransformPoint(wanderTarget));

        agent.AddSteeringForce(f1, priority);
        agent.AddSteeringForce(f2, minMaxPriority.y - priority);

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(point.position, minMaxDistanceFromPoint.y);
        Gizmos.DrawWireSphere(point.position, minMaxDistanceFromPoint.x);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + ((Vector3)f1 * priority));
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + ((Vector3)f2 * (minMaxPriority.y - priority)));
    }
}