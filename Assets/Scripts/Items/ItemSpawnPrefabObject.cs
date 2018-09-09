using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/New Item")]
public class ItemSpawnPrefabObject : Item
{
    public GameObject itemToHold;
    public GameObject itemToSpawnOnUse;
    [Range(0.0f,1.0f)]
    public float moveSpeedPercent;
    public float itemCooldownTime;
    public float itemMoveStunTime;
    [Range(0,360)]
    public int angleSnap = 45;

    private GameObject heldItem;

    public override void Begin(ItemController user)
    {
        if (itemToHold != null)
        {
            if(user.MovementController!=null)
            {
                user.MovementController.oldMoveSpeed.Value = user.MovementController.moveSpeed.Value;
                user.MovementController.moveSpeed.Value = user.MovementController.moveSpeed.Value * moveSpeedPercent;
            }

            heldItem = Instantiate(itemToHold,user.transform);
            //CheckForDamageComponent(heldItem, user);
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
            if (angleSnap == 0)
                RotateObject(heldItem.transform, user.InputController.lookDirection);
            else
                RotateObject(heldItem.transform, user.InputController.lookDirection, angleSnap);
        }
    }

    public override void End(ItemController user)
    {
        if (heldItem != null)
        {
            Destroy(heldItem.gameObject);
            heldItem = null;
        }

        if (user.MovementController != null)
        {
            user.MovementController.moveSpeed.Value = user.MovementController.oldMoveSpeed.Value;
        }

        GameObject spawnObject = Instantiate(itemToSpawnOnUse, user.SpawnTransform);
        //CheckForDamageComponent(spawnObject, user);

        if (angleSnap == 0)
            RotateObject(spawnObject.transform, user.InputController.lookDirection);
        else
            RotateObject(spawnObject.transform, user.InputController.lookDirection, angleSnap);

        spawnObject.transform.position = (Vector2)user.SpawnTransform.position + (Vector2)spawnObject.transform.right;

        /*
        //  if object has a component that deals damage, make sure to set it so that it cant hurt itself.
        DealDamage dd = spawnObject.GetComponentInChildren<DealDamage>();
        if (dd != null)
            dd.ignoreObject = user.gameObject;
            */

        //  apply item delay so you cant spam items
        user.ApplyItemDelay(itemCooldownTime);

        // apply movement stun
        user.ApplyMoveStun(itemMoveStunTime);
    }

    protected virtual void RotateObject(Transform transformToRotate, Vector2 _direction)
    {
        //  gets the angle from the look direction
        float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;

        //  rotates object to face the new angle
        transformToRotate.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    protected virtual void RotateObject(Transform transformToRotate, Vector2 _direction, int _angleSnap)
    {
        //  gets the angle from the look direction
        float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        //  rotates object to face the new angle
        transformToRotate.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);

        var vec = transformToRotate.eulerAngles;
        vec.x = Mathf.Round(vec.x / _angleSnap) * _angleSnap;
        vec.y = Mathf.Round(vec.y / _angleSnap) * _angleSnap;
        vec.z = Mathf.Round(vec.z / _angleSnap) * _angleSnap;
        transformToRotate.eulerAngles = vec;
    }

    /*
    private void CheckForDamageComponent(GameObject objToCheck, ItemController user)
    {
        ChangeHealthOnTriggerEnter changeHealth = objToCheck.GetComponentInChildren<ChangeHealthOnTriggerEnter>();
        if (changeHealth != null)
        {
            changeHealth.ignoreObject = user.gameObject;
        }
    }
    */

}
