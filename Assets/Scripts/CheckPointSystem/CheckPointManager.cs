using System.Collections;
using System.Collections.Generic;
using ShadyPixel.Singleton;
using UnityEngine;
using Sirenix.OdinInspector;

public class CheckPointManager : Singleton<CheckPointManager>, IDataPersister
{
    public HashSet<string> checkPoints;
    public DataSettings dataSettings;

    void Awake()
    {
        Initialize(this);
        PersistentDataManager.RegisterPersister(this);

        if (checkPoints == null)
            checkPoints = new HashSet<string>();
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
        return new Data<HashSet<string>>(checkPoints);
    }

    public void LoadData(Data data)
    {
        var loadedData = data as Data<HashSet<string>>;
        checkPoints = loadedData.value;

        foreach(Checkpoint c in GameObject.FindObjectsOfType<Checkpoint>())
        {
            c.Init();
        }
    }
}