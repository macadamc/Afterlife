using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class TriggerVolume : MonoBehaviour
{
    public enum TriggerOn { Enter, Exit}
    public TriggerOn triggerOn;

    [ReadOnly]
    public bool state;

    [DrawWithUnity]
    public UnityEvent actions;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(state == false)
        {
            state = true;
        }
        else
        {
            return;
        }

        if (triggerOn == TriggerOn.Enter)
        {
            actions.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (state == true)
        {
            state = false;
        }
        else
        {
            return;
        }

        if (triggerOn == TriggerOn.Exit)
        {
            actions.Invoke();
        }
    }
}
