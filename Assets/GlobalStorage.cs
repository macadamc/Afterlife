using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.Singleton;

public class GlobalStorage : Singleton<GlobalStorage>
{
    public GlobalStorageObject storage;

    void OnEnable()
    {
        Initialize(this);
    }
}
