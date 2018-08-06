using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.Singleton;
using UnityEngine.EventSystems;
using Pixelplacement;
using UnityEngine.UI;

public class InventoryManager : Singleton<InventoryManager>
{
    public ItemRuntimeSet playerInventory;
    public GameObject inventoryItemPrefab;
    public CanvasGroup inventoryCanvasGroup;
    public Transform inventoryItemHolder;
    public Text descriptionText;

    public delegate void OnEquipItem(Item item);
    public OnEquipItem onEquipItem;

    EventSystem eventSystem;
    List<InventoryItem> items = new List<InventoryItem>();
    Item currentItem;

    public void ShowInventory(bool state)
    {
        if (state == true)
        {
            CreateInventory();
            Tween.CanvasGroupAlpha(inventoryCanvasGroup, 0.0f, 1.0f, 0.5f, 0.0f, Tween.EaseInOut, Tween.LoopType.None, null, null, false);
        }
        else
        {
            DisableItems();
            Tween.CanvasGroupAlpha(inventoryCanvasGroup, 1.0f, 0.0f, 0.5f, 0.0f, Tween.EaseInOut, Tween.LoopType.None, null, null, false);
        }
    }

    public void ClearInventory()
    {
        foreach (Transform child in inventoryItemHolder)
        {
            Destroy(child.gameObject);
        }

    }

    public void DisableItems()
    {
        foreach(InventoryItem item in items)
        {
            item.enabled = false;
        }
        items.Clear();
    }

    public void CreateInventory()
    {
        ClearInventory();
        foreach (Item item in playerInventory.Items)
        {
            GameObject g = Instantiate(inventoryItemPrefab, inventoryItemHolder);

            InventoryItem ii = g.GetComponent<InventoryItem>();
            Image image = ii.GetComponent<Image>();
            ii.item = item;
            image.sprite = ii.item.aboveHeadSprite;
            items.Add( ii );

            if(currentItem == item)
            {
                eventSystem.SetSelectedGameObject(ii.gameObject);
            }
        }
    }

    public void Equip(Item item)
    {
        currentItem = item;

        if (onEquipItem != null)
            onEquipItem.Invoke(item);
    }

    public void LookAt(Item item)
    {
        descriptionText.text = item.description;
    }

    private void OnEnable()
    {
        Initialize(this);
        eventSystem = FindObjectOfType<EventSystem>();
        inventoryCanvasGroup.alpha = 0.0f;

    }
}
