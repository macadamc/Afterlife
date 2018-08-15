using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class TriggerZone : MonoBehaviour
{
    [System.Serializable]
    public class Events
    {
        public UnityEvent onTrigger;
        public UnityEvent onExit;
    }
    [Tooltip("List of tags that can trigger this object. If blank it will use any OnTriggerEnter calls it receives.")]
    public List<string> triggerTags = new List<string>();
    [Tooltip("List of tags that this trigger will ignore collisions with.")]
    public List<string> ignoreTags = new List<string>();
    public GameObject ignoreObject;
    public bool addPlayerTag = false;
    [DrawWithUnity]
    public Events events;

    private void OnEnable()
    {
        if (addPlayerTag && !triggerTags.Contains("Player"))
            triggerTags.Add("Player");
    }

    protected bool HasTag(GameObject go, List<string> tags)
    {
        bool _hasTag = false;

        foreach (string tag in tags)
        {
            if (tag == go.tag)
                _hasTag = true;
        }

        return _hasTag;
    }

    protected virtual bool ConditionsMet(Collider2D collision)
    {
        //if doesnt have trigger tags
        if (triggerTags.Count > 0)
        {
            if (HasTag(collision.gameObject, triggerTags) == false)
                return false;
        }

        //if has ignore tags
        if (ignoreTags.Count > 0)
        {
            if (HasTag(collision.gameObject, ignoreTags) == true)
                return false;
        }

        if (ignoreObject != null && collision.gameObject == ignoreObject)
            return false;

        return true;
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (ConditionsMet(collision))
        {
            OnEnter(collision);
            events.onTrigger?.Invoke();
        }
    }

    protected void OnTriggerStay2D(Collider2D collision)
    {
        if (ConditionsMet(collision))
        {
            OnStay(collision);
        }
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (ConditionsMet(collision))
        {
            OnExit(collision);
            events.onExit?.Invoke();
        }            
    }

    protected virtual void OnEnter(Collider2D collision)
    {

    }

    protected virtual void OnStay(Collider2D collision)
    {

    }

    protected virtual void OnExit(Collider2D collision)
    {

    }
}

