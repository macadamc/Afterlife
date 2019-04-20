using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

[ShowOdinSerializedPropertiesInInspector]
public class InteractOnTrigger2D : SerializedMonoBehaviour
{
    public UnityEvent OnEnter, OnExit;
    //public LayerMask layers;
    public bool addPlayerTag = true;
    //public List<string> interactableTags;

    public TargetTags targetTags;

    protected Collider2D m_Collider;
    protected bool CheckTags (Collider2D other)
    {
        if (targetTags == null)
            return true;
        var targetList = targetTags.GetTargets();

        if (targetList.Count == 0)
            return true;

        return targetList.Contains(other.tag);

    }

    private void Awake()
    {
        if (targetTags == null)
            targetTags = GetComponent<TargetTags>();

        if(targetTags != null)
        {
            var targetList = targetTags.GetTargets();

            if (addPlayerTag && targetList.Contains(Tags.Player) == false)
                targetTags.overridableTargets.Add(Tags.Player);
        }
       
    }

    /// <summary>
    /// called from unity editor dropdown menu and when component is created.
    /// </summary>
    protected virtual void Reset()
    {
        //layers = LayerMask.NameToLayer("Everything");
        m_Collider = GetComponent<Collider2D>();
        m_Collider.isTrigger = true;
    }

    /// <summary>
    /// check layers that other gameobject is on compared to layermask.
    /// </summary>
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (!enabled)
            return;

        if (CheckTags(other))
        {
            ExecuteOnEnter(other);
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if (!enabled)
            return;

        if (CheckTags(other))
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