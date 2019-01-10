﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;
using Sirenix.OdinInspector;
using UnityEngine.Events;

[System.Serializable]
[InlineProperty]
public class ActionVariable
{
    public enum Type
    {
        Additive, Absolute
    }
    [HorizontalGroup, HideLabel]
    public float value;
    [HorizontalGroup, HideLabel]
    public Type type;
}

[System.Serializable]
public class ProjectileAction
{
    public ActionVariable speed;
    public ActionVariable acceleration;
    public ActionVariable angularVelocity;
    public ActionVariable angularAcceleration;

    public UnityEvent onActionStart;
    public float delayBeforeNextAction;
}


public class Projectile : MonoBehaviour
{
    [TabGroup("Default Variables")]
    public float speed = 1f;
    [TabGroup("Default Variables")]
    public float acceleration;
    [TabGroup("Default Variables")]
    public Vector2 speedBounds = new Vector2(-20f,20f);
    [TabGroup("Default Variables")]
    public float angularVelocity;
    [TabGroup("Default Variables")]
    public float angularAcceleration;
    [TabGroup("Default Variables")]
    public Vector2 angularVelocityBounds = new Vector2(-1080f, 1080f);
    [TabGroup("Default Variables")]
    public bool destroyOnCollision;
    [TabGroup("Default Variables"), ShowIf("destroyOnCollision")]
    public LayerMask collisionLayers;
    [TabGroup("Default Variables")]
    public UnityEvent onProjectileCollision;

    //public Transform creator;
    /*
    [TabGroup("Action Queue")]
    public float delayBeforeStartingQueue;
    [TabGroup("Action Queue")]
    public List<ProjectileAction> actionQueue = new List<ProjectileAction>();
    */

    Rigidbody2D rb;

    private void OnTriggerEnter2D(Collider2D collision)
    { 
        if ((collisionLayers.value & 1 << collision.gameObject.layer) != 0)
        {
            onProjectileCollision.Invoke();

            if (destroyOnCollision)
                Destroy(transform.gameObject);
        }
    }

    private void OnEnable()
    {
        if(rb == null)
            rb = GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        speed = Mathf.Clamp(speed + acceleration * Time.deltaTime, speedBounds.x, speedBounds.y);
        angularVelocity = Mathf.Clamp(angularVelocity + angularAcceleration * Time.deltaTime, angularVelocityBounds.x, angularVelocityBounds.y);
        rb.velocity = transform.right * speed;
        transform.Rotate(new Vector3(0, 0, angularVelocity * Time.deltaTime));
    }
}