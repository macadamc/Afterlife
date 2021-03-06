﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class KeyItemPickup : InteractOnTrigger2D, IDataPersister
{
    public bool disableOnEnter = false;

    [HideInInspector]
    new public CircleCollider2D collider;

    public AudioClip clip;
    public DataSettings dataSettings;

    private void Awake()
    {
        PersistentDataManager.RegisterPersister(this);
    }

    void OnEnable()
    {
        collider = GetComponent<CircleCollider2D>();
    }

    private void OnDestroy()
    {
        PersistentDataManager.UnregisterPersister(this);
    }

    protected override void Reset()
    {
        //layers = LayerMask.NameToLayer("Everything");
        collider = GetComponent<CircleCollider2D>();
        collider.isTrigger = true;
        dataSettings = new DataSettings();
    }

    protected override void ExecuteOnEnter(Collider2D other)
    {
        if (disableOnEnter)
        {
            gameObject.SetActive(false);
            Save();
        }

        if (clip) AudioSource.PlayClipAtPoint(clip, transform.position);

        base.ExecuteOnEnter(other);
    }

    public void Save()
    {
        PersistentDataManager.SetDirty(this);
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

    public Data SaveData()
    {
        return new Data<bool>(gameObject.activeSelf);
    }

    public void LoadData(Data data)
    {
        if(data != null)
        {
            Data<bool> inventoryItemData = (Data<bool>)data;
            gameObject.SetActive(inventoryItemData.value);
        }
        
    }
}