using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ShadyPixel.Variables
{
    [CreateAssetMenu ( menuName = "ShadyPixel/Variables/New Int Variable" )]
    public class IntVariable : ScriptableObjectVariable<int>
    {
        public void SetValue(IntVariable variable)
        {
            RuntimeValue = variable.GetValue();
        }

        public void ChangeValue(int change)
        {
            RuntimeValue += change;
        }
        public void ChangeValue(IntVariable variable)
        {
            RuntimeValue += variable.GetValue();
        }
    }
}
