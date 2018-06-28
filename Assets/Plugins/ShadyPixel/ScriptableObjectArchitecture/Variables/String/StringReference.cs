using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ShadyPixel.Variables
{
    [Serializable]
    [InlineProperty]
    public class StringReference
    {
        public enum Type
        {
            Constant, Variable
        }

        [HideLabel]
        [HorizontalGroup]
        public Type type;

        [HideLabel]
        [HorizontalGroup]
        [ShowIf("type", Type.Constant)]
        public string ConstantValue;


        [HideLabel]
        [HorizontalGroup]
        [ShowIf("type", Type.Variable)]
        public StringVariable Variable;

        public StringReference()
        { }

        public StringReference(string value)
        {
            type = Type.Constant;
            ConstantValue = value;
        }

        public string Value
        {
            get { return type == Type.Constant ? ConstantValue : Variable.GetValue(); }
            set
            {
                if (type == Type.Constant)
                {
                    ConstantValue = value;
                }
                else
                {
                    Variable.SetValue(value);
                }
            }
        }


        public static implicit operator string(StringReference reference)
        {
            return reference.Value;
        }
    }
}