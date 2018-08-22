using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PersistGameObjectEnabledBool : MonoBehaviour, IDataPersister
{
    public DataSettings dataSettings;
    public bool initalState;

    DataSettings IDataPersister.GetDataSettings()
    {
        return dataSettings;
    }

    void IDataPersister.LoadData(Data data)
    {
        Data<bool> inventoryItemData = (Data<bool>)data;
        gameObject.SetActive(inventoryItemData.value);
    }

    Data IDataPersister.SaveData()
    {
        return new Data<bool>(gameObject.activeSelf);
    }

    void IDataPersister.SetDataSettings(string dataTag, DataSettings.PersistenceType persistenceType)
    {
        dataSettings.dataTag = dataTag;
        dataSettings.persistenceType = persistenceType;
    }

    private void Awake()
    {
        PersistentDataManager.RegisterPersister(this);
        gameObject.SetActive(initalState);
    }
    private void OnDestroy()
    {
        PersistentDataManager.UnregisterPersister(this);
    }
}
