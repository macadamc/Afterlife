using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoomerangProjectileBehaviour : Projectile
{
    public Transform returnTarget;
    public float maxForce;

    public float returnThresholdDst = 0.5f;
    public float returnThresholdSpd = 0.5f;
    public float rotateSpeed = 180;
    bool returnToUser;
    HeldItem heldItem;
    Quaternion neededRotation;
    public CircleCollider2D coll;
    public float BounceTime = 1f;
    float bounceEndTime;
    bool isBouncing;


    public LayerMask obstacleLayers;

    public override void Update()
    {
        base.Update();

        if(isBouncing)
        {
            if (Time.time >= bounceEndTime)
            {
                isBouncing = false;
            }
            else
                return;
        }

        if(returnToUser)
        {
            if (heldItem == null)
                heldItem = GetComponent<HeldItem>();

            var vectorToTarget = returnTarget.position - transform.position;
            var angleToTarget = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            var quaternionToTarget = Quaternion.AngleAxis(angleToTarget, Vector3.forward);
            transform.localRotation = Quaternion.Slerp(transform.rotation, quaternionToTarget, Time.deltaTime * rotateSpeed);

            DistanceCheck(heldItem.User);
        }
        else
        {
            if (speed < returnThresholdSpd)
            {
                SetReturnState(true);
            }
            
        }
    }

    protected virtual void RotateObject(Transform transformToRotate, Vector2 _direction)
    {
        //  gets the angle from the look direction
        float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;

        //  rotates object to face the new angle
        transformToRotate.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void SetReturnTargetToUser(ItemController user)
    {
        returnTarget = user.transform;
    }

    public void AddVelocityToUserHeldTime(ItemController user)
    {
        speed += Mathf.Clamp01(user.HeldTime / ((ItemSpawnPrefabWithCharge)user.currentItem).chargeTime) * maxForce;
    }

    public void DistanceCheck(ItemController user)
    {
        
        if (Vector2.Distance(transform.position,returnTarget.position) < returnThresholdDst)
        {
            
            user.ResetUseTime();
            Destroy(gameObject);
        }

    }
    public void SetReturnState(bool state)
    {
        acceleration = 5f;
        returnToUser = state;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        var hit = Physics2D.CircleCast(transform.position, coll.radius, Vector2.zero, 0f, obstacleLayers.value);
        if ((obstacleLayers.value & 1 << collision.gameObject.layer) != 0 && returnToUser == false)
        {
            if (hit.collider != null)
            {
                //var hitDir = hit.point - (Vector2)transform.position;
                //hitDir.Normalize();

                //var bounceDir = Vector2.Reflect(hitDir, hit.normal);

                //transform.rotation = Quaternion.LookRotation(bounceDir);

            }

            SetReturnState(true);
        }
        else
        {
            base.OnTriggerEnter2D(collision);
        }
            
    }
}
