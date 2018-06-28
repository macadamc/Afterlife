using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ShadyPixel.Variables
{
    [CreateAssetMenu ( menuName = "ShadyPixel/Variables/New String Variable" )]
    public class StringVariable : ScriptableObjectVariable<string>
    {
        public void SetValue(StringVariable variable)
        {
            RuntimeValue = variable.GetValue();
        }

        public void AppendValue(string variable)
        {
            RuntimeValue += variable;
        }
        public void AppendValue(StringVariable variable)
        {
            RuntimeValue += variable.GetValue();
        }
        public void AppendValue(string change, string delimiter)
        {
            RuntimeValue += delimiter + change;
        }
        public void AppendValue(StringVariable variable, string delimiter)
        {
            RuntimeValue += delimiter + variable.GetValue();
        }
        public void AppendValue(string variable, StringVariable delimiter)
        {
            RuntimeValue += delimiter.GetValue() + variable;
        }
        public void AppendValue(StringVariable variable, StringVariable delimiter)
        {
            RuntimeValue += delimiter.GetValue() + variable.GetValue();
        }
    }
}
