using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public class SetPersistantVariable : MonoBehaviour {

    public string key;

    public enum ValueType { Bool, Float, String }
    public ValueType valueType;

    public GlobalStorageObject storage;

    [ShowIf("valueType", ValueType.Bool)]
    public bool b;
    [ShowIf("valueType", ValueType.Float)]
    public float f;
    [ShowIf("valueType", ValueType.String)]
    public string s;

    public void SetValue()
    {
        switch(valueType)
        {
            case ValueType.Bool:
                storage.SetValue(key, b);
                break;
            case ValueType.Float:
                storage.SetValue(key, f);
                break;
            case ValueType.String:
                storage.SetValue(key, s);
                break;
        }
    }
}
