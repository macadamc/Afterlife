using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace ShadyPixel.Events
{
    public class BasicUnityEvents : MonoBehaviour
    {
        [System.Serializable]
        public class BasicEvents
        {
            [Tooltip("Event that gets raised when Object is Enabled.")]
            public UnityEvent OnEnable;
            [Tooltip("Event that gets raised when Object is Disabled.")]
            public UnityEvent OnDisable;
        }

        [DrawWithUnity]
        public BasicEvents events;

        private void OnEnable()
        {
            if (events.OnEnable != null) events.OnEnable.Invoke();
        }

        private void OnDisable()
        {
            if (events.OnDisable != null) events.OnDisable.Invoke();
        }
    }
}
