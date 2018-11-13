using System.Collections.Generic;
using Sirenix.Serialization;
using UnityEngine.Events;

public class VariableStorageChecker
{

    public VariableStorageChecker()
    {
        checks = new List<CompareObject>();
        events = new Events();
    }
    [OdinSerialize]
    public List<CompareObject> checks;

    //public string[] inventoryItems;

    [System.Serializable]
    public class Events
    {
        public Events()
        {
            OnHasItem = new UnityEvent();
            OnDoesNotHaveItem = new UnityEvent();
        }

        public UnityEvent OnHasItem, OnDoesNotHaveItem;
    }

    public Events events;


    public bool DoChecks(PersistentVariableStorage storage)
    {
        if (storage != null)
        {
            for (var i = 0; i < checks.Count; i++)
            {
                if (checks[i].Check(storage.storage) == false)
                {
                    events.OnDoesNotHaveItem.Invoke();
                    return false;
                }
            }
            events.OnHasItem.Invoke();
            return true;
        }
        return false;
    }

}
