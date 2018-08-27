using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class PersistGameObjectEnabledBool : MonoBehaviour, IDataPersister
{
    public DataSettings dataSettings;

    public List<GameObject>objectsToPersist;

    DataSettings IDataPersister.GetDataSettings()
    {
        return dataSettings;
    }

    void IDataPersister.LoadData(Data data)
    {
        var loadedData = (Data<List<bool>>)data;

        for (int i = 0;i < objectsToPersist.Count; i++)
        {
            objectsToPersist[i].SetActive(loadedData.value[i]);
        }
    }

    Data IDataPersister.SaveData()
    {
        List<bool> saveData = new List<bool>();

        foreach(GameObject go in objectsToPersist)
        {
            saveData.Add(go.activeSelf);
        }

        return new Data<List<bool>>(saveData);
    }

    void IDataPersister.SetDataSettings(string dataTag, DataSettings.PersistenceType persistenceType)
    {
        dataSettings.dataTag = dataTag;
        dataSettings.persistenceType = persistenceType;
    }

    private void Awake()
    {
        PersistentDataManager.RegisterPersister(this);
    }

    private void OnEnable()
    {
        PersistentDataManager.RegisterPersister(this);
    }

    private void OnDestroy()
    {
        PersistentDataManager.UnregisterPersister(this);
    }
}