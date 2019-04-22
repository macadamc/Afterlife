using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBouncy : MonoBehaviour
{
    public Transform root;
    public LayerMask deathMask;
    public LayerMask bounceMask;

    public Rigidbody2D rb;
    public Projectile p;
    void OnTriggerEnter2D(Collider2D other)
    {
        if ((bounceMask.value & 1 << other.gameObject.layer) != 0)
        {
            //bounce.
            //p. = Vector2.Reflect(rb.velocity.normalized, (other.transform.position - transform.position).normalized);
        }
        else if((deathMask.value & 1 << other.gameObject.layer) != 0)
        {
            Destroy(root);
        }
        
    }
}
