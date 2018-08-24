using System.Collections;
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
    public SpriteRenderer aboveHeadSpriteRenderer;
    public AudioSource inventoryAudioSource;

    private ItemController _itemController;

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
        item.pickupSfx.Play(inventoryAudioSource);
        //ItemController.MovementController.Stun(1.0f);
        yield return new WaitForSeconds(1f);
        aboveHeadSpriteRenderer.gameObject.SetActive(false);
        // if there is only one item, equip the item
        if (itemSet.Items.Count == 1)
            InventoryManager.Instance.Equip(item);
    }

    private void Start()
    {        
        itemSet.Items.Clear();

        if(aboveHeadSpriteRenderer!=null)
            aboveHeadSpriteRenderer.gameObject.SetActive(false);

        InventoryManager.Instance.onEquipItem += OnEquipItem;
    }
    private void OnEnable()
    {
        PersistentDataManager.RegisterPersister(this);
    }

    private void OnDisable()
    {
        InventoryManager.Instance.onEquipItem -= OnEquipItem;
        PersistentDataManager.UnregisterPersister(this);
    }

    private void OnEquipItem(Item item)
    {
        ItemController.EquipItem(item);
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
       foreach(string itemName in ((Data<List<string>>)data).value)
        {
            itemSet.Items.Add(Resources.Load<Item>(itemName));
            Debug.Log(itemName);

            /*
            itemSet.Items.Add(ScriptableObject.CreateInstance(
                types.Where(T => T.Name == itemName).FirstOrDefault().Name) as Item
                );

            */
        }

        if (itemSet.Items.Count > 0)
            OnEquipItem(itemSet.Items[0]);

        Debug.Log("Loaded");
    }

    public static List<Type> FindAllDerivedTypes<T>()
    {
        return FindAllDerivedTypes<T>(Assembly.GetAssembly(typeof(Item)));
    }

    public static List<Type> FindAllDerivedTypes<T>(Assembly assembly)
    {
        var derivedType = typeof(T);
        return assembly
            .GetTypes()
            .Where(t =>
                t != derivedType &&
                derivedType.IsAssignableFrom(t)
                ).ToList();

    }
}
