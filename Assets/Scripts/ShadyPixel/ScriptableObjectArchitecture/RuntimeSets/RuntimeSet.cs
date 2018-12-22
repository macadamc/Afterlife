using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace ShadyPixel.RuntimeSets
{
    public abstract class RuntimeSet<T> : ScriptableObject
    {
        [DrawWithUnity]
        public List<T> Items = new List<T>();

        public delegate void OnListChange();
        public OnListChange onListChange;

        public void Add(T thing)
        {
            if (!Items.Contains(thing))
                Items.Add(thing);

            if (onListChange != null)
                onListChange.Invoke();
        }

        public void Remove(T thing)
        {
            if (Items.Contains(thing))
                Items.Remove(thing);

            if (onListChange != null)
                onListChange.Invoke();
        }
    }
}
