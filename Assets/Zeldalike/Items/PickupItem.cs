using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PickupItem : TriggerZone
{
    public Item pickup;

    protected override void OnEnter(Collider2D collision)
    {
        Inventory inventory = collision.gameObject.GetComponent<Inventory>();

        if(inventory != null)
        {
            inventory.AddItem(pickup);
            Destroy(gameObject);
        }
    }
}