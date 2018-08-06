using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class InventoryItem : Selectable, IEventSystemHandler
{
    public Item item;
    public GameObject selectedObject;

    [System.Serializable]
    public class Events
    {
        public UnityEvent onSelect;
        public UnityEvent onSubmit;
    }

    [DrawWithUnity]
    public Events events;

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        events.onSelect.Invoke();
        Log("called on select unity event");
        InventoryManager.Instance.LookAt(item);
        InventoryManager.Instance.Equip(item);
        selectedObject.SetActive(true);
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        //base.OnDeselect(eventData);
        selectedObject.SetActive(false);
    }

    /*
    public void OnSubmit(BaseEventData eventData)
    {
        events.onSubmit.Invoke();
        Log("called on submit unity event");
        InventoryManager.Instance.Equip(item);
    }
    */

    public void Log(string message)
    {
        Debug.Log(message);
    }
}
