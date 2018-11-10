using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

[RequireComponent(typeof(Collider2D)), ShowOdinSerializedPropertiesInInspector]
public class InteractOnTrigger2D : SerializedMonoBehaviour
{
    public UnityEvent OnEnter, OnExit;
    public LayerMask layers;
    public bool addPlayerTag = true;
    public List<string> interactableTags;

    
    public VariableStorageChecker[] inventoryChecks;

    protected Collider2D m_Collider;
    protected bool CheckTags (Collider2D other)
    {
        if (interactableTags.Count == 0)
            return true;

        return interactableTags.Contains(other.tag);

    }

    private void Awake()
    {
        if (addPlayerTag && interactableTags.Contains("Player") == false)
            interactableTags.Add("Player");
    }

    // called from unity editor dropdown menu and when component is created
    protected virtual void Reset()
    {
        layers = LayerMask.NameToLayer("Everything");
        m_Collider = GetComponent<Collider2D>();
        m_Collider.isTrigger = true;
        inventoryChecks = new VariableStorageChecker[0];
    }

    // checks layer that other gameobject is on compared to layermask.
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!enabled)
            return;

        if (layers.Contains(other.gameObject) && CheckTags(other))
        {
            ExecuteOnEnter(other);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!enabled)
            return;

        if (layers.Contains(other.gameObject) && CheckTags(other))
        {
            ExecuteOnExit(other);
        }
    }

    // loops through inventory checks and calls function on them
    protected void DoInventoryChecks(Collider2D other)
    {
        PersistentVariableStorage store = other.GetComponentInChildren<PersistentVariableStorage>();

        if (store == null || inventoryChecks == null || inventoryChecks.Length == 0)
            return;

        for (int i = 0; i < inventoryChecks.Length; i++)
        {
            inventoryChecks[i].DoChecks(store);
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
