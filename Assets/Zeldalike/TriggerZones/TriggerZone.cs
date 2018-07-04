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

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        //if doesnt have trigger tags return out
        if (triggerTags.Count > 0)
        {
            if (HasTag(collision.gameObject, triggerTags) == false)
                return;
        }

        //if has ignore tags return out
        if (ignoreTags.Count > 0)
        {
            if (HasTag(collision.gameObject, ignoreTags) == true)
                return;
        }

        if (ignoreObject != null && collision.gameObject == ignoreObject)
            return;

        OnEnter(collision);

        if(events.onTrigger != null)
            events.onTrigger.Invoke();
    }

    protected void OnTriggerStay2D(Collider2D collision)
    {
        //if doesnt have trigger tags return out
        if (triggerTags.Count > 0)
        {
            if (HasTag(collision.gameObject, triggerTags) == false)
                return;
        }

        //if has ignore tags return out
        if (ignoreTags.Count > 0)
        {
            if (HasTag(collision.gameObject, ignoreTags) == true)
                return;
        }


        if (ignoreObject != null && collision.gameObject == ignoreObject)
            return;

        OnStay(collision);
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        //if doesnt have trigger tags return out
        if (triggerTags.Count > 0)
        {
            if (HasTag(collision.gameObject, triggerTags) == false)
                return;
        }

        //if has ignore tags return out
        if (ignoreTags.Count > 0)
        {
            if (HasTag(collision.gameObject, ignoreTags) == true)
                return;
        }

        if (ignoreObject != null && collision.gameObject == ignoreObject)
            return;

        OnExit(collision);

        if (events.onTrigger != null)
            events.onTrigger.Invoke();
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
