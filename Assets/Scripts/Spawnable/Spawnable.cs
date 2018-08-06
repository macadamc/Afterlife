using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn;

public class Spawnable : MonoBehaviour
{
    public SpawnGameObject spawner;
    public bool isLocal;
    public bool persistant;

    public void SetVar(bool state)
    {
        if (isLocal)
            SetLocalVar(state);

        if (persistant)
            SetSavedVar(state);

        //Debug.Log(Mathf.Abs( spawner.GetInstanceID()).ToString());
    }


    private void SetLocalVar(bool state)
    {
        SaveLoadManager.Instance.tempVariables.SetValue(spawner.guid, new Value(state));
    }

    private void SetSavedVar(bool state)
    {
        SaveLoadManager.Instance.savedVariables.SetValue(spawner.guid, new Value(state));
        //SaveLoadManager.Instance.savedVariables.Save();
    }
}
