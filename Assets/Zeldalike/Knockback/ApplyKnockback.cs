using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyKnockback : TriggerZone
{
    public float knockbackForce = 5f;
    public bool useRotation = true;
    //public float useRotationPercent = 0.5f;

    // trigger collisions
    protected override void OnEnter(Collider2D collision)
    {
        Knockback(collision.gameObject);
    }

    private void Knockback(GameObject knockbackObj)
    {
        // tried to find a movement controller, if null return out of statement
        MovementController Mc = knockbackObj.GetComponentInChildren<MovementController>();

        if (Mc == null)
            return;

        // get direction between the two objects
        Vector3 knockback = Vector2.zero;
        if(useRotation)
        {
            knockback += transform.rotation * Vector3.right;
        }

        knockback += (knockbackObj.transform.position - transform.position).normalized;

        if (useRotation)
            knockback /= 2f;

        // multiplies direction by the force of the knockback
        knockback *= knockbackForce;

        // applies the knockback to the movement controller
        Mc.AddKnockback(knockback);
    }

}
