using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine.Events;
using ShadyPixel.Variables;

public class PersistentVariableStorage : SerializedMonoBehaviour, IDataPersister
{
    private void OnEnable()
    {
        storage.OnAdd += DoOnAdd;
        storage.OnChange += DoOnChange;
        storage.OnRemove += DoOnRemove;
        PersistentDataManager.RegisterPersister(this);
    }
    private void OnDisable()
    {
        storage.OnAdd -= DoOnAdd;
        storage.OnChange -= DoOnChange;
        storage.OnRemove -= DoOnRemove;
        PersistentDataManager.UnregisterPersister(this);
    }
    private void OnDestroy()
    {
        storage.OnAdd -= DoOnAdd;
        storage.OnChange -= DoOnChange;
        storage.OnRemove -= DoOnRemove;
        PersistentDataManager.UnregisterPersister(this);
    }

    [CustomContextMenu("SceneOlnyStorage", "CreateSceneOlnyStorage")]
    public GlobalStorageObject storage;

    public DataSettings dataSettings;

    public VariableStorageEvent[] inventoryEvents;

    [System.Serializable]
    public class VariableStorageEvent
    {
        public string key;
        [DrawWithUnity]
        public UnityEvent OnAdd;
        [DrawWithUnity]
        public UnityEvent OnChange;
        [DrawWithUnity]
        public UnityEvent OnRemove;
    }

    void IDataPersister.LoadData(Data data)
    {
        Data<Dictionary<string, string>, Dictionary<string, float>, Dictionary<string, bool>> loadedData = (Data<Dictionary<string, string>, Dictionary<string, float>, Dictionary<string, bool>>)data;
        storage.strings = loadedData.value0;
        storage.floats = loadedData.value1;
        storage.bools = loadedData.value2;
    }

    Data IDataPersister.SaveData()
    {
        return new Data<Dictionary<string, string>, Dictionary<string, float>, Dictionary<string, bool>>(storage.strings, storage.floats, storage.bools);
    }

    public DataSettings GetDataSettings()
    {
        return dataSettings;
    }

    public void SetDataSettings(string dataTag, DataSettings.PersistenceType persistenceType)
    {
        dataSettings.dataTag = dataTag;
        dataSettings.persistenceType = persistenceType;
    }

    VariableStorageEvent GetInventoryEvent(string key)
    {
        foreach (var iv in inventoryEvents)
        {
            if (iv.key == key) return iv;
        }
        return null;
    }

    public void DoOnAdd(string key, GlobalStorageObject.VarType type)
    {
        GetInventoryEvent(key)?.OnAdd.Invoke();
    }
    public void DoOnChange(string key, GlobalStorageObject.VarType type)
    {
        GetInventoryEvent(key)?.OnChange.Invoke();
    }
    public void DoOnRemove(string key, GlobalStorageObject.VarType type)
    {
        GetInventoryEvent(key)?.OnRemove.Invoke();
    }

    internal void CreateSceneOlnyStorage()
    {
        storage = ScriptableObject.CreateInstance<GlobalStorageObject>();
        storage.name = $"{gameObject.name}_TempStorage";
    }
}


