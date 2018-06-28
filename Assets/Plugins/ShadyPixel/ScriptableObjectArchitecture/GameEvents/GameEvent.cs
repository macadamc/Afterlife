using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace ShadyPixel.GameEvents
{
    [CreateAssetMenu( menuName = "ShadyPixel/Scriptable Game Event/New Game Event" )]
    public class GameEvent : ScriptableObject
    {
        public List<GameEventListener> listeners = new List<GameEventListener>();

        [Button("Force Listener Response", ButtonSizes.Large)]
        public void Raise()
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
                listeners[i].OnEventRaised();
        }

        public void AddListener(GameEventListener listener)
        {
            if (!listeners.Contains(listener))
                listeners.Add(listener);
        }

        public void RemoveListener(GameEventListener listener)
        {
            if (listeners.Contains(listener))
                listeners.Remove(listener);
        }
    }
}
