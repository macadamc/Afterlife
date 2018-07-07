using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoneEvent : MonoBehaviour
{
    public List<string> triggerKeys = new List<string>();
    public List<string> ignoreKeys = new List<string>();

    public void CheckTriggerEventKeys()
    {
        //Debug.Log(Mathf.Abs(GetInstanceID()).ToString());
        if (SaveLoadManager.Instance.tempVariables.HasKey(Mathf.Abs(GetInstanceID()).ToString()) || SaveLoadManager.Instance.savedVariables.HasKey(Mathf.Abs(GetInstanceID()).ToString()))
        {
            return;
        }

        foreach(string s in triggerKeys)
        {
            if(SaveLoadManager.Instance.tempVariables.HasKey(s) == false && SaveLoadManager.Instance.savedVariables.HasKey(s) == false)
            {
                return;
            }
        }

        foreach(string s in ignoreKeys)
        {
            if (SaveLoadManager.Instance.tempVariables.HasKey(s) || SaveLoadManager.Instance.savedVariables.HasKey(s))
            {
                return;
            }
        }

        OnTrigger();
    }

    public void TriggerExitEvent()
    {
        OnExit();
    }

    protected virtual void OnTrigger()
    {

    }

    protected virtual void OnExit()
    {

    }


}
