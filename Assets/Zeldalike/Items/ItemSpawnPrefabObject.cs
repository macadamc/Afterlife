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

            //  spawns object and sets parent
            heldItem = Instantiate(itemToHold, user.SpawnTransform);

            CheckForDamageComponent(heldItem, user);

            //  sets transform of spawned object
            heldItem.transform.position = (Vector2)user.SpawnTransform.position;

            //  gets the angle from the look direction
            float angle = Mathf.Atan2(user.lookDirection.normalized.y, user.lookDirection.normalized.x) * Mathf.Rad2Deg;

            //  rotates object to face the new angle
            heldItem.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

    }

    public override void Hold(ItemController user)
    {
        if (itemToHold != null && heldItem != null)
        {

            //  gets the angle from the look direction
            float angle = Mathf.Atan2(user.lookDirection.normalized.y, user.lookDirection.normalized.x) * Mathf.Rad2Deg;

            //  rotates object to face the new angle
            heldItem.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
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

        //  spawns object and sets parent
        GameObject spawnObject = Instantiate(itemToSpawnOnUse, user.SpawnTransform);

        CheckForDamageComponent(spawnObject, user);

        //  sets transform of spawned object, user direction is used to place it on one of the entitys sides
        spawnObject.transform.position = (Vector2)user.SpawnTransform.position + user.lookDirection.normalized;


        //  gets the angle from the look direction
        float angle = Mathf.Atan2(user.lookDirection.normalized.y, user.lookDirection.normalized.x) * Mathf.Rad2Deg;

        //  rotates object to face the new angle
        spawnObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

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

    private void CheckForDamageComponent(GameObject objToCheck, ItemController user)
    {
        ChangeHealthOnTriggerEnter changeHealth = objToCheck.GetComponentInChildren<ChangeHealthOnTriggerEnter>();
        if (changeHealth != null)
        {
            changeHealth.ignoreObject = user.gameObject;
        }
    }

}
