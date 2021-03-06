﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public class Vision : MonoBehaviour
{
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    public Transform visionTransform;

    [Range(0, 360)]
    public int angleSnap = 0;
    public bool useInputControllerForDirection;
    public InputController ic;

    public List<string> visionTags = new List<string>();
    //[Required]
    //public Targets targets;

    public UnityEvent onTargetSeen;
    public UnityEvent onNoTargets;
    bool hasTargets;
    [ShowInInspector, ListDrawerSettings(IsReadOnly = true)]
    public List<Transform> targets { get; protected set; }

    private void OnEnable()
    {
        targets = new List<Transform>();
        hasTargets = false;

    } 

    /// <summary>
    /// If this vision object can see open ground at a position in the world.
    /// </summary>
    /// <param name="targetPosition"></param>
    /// <returns></returns>
    public bool HasVisionToGround(Vector2 targetPosition)
    {
        Vector2 dirToTarget = (targetPosition - (Vector2)visionTransform.position).normalized;
        bool r = false;
        if (Vector2.Angle(visionTransform.right, dirToTarget) < viewAngle / 2)
        {
            float dstToTarget = Vector2.Distance(visionTransform.position, targetPosition);

            RaycastHit2D hit = Physics2D.Raycast(visionTransform.position, dirToTarget, dstToTarget, obstacleMask);

            // if null, nothing was hit, which means there is open ground there
            if (hit.transform == null)
                r = true;
        }

        return r;
    }

    public bool HasTargets { get { return targets != null && targets.Count > 0;  } }

    private Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += visionTransform.eulerAngles.z;
        }
        return new Vector2(Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), Mathf.Sin(angleInDegrees * Mathf.Deg2Rad));
    }

    private void Update()
    {
        if (PauseManager.Instance != null && PauseManager.Instance.Paused)
            return;

        if (useInputControllerForDirection && ic != null)
        {
            if(angleSnap > 0)
                RotateObj(angleSnap);
            else
                RotateObj();
        }

        FindVisibleTargets();
    }

    private void RotateObj(float snapAmount)
    {
        float angle = Mathf.Atan2(ic.lookDirection.y, ic.lookDirection.x) * Mathf.Rad2Deg;
        visionTransform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        var vec = visionTransform.eulerAngles;
        vec.x = Mathf.Round(vec.x / snapAmount) * snapAmount;
        vec.y = Mathf.Round(vec.y / snapAmount) * snapAmount;
        vec.z = Mathf.Round(vec.z / snapAmount) * snapAmount;
        visionTransform.eulerAngles = vec;
    }

    private void RotateObj()
    {
        float angle = Mathf.Atan2(ic.lookDirection.y, ic.lookDirection.x) * Mathf.Rad2Deg;
        visionTransform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    
    private void FindVisibleTargets()
    {
        //targets.transforms.Clear();
        targets.Clear();
        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(visionTransform.position, viewRadius, targetMask);
        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            if (targetsInViewRadius[i].transform == transform)
                continue;

            bool _hasTag = false;
            if(visionTags.Count !=0)
            {
                foreach (string tag in visionTags)
                {
                    if (targetsInViewRadius[i].gameObject.CompareTag(tag))
                        _hasTag = true;
                }
            }

            if(visionTags.Count == 0 || _hasTag)
            {
                bool _canSee = HasVisionToGround(targetsInViewRadius[i].transform.position);

                if (_canSee)
                    targets.Add(targetsInViewRadius[i].transform);
            }
        }
        if (targets.Count > 0)//targets.transforms.Count > 0)
        {
            if(hasTargets == false)
            {
                onTargetSeen.Invoke();
                hasTargets = true;
            }
        }
        else
        { 
            if(hasTargets)
            {
                hasTargets = false;
                onNoTargets.Invoke();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 viewAngleA = DirFromAngle(-viewAngle / 2, false);
        Vector3 viewAngleB = DirFromAngle(viewAngle / 2, false);

        Gizmos.color = Color.grey;
        Gizmos.DrawWireSphere(transform.position, viewRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + viewAngleA * viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + viewAngleB * viewRadius);

        
        Gizmos.color = Color.red;
        if (targets == null)
        return;
        foreach (Transform visibleTarget in targets)//targets.transforms)
        {
        Gizmos.DrawLine(visionTransform.position, visibleTarget.position);
        }
        
    }

}
