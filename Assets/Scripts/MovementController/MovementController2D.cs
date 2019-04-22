using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController2D : MonoBehaviour
{
    public delegate void OnUpdate();
    public OnUpdate onUpdate;

    public Vector2 deltaVelocity;

    public Rigidbody2D rb;

    public void AddForce(Vector2 change)
    {
        deltaVelocity += change;
    }

    public void SetForce(Vector2 velocity)
    {
        deltaVelocity = velocity;
    }

    void Update()
    {
        onUpdate.Invoke();
        rb.velocity = deltaVelocity;
        deltaVelocity = Vector2.zero;
    }
}
