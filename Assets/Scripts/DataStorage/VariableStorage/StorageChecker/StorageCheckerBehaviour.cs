using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Events;

public class StorageCheckerBehaviour : SerializedMonoBehaviour
{
    /// <summary>
    /// run when its gameobject is Enabled
    /// </summary>
    public bool CheckOnEnabled;

    private bool triggered;

    /// <summary>
    ///  storage object that the checker makes comparisons to.
    /// </summary>
    public GlobalStorageObject storage;

    public VariableStorageChecker[] Checkers;

    [Button]
    public void DoChecks() => DoChecks(storage);
    public void DoChecks(GlobalStorageObject storage)
    {
        if (storage == null || Checkers == null || Checkers.Length == 0)
            return;

        for (int i = 0; i < Checkers.Length; i++)
        {
            Checkers[i].DoChecks(storage);
        }
    }

    public void Reset()
    {
        Checkers = new VariableStorageChecker[0];
    }

    void Update()
    {
        if (!triggered && CheckOnEnabled)
        {
            triggered = true;
            DoChecks(storage);
        }
    }

    private void OnDisable()
    {
        triggered = false;
    }
}
