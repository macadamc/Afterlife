using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SeawispHunter.MinibufferConsole;
using System.Linq;
using System;

public class ItemCompleter : ICompleter, ICoercer, ICache
{
    private Dictionary<string, UnityEngine.Object> _resources = null;
    protected Dictionary<string, UnityEngine.Object> resources 
    {
        get
        {
            if(_resources == null)
            {
                Debug.Log("Loading Items From Resourses");
                _resources = new Dictionary<string, UnityEngine.Object>();
                foreach (var resource in Resources.LoadAll<Item>(""))
                {
                    _resources[resource.name] = resource;
                }
            }
            return _resources;
        }

    }

    public Type defaultType
    {
        get
        {
            return typeof(Item);
        }
    }

    public object Coerce(string input, Type t)
    {
        if (!t.IsAssignableFrom(defaultType))
            throw new CoercionException(defaultType, t);

        if (resources.ContainsKey(input))
            return resources[input];
        else
            return null;
    }

    public IEnumerable<string> Complete(string input)
    {
        return Matcher.Match(input, resources.Select(x => x.Key));
    }

    public void ResetCache()
    {
        _resources = null;
    }
}
