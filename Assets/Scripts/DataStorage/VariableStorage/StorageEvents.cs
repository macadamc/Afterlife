using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

[System.Serializable]
public class VariableStorageEvent
{
    [TabGroup("LookUp")]
    public string key;
    [TabGroup("LookUp")]
    public bool keyLookupOnly;
    [HideIf("keyLookupOnly"), TabGroup("LookUp")]
    public GlobalStorageObject.VarType VarType;

    [DrawWithUnity, TabGroup("Events")]
    public UnityEvent OnAdd;
    [DrawWithUnity, TabGroup("Events")]
    public UnityEvent OnChange;
    [DrawWithUnity, TabGroup("Events")]
    public UnityEvent OnRemove;
}

public class StorageEvents : SerializedMonoBehaviour
{
    [CustomContextMenu("SceneOlnyStorage", "CreateSceneOlnyStorage")]
    public GlobalStorageObject storage;

    [ListDrawerSettings(DraggableItems = true, ShowPaging = true, NumberOfItemsPerPage = 1)]
    public VariableStorageEvent[] events;
    

    private void OnEnable()
    {
        storage.OnAdd += DoOnAdd;
        storage.OnChange += DoOnChange;
        storage.OnRemove += DoOnRemove;
    }
    private void OnDisable()
    {
        storage.OnAdd -= DoOnAdd;
        storage.OnChange -= DoOnChange;
        storage.OnRemove -= DoOnRemove;
    }

    VariableStorageEvent GetEvent(string key, GlobalStorageObject.VarType varType)
    {
        foreach (var iv in events)
        {
            if (iv.key == key)
            {
                if(iv.keyLookupOnly)
                    return iv;
                if (iv.VarType == varType)
                {
                    return iv;
                }
            }
        }
        return null;
    }

    public void DoOnAdd(string key, GlobalStorageObject.VarType type)
    {
        GetEvent(key, type)?.OnAdd.Invoke();
    }
    public void DoOnChange(string key, GlobalStorageObject.VarType type)
    {

        GetEvent(key, type)?.OnChange.Invoke();
    }
    public void DoOnRemove(string key, GlobalStorageObject.VarType type)
    {

        GetEvent(key, type)?.OnRemove.Invoke();
    }

    internal void CreateSceneOlnyStorage()
    {
        storage = ScriptableObject.CreateInstance<GlobalStorageObject>();
        storage.name = $"{gameObject.name}_TempStorage";
    }
}
