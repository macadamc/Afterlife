using System.Collections;
using System.Collections.Generic;
using ShadyPixel.Singleton;
using UnityEngine;
using Sirenix.OdinInspector;

public class CheckPointManager : Singleton<CheckPointManager>, IDataPersister
{
    [ShowInInspector]
    public Dictionary<string, string> checkPoints = new Dictionary<string, string>();
    public DataSettings dataSettings;

    void Awake()
    {
        Initialize(this);
        PersistentDataManager.RegisterPersister(this);
    }

    void OnDestroy()
    {
        PersistentDataManager.UnregisterPersister(this);
    }

    public DataSettings GetDataSettings()
    {
        return dataSettings;
    }

    public void SetDataSettings(string dataTag, DataSettings.PersistenceType persistenceType)
    {
        dataSettings = new DataSettings()
        {
            dataTag = dataTag,
            persistenceType = persistenceType
        };
    }

    public Data SaveData()
    {
        return new Data<Dictionary<string, string>>(checkPoints);
    }

    public void LoadData(Data data)
    {
        var loadedData = data as Data<Dictionary<string, string>>;
        checkPoints = loadedData.value;
    }
}
