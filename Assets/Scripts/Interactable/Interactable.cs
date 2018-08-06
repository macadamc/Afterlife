using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class Interactable : TriggerZone
{
    [System.Serializable]
    public class InteractableEvents
    {
        public UnityEvent onInteract;
    }
    [DrawWithUnity]
    public InteractableEvents ev;
    [HideInInspector]
    public bool triggered;
    

    protected override void OnStay(Collider2D collision)
    {
        if (SimpleInput.GetButtonDown("Interact") && triggered == false)
        {
            triggered = true;
            ev?.onInteract?.Invoke();
        }
    }

    protected override void OnExit(Collider2D collision)
    {
        triggered = false;
    }
}
