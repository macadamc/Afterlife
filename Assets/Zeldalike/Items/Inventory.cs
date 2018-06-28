using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public ItemController ItemController
    {
        get
        {
            if (_itemController == null)
                _itemController = GetComponent<ItemController>();

            return _itemController;
        }
    }
    public List<Item> activeItems = new List<Item>();
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
        if (!activeItems.Contains(item))
        {
            activeItems.Add(item);

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
        if (activeItems.Contains(item))
            activeItems.Remove(item);
    }

    public IEnumerator GetItem(Item item)
    {
        aboveHeadSpriteRenderer.sprite = item.aboveHeadSprite;
        aboveHeadSpriteRenderer.gameObject.SetActive(true);
        item.pickupSfx.Play(inventoryAudioSource);
        ItemController.MovementController.Stun(1.0f);
        yield return new WaitForSecondsRealtime(1.0f);
        aboveHeadSpriteRenderer.gameObject.SetActive(false);
        // if there is only one item, equip the item
        if (activeItems.Count == 1)
            ItemController.EquipItem(activeItems[0]);
    }

    private void Start()
    {
        if(aboveHeadSpriteRenderer!=null)
            aboveHeadSpriteRenderer.gameObject.SetActive(false);
    }
}
