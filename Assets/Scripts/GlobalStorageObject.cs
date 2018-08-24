﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn;
using Sirenix.OdinInspector;

[CreateAssetMenu]
public class GlobalStorageObject : SerializedScriptableObject
{
    public Dictionary<string, string> strings = new Dictionary<string, string>();
    public Dictionary<string, float> floats = new Dictionary<string, float>();
    public Dictionary<string, bool> bools = new Dictionary<string, bool>();

    public delegate void VariableStorageEvent(string key);

    [HideInInspector]
    public VariableStorageEvent OnAdd, OnChange, OnRemove;

    public bool Contains(string key)
    {
        return strings.ContainsKey(TrimStart(key)) || floats.ContainsKey(TrimStart(key)) || bools.ContainsKey(TrimStart(key));
    }
    public object GetFirstOrNull(string key)
    {
        key = TrimStart(key);

        if (strings.ContainsKey(key))
            return strings[key];
        if (floats.ContainsKey(key))
            return floats[key];
        if (bools.ContainsKey(key))
            return bools[key];
        return null;
    }

    protected string TrimStart(string key)
    {
        return key.TrimStart('$');
    }
    public string GetString(string key)
    {
        return strings[TrimStart(key)];
    }
    public float GetFloat(string key)
    {
        return floats[TrimStart(key)];
    }
    public bool GetBool(string key)
    {
        return bools[TrimStart(key)];
    }

    public void SetValue(string key, string value)
    {
        key = TrimStart(key);

        if (strings.ContainsKey(key) == false)
        {
            strings.Add(key, value);
            OnAdd?.Invoke(key);
        }
        else
        {
            if(value != strings[key])
            {
                strings[key] = value;
                OnChange?.Invoke(key);
            }
        }

    }
    public void SetValue(string key, float value)
    {
        key = TrimStart(key);
        if (floats.ContainsKey(key) == false)
        {
            floats.Add(key, value);
            OnAdd?.Invoke(key);
        }
        else
        {
            if(value != floats[key])
            {
                floats[key] = value;
                OnChange?.Invoke(key);
            }

        }

    }
    public void SetValue(string key, bool value)
    {
        key = TrimStart(key);
        if (bools.ContainsKey(key) == false)
        {
            bools.Add(key, value);
            OnAdd?.Invoke(key);
        }
        else
        {            
            if(value != bools[key])
            {
                bools[key] = value;
                OnChange?.Invoke(key);
            }
                
        }
    }

    public void SetBool(string key)
    {
        SetValue(TrimStart(key), true);
    }
    public void ToggleBool(string key)
    {
        key = TrimStart(key);
        if (Contains(key) == false)
            return;
        SetValue(key, !GetBool(key));
    }

    public void ChangeNumber(string key, float amount)
    {
        key = TrimStart(key);

        if (floats.ContainsKey(key))
            amount += GetFloat(key);

        SetValue(key, amount);
    }
    public void AddOneToFloat(string key)
    {
        ChangeNumber(key, 1);
    }
    public void SubOneFromFloat(string key)
    {
        ChangeNumber(key, -1);
    }

    public void RemoveValue(string key)
    {
        key = TrimStart(key);
        bool contains = Contains(key);

        if (strings.ContainsKey(key))
            strings.Remove(key);
        if (floats.ContainsKey(key))
            floats.Remove(key);
        if (bools.ContainsKey(key))
            bools.Remove(key);

        if (contains)
            OnRemove?.Invoke(key);
        //GetInventoryEvent(key)?.OnRemove.Invoke();
    }

    public void Clear()
    {
        strings.Clear();
        floats.Clear();
        bools.Clear();
    }
}