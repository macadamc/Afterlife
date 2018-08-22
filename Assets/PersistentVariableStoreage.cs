using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public class PersistentVariableStoreage : SerializedMonoBehaviour, IDataPersister
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

    //public YarnStorageWrapper yarnStorageWrapper;

    [CustomContextMenu("SceneOlnyStorage", "CreateSceneOlnyStorage")]
    public GlobalStorageObject storage;

    public DataSettings dataSettings;

    public VariableStorageEvent[] inventoryEvents;

    // 2d gameKit Inventory.
    [System.Serializable]
    public class VariableStorageEvent
    {
        public string key;
        [DrawWithUnity]
        public UnityEvent OnAdd, OnChange, OnRemove;
    }

    [System.Serializable]
    public class VariableStorageChecker
    {
        [System.Serializable]
        public class Events
        { public UnityEvent OnHasItem, OnDoesNotHaveItem; }


        public string[] inventoryItems;
        [DrawWithUnity]
        public Events events;

        public bool CheckInventory(PersistentVariableStoreage storage)
        {
            if (storage != null)
            {
                for (var i = 0; i < inventoryItems.Length; i++)
                {
                    if (!storage.storage.bools.ContainsKey(inventoryItems[i]) || storage.storage.bools[inventoryItems[i]] == false)
                    {
                        events.OnDoesNotHaveItem.Invoke();
                        return false;
                    }
                }
                events.OnHasItem.Invoke();
                return true;
            }
            return false;
        }
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

    public void DoOnAdd(string key)
    {
        GetInventoryEvent(key)?.OnAdd.Invoke();
    }
    public void DoOnChange(string key)
    {
        GetInventoryEvent(key)?.OnChange.Invoke();
    }
    public void DoOnRemove(string key)
    {
        GetInventoryEvent(key)?.OnRemove.Invoke();
    }

    internal void CreateSceneOlnyStorage()
    {
        storage = ScriptableObject.CreateInstance<GlobalStorageObject>();
        storage.name = $"{gameObject.name}_TempStorage";
    }

}

[System.Serializable]
public class YarnStorageWrapper : Yarn.VariableStorage
{
    public GlobalStorageObject storage;

    public YarnStorageWrapper(GlobalStorageObject storage)
    {
        this.storage = storage;
    }
    // Yarn Storage.
    public void Clear() => storage.Clear();

    public float GetNumber(string variableName)
    {
        return storage.GetFloat(variableName);
    }

    public void SetNumber(string variableName, float number) => storage.SetValue(variableName, number);

    public Value GetValue(string variableName)
    {
        return new Value(storage.GetFirstOrNull(variableName));
    }

    public void SetValue(string variableName, Value value)
    {
        switch (value.type)
        {
            case Value.Type.String:
                storage.SetValue(variableName, value.AsString);
                break;
            case Value.Type.Number:
                storage.SetValue(variableName, value.AsNumber);
                break;
            case Value.Type.Bool:
                storage.SetValue(variableName, value.AsBool);
                break;
            
        }
    }
}
