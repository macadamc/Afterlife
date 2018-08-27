using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractOnInteractButton2D : InteractOnTrigger2D
{
    /*
    Calls DoInventoryChecks() on input button press instead of OnTriggerEnter()
    */
    public UnityEvent OnButtonPress;

    bool m_InZone;
    InputController m_InputController;
    Collider2D m_InteractingCollider;

    // gets ref and sets m_CanExecuteButtons to true
    protected override void ExecuteOnEnter(Collider2D other)
    {
        m_InZone = true;
        m_InputController = other.gameObject.GetComponent<InputController>();
        m_InteractingCollider = other;
        OnEnter.Invoke();
        Debug.Log("[" + other.gameObject.name + "] has entered [" + name + "]");
    }

    // cleans up and sets m_CanExecuteButtons to false
    protected override void ExecuteOnExit(Collider2D other)
    {
        m_InZone = false;
        m_InputController = null;
        m_InteractingCollider = null;
        OnExit.Invoke();
        Debug.Log("[" + other.gameObject.name + "] has left [" + name + "]");
    }

    // waits for input from m_InputController and checks m_CanExecuteButtons
    void Update()
    {
        if(m_InZone)
        {
            if (m_InputController != null && SimpleInput.GetButtonDown("Interact")) //m_InputController.input.pressed)
            {
                OnInteractButtonPress();
            }
        }
    }

    protected virtual void OnInteractButtonPress()
    {
        OnButtonPress.Invoke();
        DoInventoryChecks(m_InteractingCollider);
        Debug.Log("[" + name + "] Triggered by [" + m_InteractingCollider.gameObject.name + "]");
    }
}
