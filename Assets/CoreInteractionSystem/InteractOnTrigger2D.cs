﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

[RequireComponent(typeof(Collider2D))]
public class InteractOnTrigger2D : MonoBehaviour
{
    [System.Serializable]
    public class Events
    {
        public UnityEvent OnEnter, OnExit;
    }

    public LayerMask layers;
    [DrawWithUnity]
    public Events events;
    public InventoryController.InventoryChecker[] inventoryChecks;

    protected Collider2D m_Collider;

    void Reset()
    {
        layers = LayerMask.NameToLayer("Everything");
        m_Collider = GetComponent<Collider2D>();
        m_Collider.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!enabled)
            return;

        if (layers.Contains(other.gameObject))
        {
            ExecuteOnEnter(other);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!enabled)
            return;

        if (layers.Contains(other.gameObject))
        {
            ExecuteOnExit(other);
        }
    }

    protected virtual void ExecuteOnEnter(Collider2D other)
    {
        events.OnEnter.Invoke();
        for (int i = 0; i < inventoryChecks.Length; i++)
        {
            inventoryChecks[i].CheckInventory(other.GetComponentInChildren<InventoryController>());
        }
    }

    protected virtual void ExecuteOnExit(Collider2D other)
    {
        events.OnExit.Invoke();
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "InteractionTrigger", false);
    }
}