using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Items/New Chargable Item")]
public class ItemSpawnPrefabWithCharge : Item
{
    public GameObject itemToHold;
    public GameObject itemToSpawnOnUse;
    [ShowIf("useOtherPrefab")]
    public GameObject otherItemToSpawn;

    [Range(0.0f, 1.0f)]
    public float moveSpeedPercent;
    public float itemCooldownTime;
    public float itemMoveStunTime;
    [Range(0, 360)]
    public int angleSnap = 45;

    private GameObject heldItem;

    public bool useOtherPrefab;
    public float chargeTime;


    public override void Begin(ItemController user)
    {
        if (itemToHold != null)
        {
            if (user.MovementController != null)
            {
                user.MovementController.oldMoveSpeed.Value = user.MovementController.moveSpeed.Value;
                user.MovementController.moveSpeed.Value = user.MovementController.moveSpeed.Value * moveSpeedPercent;
            }

            heldItem = Instantiate(itemToHold, user.SpawnTransform);

            var HeldItemListeners = heldItem.GetComponent<HeldItem>();
            if (HeldItemListeners != null)
            {
                HeldItemListeners.Init(user);
                HeldItemListeners.Begin(user);
            }

            heldItem.transform.localScale = user.transform.localScale;
            heldItem.transform.position = (Vector2)user.SpawnTransform.position;

            if (angleSnap == 0)
                RotateObject(heldItem.transform, user.InputController.lookDirection);
            else
                RotateObject(heldItem.transform, user.InputController.lookDirection, angleSnap);
        }

    }

    public override void Hold(ItemController user)
    {
        if (itemToHold != null && heldItem != null)
        {
            heldItem.transform.localScale = user.transform.localScale;

            if (angleSnap == 0)
                RotateObject(heldItem.transform, user.InputController.lookDirection);
            else
                RotateObject(heldItem.transform, user.InputController.lookDirection, angleSnap);
        }
    }

    public override void End(ItemController user)
    {
        if (user.MovementController != null)
        {
            user.MovementController.moveSpeed.Value = user.MovementController.oldMoveSpeed.Value;
        }

        GameObject prefabToSpawn = null;

        if (Time.time >= user.StartTime + chargeTime && useOtherPrefab)
            prefabToSpawn = otherItemToSpawn;
        else
            prefabToSpawn = itemToSpawnOnUse;

        GameObject spawnObject = Instantiate(prefabToSpawn, user.SpawnTransform);

        var projectile = spawnObject.GetComponent<Projectile>();
        if(projectile != null)
        {
            projectile.creator = heldItem.GetComponentInParent<Collider2D>();
            var userTargets = user.GetComponent<TargetTags>();
            var projTargets = projectile.GetComponent<TargetTags>();
            if (userTargets != null && projTargets != null)
                projTargets.SetTargets(userTargets.GetTargets());
        }

        var AttackItemListeners = spawnObject.GetComponent<HeldItem>();
        if (AttackItemListeners != null)
        {
            AttackItemListeners.Begin(user);
        }

        var HeldItemRef = user.GetComponent<HeldItemRef>();
        if (HeldItemRef != null)
            HeldItemRef.value = AttackItemListeners;

        spawnObject.transform.localScale = user.transform.localScale;

        if (angleSnap == 0)
            RotateObject(spawnObject.transform, user.InputController.lookDirection);
        else
            RotateObject(spawnObject.transform, user.InputController.lookDirection, angleSnap);

        spawnObject.transform.position = (Vector2)user.SpawnTransform.position + (Vector2)spawnObject.transform.right;

        //  apply item delay so you cant spam items
        user.ApplyItemDelay(itemCooldownTime);

        // apply movement stun
        user.ApplyMoveStun(itemMoveStunTime);

        if (heldItem != null)
        {
            var HeldItemListeners = heldItem.GetComponent<HeldItem>();
            if (HeldItemListeners != null)
            {
                HeldItemListeners.Clean(user);
            }

            Destroy(heldItem.gameObject);
            heldItem = null;
        }

    }

    protected virtual void RotateObject(Transform transformToRotate, Vector2 _direction)
    {
        //  gets the angle from the look direction
        float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;

        //  rotates object to face the new angle
        transformToRotate.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    protected virtual void RotateObject(Transform transformToRotate, Vector2 _direction, int _angleSnap)
    {
        //  gets the angle from the look direction
        float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        //  rotates object to face the new angle
        transformToRotate.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        var vec = transformToRotate.eulerAngles;
        vec.x = Mathf.Round(vec.x / _angleSnap) * _angleSnap;
        vec.y = Mathf.Round(vec.y / _angleSnap) * _angleSnap;
        vec.z = Mathf.Round(vec.z / _angleSnap) * _angleSnap;
        transformToRotate.eulerAngles = vec;
    }

}