using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine.Events;
using ShadyPixel.Variables;

public class FloatCompare : CompareObject
{
    public FloatReference Other;

    public enum ComparisonType { Equals, GreaterThan, GreaterOrEqual, LessThan, LessOrEqual, NotEqual }
    public ComparisonType comparisonType;

    public override bool Check(GlobalStorageObject storageObject)
    {
        if (storageObject.floats.ContainsKey(key))
        {
            switch (comparisonType)
            {
                case ComparisonType.Equals:
                    return storageObject.GetFloat(key) == Other.Value;
                case ComparisonType.NotEqual:
                    return storageObject.GetFloat(key) != Other.Value;
                case ComparisonType.GreaterThan:
                    return storageObject.GetFloat(key) > Other.Value;
                case ComparisonType.LessThan:
                    return storageObject.GetFloat(key) < Other.Value;
                case ComparisonType.GreaterOrEqual:
                    return storageObject.GetFloat(key) >= Other.Value;
                case ComparisonType.LessOrEqual:
                    return storageObject.GetFloat(key) <= Other.Value;
            }
        }

        return false;
    }
}

public class BoolCompare : CompareObject
{
    public override bool Check(GlobalStorageObject storage)
    {
        return storage.bools.ContainsKey(key) && storage.bools[key];
    }
}

public abstract class CompareObject
{
    public string key;
    public virtual bool Check(GlobalStorageObject storage)
    {
        throw new System.NotImplementedException();
    }
}

public class VariableStorageChecker
{

    public VariableStorageChecker()
    {
        checks = new List<CompareObject>();
        events = new Events();
    }
    [OdinSerialize]
    public List<CompareObject> checks;

    //public string[] inventoryItems;

    [System.Serializable]
    public class Events
    {
        public Events()
        {
            OnHasItem = new UnityEvent();
            OnDoesNotHaveItem = new UnityEvent();
        }

        public UnityEvent OnHasItem, OnDoesNotHaveItem;
    }

    public Events events;


    public bool DoChecks(PersistentVariableStorage storage)
    {
        if (storage != null)
        {
            for (var i = 0; i < checks.Count; i++)
            {
                if (checks[i].Check(storage.storage) == false)
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

    // 2d gameKit Inventory.
    [System.Serializable]
    public class VariableStorageEvent
    {
        public string key;
        [DrawWithUnity]
        public UnityEvent OnAdd, OnChange, OnRemove;
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
