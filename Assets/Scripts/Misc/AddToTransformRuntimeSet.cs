using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.RuntimeSets;

public class AddToTransformRuntimeSet : MonoBehaviour
{
    public TransformRuntimeSet transformRuntimeSet;

    private void OnEnable()
    {
        if(transformRuntimeSet!=null)
            transformRuntimeSet.Add(transform);
    }

    private void OnDisable()
    {
        if (transformRuntimeSet != null)
            transformRuntimeSet.Remove(transform);
    }
}
