using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PickupItem : MonoBehaviour
{
    public Item pickup;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Inventory inventory = collision.gameObject.GetComponent<Inventory>();

        if(inventory != null)
        {
            inventory.AddItem(pickup);
            Destroy(gameObject);
        }
    }
}