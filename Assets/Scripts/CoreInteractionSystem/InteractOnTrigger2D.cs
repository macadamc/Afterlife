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

    /// <summary>
    /// called from unity editor dropdown menu and when component is created.
    /// </summary>
    protected virtual void Reset()
    {
        layers = LayerMask.NameToLayer("Everything");
        m_Collider = GetComponent<Collider2D>();
        m_Collider.isTrigger = true;
    }

    /// <summary>
    /// check layers that other gameobject is on compared to layermask.
    /// </summary>
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

    /// <summary>
    /// when entering trigger and passes layermask check.
    /// </summary>
    /// <param name="other"></param>
    protected virtual void ExecuteOnEnter(Collider2D other)
    {
        OnEnter.Invoke();
    }

    /// <summary>
    /// when leaving trigger and passes layermask check
    /// </summary>
    /// <param name="other"></param>
    protected virtual void ExecuteOnExit(Collider2D other)
    {
        OnExit.Invoke();
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "InteractionTrigger", false);
    }
}