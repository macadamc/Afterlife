using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PersistentVariableStorage : SerializedMonoBehaviour, IDataPersister
{
    private void OnEnable()
    {
        PersistentDataManager.RegisterPersister(this);
    }
    private void OnDisable()
    {
        PersistentDataManager.UnregisterPersister(this);
    }
    private void OnDestroy()
    {
        PersistentDataManager.UnregisterPersister(this);
    }

    [CustomContextMenu("SceneOlnyStorage", "CreateSceneOlnyStorage")]
    public GlobalStorageObject storage;

    public DataSettings dataSettings;

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

    internal void CreateSceneOlnyStorage()
    {
        storage = ScriptableObject.CreateInstance<GlobalStorageObject>();
        storage.name = $"{gameObject.name}_TempStorage";
    }

    [Button]
    void AddStorageChecker()
    {
        var checker = gameObject.AddComponent<StorageCheckerBehaviour>();
        checker.storage = storage;
    }
    [Button]
    void AddStorageEvents()
    {
        var events = gameObject.AddComponent<StorageEvents>();
        events.storage = storage;
    }
}