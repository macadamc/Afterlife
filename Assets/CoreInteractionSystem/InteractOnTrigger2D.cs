using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

[RequireComponent(typeof(Collider2D))]
public class InteractOnTrigger2D : MonoBehaviour
{
    public UnityEvent OnEnter, OnExit;
    public LayerMask layers;
    public PersistentVariableStoreage.VariableStorageChecker[] inventoryChecks;

    protected Collider2D m_Collider;

    // called from unity editor dropdown menu and when component is created
    protected virtual void Reset()
    {
        layers = LayerMask.NameToLayer("Everything");
        m_Collider = GetComponent<Collider2D>();
        m_Collider.isTrigger = true;
    }

    // checks layer that other gameobject is on compared to layermask.
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

    // loops through inventory checks and calls function on them
    protected void DoInventoryChecks(Collider2D other)
    {
        for (int i = 0; i < inventoryChecks.Length; i++)
        {
            inventoryChecks[i].CheckInventory(other.GetComponentInChildren<PersistentVariableStoreage>());
        }
    }

    // when entering trigger and passes layermask check
    protected virtual void ExecuteOnEnter(Collider2D other)
    {
        OnEnter.Invoke();
        DoInventoryChecks(other);
    }

    // when leaving trigger and passes layermask check
    protected virtual void ExecuteOnExit(Collider2D other)
    {
        OnExit.Invoke();
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "InteractionTrigger", false);
    }
}
