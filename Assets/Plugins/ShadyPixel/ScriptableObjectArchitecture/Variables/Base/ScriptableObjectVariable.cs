using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace ShadyPixel.Variables
{
    public class ScriptableObjectVariable<T> : ScriptableObject, ISerializationCallbackReceiver
    {
        public T InitialValue;
        [ReadOnly]
        public T RuntimeValue;

        [Button("Reset Value to Default", ButtonSizes.Large)]
        public void OnAfterDeserialize()
        {
            RuntimeValue = InitialValue;
        }

        public void OnBeforeSerialize() { }

        public T GetValue()
        {
            return RuntimeValue;
        }

        public void SetValue(T value)
        {
            RuntimeValue = value;
        }
    }
}

