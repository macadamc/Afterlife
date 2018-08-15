using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName ="ShadyPixel/Inventory")]
public class InventorySO : ScriptableObject
{
    public HashSet<string> m_InventoryItems = new HashSet<string>();

    public void Add(string key)
    {
        if (!m_InventoryItems.Contains(key))
            m_InventoryItems.Add(key);
    }
    public void Remove(string key)
    {
        if (m_InventoryItems.Contains(key))
            m_InventoryItems.Remove(key);
    }
    public bool Contains(string key)
    {
        return m_InventoryItems.Contains(key);
    }
    public void Clear()
    {
        m_InventoryItems.Clear();
    }
}

