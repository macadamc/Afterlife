using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


public class ActiveItemPickup : KeyItemPickup
{
    public Item itemToAdd;

    protected override void ExecuteOnEnter(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player"))
            return;

        Inventory inventory = other.gameObject.GetComponent<Inventory>();
        if(inventory != null)
        {
            inventory.AddItem(itemToAdd);
            base.ExecuteOnEnter(other);
        }
    }

}
