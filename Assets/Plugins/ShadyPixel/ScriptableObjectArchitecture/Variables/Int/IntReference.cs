using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ShadyPixel.Variables
{
    [Serializable]
    [InlineProperty]
    public class IntReference
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
        public int ConstantValue;


        [HideLabel]
        [HorizontalGroup]
        [ShowIf("type", Type.Variable)]
        public IntVariable Variable;

        public IntReference()
        { }

        public IntReference(int value)
        {
            type = Type.Constant;
            ConstantValue = value;
        }

        public int Value
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


        public static implicit operator int(IntReference reference)
        {
            return reference.Value;
        }
    }
}