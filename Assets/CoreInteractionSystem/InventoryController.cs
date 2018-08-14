﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class InventoryController : MonoBehaviour, IDataPersister
{
    [System.Serializable]
    public class InventoryEvent
    {
        [System.Serializable]
        public class Events
        {
            public UnityEvent OnAdd, OnRemove;
        }

        public string key;
        [DrawWithUnity]
        public Events events;
    }


    [System.Serializable]
    public class InventoryChecker
    {
        [System.Serializable]
        public class Events
        { public UnityEvent OnHasItem, OnDoesNotHaveItem; }


        public string[] inventoryItems;
        [DrawWithUnity]
        public Events events;

        public bool CheckInventory(InventoryController inventory)
        {
            if (inventory != null)
            {
                for (var i = 0; i < inventoryItems.Length; i++)
                {
                    if (!inventory.HasItem(inventoryItems[i]))
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


    public InventoryEvent[] inventoryEvents;
    public event System.Action OnInventoryLoaded;

    public DataSettings dataSettings;

    HashSet<string> m_InventoryItems = new HashSet<string>();


    //Debug function useful in editor during play mode to print in console all objects in that InventoyController
    [ContextMenu("Dump")]
    void Dump()
    {
        foreach (var item in m_InventoryItems)
        {
            Debug.Log(item);
        }
    }

    void OnEnable()
    {
        PersistentDataManager.RegisterPersister(this);
    }

    void OnDisable()
    {
        PersistentDataManager.UnregisterPersister(this);
    }

    public void AddItem(string key)
    {
        if (!m_InventoryItems.Contains(key))
        {
            m_InventoryItems.Add(key);
            var ev = GetInventoryEvent(key);
            if (ev != null) ev.events.OnAdd.Invoke();
        }
    }

    public void RemoveItem(string key)
    {
        if (m_InventoryItems.Contains(key))
        {
            var ev = GetInventoryEvent(key);
            if (ev != null) ev.events.OnRemove.Invoke();
            m_InventoryItems.Remove(key);
        }
    }

    public bool HasItem(string key)
    {
        return m_InventoryItems.Contains(key);
    }

    public void Clear()
    {
        m_InventoryItems.Clear();
    }

    InventoryEvent GetInventoryEvent(string key)
    {
        foreach (var iv in inventoryEvents)
        {
            if (iv.key == key) return iv;
        }
        return null;
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
        return new Data<HashSet<string>>(m_InventoryItems);
    }

    public void LoadData(Data data)
    {
        Data<HashSet<string>> inventoryData = (Data<HashSet<string>>)data;
        foreach (var i in inventoryData.value)
            AddItem(i);
        if (OnInventoryLoaded != null) OnInventoryLoaded();
    }
}
