using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyKnockback : InteractOnTrigger2D
{
    public float knockbackForce = 5f;
    public bool useRotation = true;
    //public float useRotationPercent = 0.5f;

    // trigger collisions
    protected override void ExecuteOnEnter(Collider2D other)
    {
        base.ExecuteOnEnter(other);
        Knockback(other.gameObject);
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
        else
        {

            knockback += (knockbackObj.transform.position - transform.position).normalized;
        }

        // multiplies direction by the force of the knockback
        knockback *= knockbackForce;

        // applies the knockback to the movement controller
        Mc.AddKnockback(knockback);
    }

}
