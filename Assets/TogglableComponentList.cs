using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class TogglableComponentList : MonoBehaviour {

    public bool activateOnEnableDisable;
    [ShowIf("activateOnEnableDisable")]
    public bool useToggle;
    [HideIf("useToggle")]
    public bool firstState;

    public List<MonoBehaviour> Items;

    private void OnEnable()
    {
        if (activateOnEnableDisable)
        {
            if (useToggle)
                Toggle();
            else
                SetEnabled(firstState);
        }
    }
    private void OnDisable()
    {
        if (activateOnEnableDisable)
        {
            if (useToggle)
                Toggle();
            else
                SetEnabled(!firstState);
        }
    }

    public void Toggle()
    {
        foreach(var item in Items)
        {
            item.enabled = !item.enabled;
        }
    }
    public void SetEnabled(bool state)
    {
        foreach (var item in Items)
        {
            item.enabled = state;
        }
    }
}
