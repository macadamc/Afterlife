using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

namespace ShadyPixel.Variables
{
    [CreateAssetMenu(menuName = "ShadyPixel/Variables/New Float Variable")]
    public class FloatVariable :  ScriptableObjectVariable<float>
    {

        public void SetValue(FloatVariable variable)
        {
            RuntimeValue = variable.GetValue();
        }

        public void ChangeValue(float change)
        {
            RuntimeValue += change;
        }

        public void ChangeValue(FloatVariable variable)
        {
            RuntimeValue += variable.GetValue();
        }
    }
}
