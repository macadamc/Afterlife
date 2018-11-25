using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu]
public class GlobalStorageObject : SerializedScriptableObject
{
    public enum VarType { FLOAT, STRING, BOOL}
    public Dictionary<string, string> strings = new Dictionary<string, string>();
    public Dictionary<string, float> floats = new Dictionary<string, float>();
    public Dictionary<string, bool> bools = new Dictionary<string, bool>();

    public delegate void VariableStorageEvent(string key, VarType type);

    [System.NonSerialized]
    public VariableStorageEvent OnAdd;
    [System.NonSerialized]
    public VariableStorageEvent OnChange;
    [System.NonSerialized]
    public VariableStorageEvent OnRemove;

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
        string tKey = TrimStart(key);
        if (strings.ContainsKey(tKey) == false)
            SetValue(tKey, null);

        return strings[tKey];
    }
    public float GetFloat(string key)
    {
        string tKey = TrimStart(key);
        if (floats.ContainsKey(tKey) == false)
            SetValue(tKey, 0);

        return floats[tKey];
    }
    public bool GetBool(string key)
    {
        string tKey = TrimStart(key);

        if (bools.ContainsKey(tKey) == false)
            SetValue(tKey, false);

        return bools[tKey];
    }

    public void SetValue(string key, string value)
    {
        key = TrimStart(key);

        if (strings.ContainsKey(key) == false)
        {
            strings.Add(key, value);
            OnAdd?.Invoke(key, VarType.STRING);
        }
        else
        {
            if(value != strings[key])
            {
                strings[key] = value;
                OnChange?.Invoke(key, VarType.STRING);
            }
        }

    }
    public void SetValue(string key, float value)
    {
        key = TrimStart(key);
        if (floats.ContainsKey(key) == false)
        {
            floats.Add(key, value);
            OnAdd?.Invoke(key, VarType.FLOAT);
        }
        else
        {
            if(value != floats[key])
            {
                floats[key] = value;
                OnChange?.Invoke(key, VarType.FLOAT);
            }

        }

    }
    public void SetValue(string key, bool value)
    {
        key = TrimStart(key);
        if (bools.ContainsKey(key) == false)
        {
            bools.Add(key, value);
            OnAdd?.Invoke(key, VarType.BOOL);
        }
        else
        {            
            if(value != bools[key])
            {
                bools[key] = value;
                OnChange?.Invoke(key, VarType.BOOL);
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
        VarType varType = VarType.BOOL;

        if (strings.ContainsKey(key))
        {
            strings.Remove(key);
            varType = VarType.STRING;
        }
            
        if (floats.ContainsKey(key))
        {
            varType = VarType.FLOAT;
            floats.Remove(key);
        }
            
        if (bools.ContainsKey(key))
        {
            varType = VarType.BOOL;
            bools.Remove(key);
        }
            

        if (contains)
            OnRemove?.Invoke(key, varType);
        //GetInventoryEvent(key)?.OnRemove.Invoke();
    }

    public void Clear()
    {
        strings.Clear();
        floats.Clear();
        bools.Clear();
    }

    public void CopyValues(GlobalStorageObject other)
    {
        strings = new Dictionary<string, string>(other.strings);
        floats = new Dictionary<string, float>(other.floats);
        bools = new Dictionary<string, bool>(other.bools);
    }
}