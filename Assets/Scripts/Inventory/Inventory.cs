﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.RuntimeSets;
using ShadyPixel;
using System.Linq;
using System;
using System.Reflection;

public class Inventory : MonoBehaviour, IDataPersister
{
    public DataSettings dataSettings;

    static List<Type> types;

    public ItemController ItemController
    {
        get
        {
            if (_itemController == null)
                _itemController = GetComponent<ItemController>();

            return _itemController;
        }
    }
    //public List<Item> activeItems = new List<Item>();
    public ItemRuntimeSet itemSet;
    //public SpriteRenderer aboveHeadSpriteRenderer;

    private ItemController _itemController;

    /*
    /// <summary>
    /// Add Item to activeItem list
    /// </summary>
    /// <param name="item"></param>
    public void AddItem(Item item)
    {
    // if user does not already have item in iventory, add it
    if (!itemSet.Items.Contains(item))
    {
        itemSet.Add(item);

        StartCoroutine(GetItem(item));
    }
    }
    /// <summary>
    /// Remove Item from activeItem list
    /// </summary>
    /// <param name="item"></param>
    public void RemoveItem(Item item)
    {
    // if user has item in inventory, remove it
    if (itemSet.Items.Contains(item))
        itemSet.Remove(item);
    }

    public IEnumerator GetItem(Item item)
    {
    aboveHeadSpriteRenderer.sprite = item.aboveHeadSprite;
    aboveHeadSpriteRenderer.gameObject.SetActive(true);
    //ItemController.MovementController.Stun(1.0f);
    yield return new WaitForSeconds(1f);
    aboveHeadSpriteRenderer.gameObject.SetActive(false);
    // if there is only one item, equip the item
    if (itemSet.Items.Count == 1)
        InventoryManager.Instance.Equip(item);
    }
    */

    private void Start()
    {        
        itemSet.Items.Clear();
        /*
        if(aboveHeadSpriteRenderer!=null)
            aboveHeadSpriteRenderer.gameObject.SetActive(false);
        */

        InventoryManager.Instance.onEquipItem += OnEquipItem;
    }
    private void OnEnable()
    {
        PersistentDataManager.RegisterPersister(this);
        itemSet.onListChange += OnItemListChange;
    }

    private void OnDisable()
    {
        InventoryManager.Instance.onEquipItem -= OnEquipItem;
        PersistentDataManager.UnregisterPersister(this);

        itemSet.onListChange -= OnItemListChange;
    }

    private void OnEquipItem(Item item)
    {
        ItemController.EquipItem(item);
        for (int i = 0; i < itemSet.Items.Count; i++)
        {
            if(item.name == itemSet.Items[i].name)
            {
                ItemController.itemIndex = i;
            }
        }
    }

    private void OnItemListChange()
    {
        if (itemSet.Items.Count == 1)
            InventoryManager.Instance.Equip(itemSet.Items[0]);
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
        return new Data<List<string>>(itemSet.Items.Select(N => N.name).ToList());
    }

    public void LoadData(Data data)
    {
        itemSet.Items.Clear();
       foreach(string itemName in ((Data<List<string>>)data).value)
        {
            itemSet.Items.Add(Resources.Load<Item>(itemName));
        }

        if (itemSet.Items.Count > 0)
            OnEquipItem(itemSet.Items[0]);
    }
}
