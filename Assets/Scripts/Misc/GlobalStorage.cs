using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.Singleton;

public class GlobalStorage : Singleton<GlobalStorage>, IDataPersister
{
    public GlobalStorageObject storage;

    void Awake()
    {
        Initialize(this);
        PersistentDataManager.RegisterPersister(this);
    }

    void OnDestroy()
    {
        PersistentDataManager.UnregisterPersister(this);
    }

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
}
