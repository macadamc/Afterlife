using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetProjectileXDist : ShadyPixel.StateMachine.State
{
    public float distance;
    Projectile proj;

    protected override void OnEnable()
    {
        
    }

    private void Update()
    {
            proj = GetComponentInParent<ProjectileRef>().value;

        if(proj != null && Vector2.Distance(proj.transform.position, transform.position) <= distance)
            Next();
    }
}
