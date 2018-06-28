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
    [DrawWithUnity]
    public Events events;

    protected bool HasTag(GameObject go)
    {

        bool _hasTag = false;

        foreach (string tag in triggerTags)
        {
            if (tag == go.tag)
                _hasTag = true;
        }

        return _hasTag;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(triggerTags.Count > 0)
        {
            if (HasTag(collision.gameObject) == false)
                return;
        }

        if(events.onTrigger != null)
            events.onTrigger.Invoke();
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (triggerTags.Count > 0)
        {
            if (HasTag(collision.gameObject) == false)
                return;
        }

        if (events.onTrigger != null)
            events.onTrigger.Invoke();
    }
}
