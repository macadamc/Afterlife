using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ShadyPixel.Variables
{
    [Serializable]
    [InlineProperty]
    public class FloatReference
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
        [ShowIf("type",Type.Constant)]
        public float ConstantValue;


        [HideLabel]
        [HorizontalGroup]
        [ShowIf("type", Type.Variable)]
        public FloatVariable Variable;

        public FloatReference()
        { }

        public FloatReference(float value)
        {
            type = Type.Constant;
            ConstantValue = value;
        }

        public float Value
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


        public static implicit operator float(FloatReference reference)
        {
            return reference.Value;
        }
    }
}