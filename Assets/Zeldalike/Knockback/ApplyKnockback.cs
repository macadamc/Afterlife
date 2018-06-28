using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyKnockback : MonoBehaviour
{
    public float knockbackForce = 5f;

    // solid collisions
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Knockback(collision.gameObject);
    }

    // trigger collisions
    private void OnTriggerEnter2D(Collider2D collision)
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
        Vector2 knockback = (knockbackObj.transform.position - transform.position).normalized;

        // multiplies direction by the force of the knockback
        knockback *= knockbackForce;

        // applies the knockback to the movement controller
        Mc.AddKnockback(knockback);
    }

}
