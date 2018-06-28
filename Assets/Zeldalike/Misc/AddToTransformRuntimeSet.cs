using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.RuntimeSets;

public class AddToTransformRuntimeSet : MonoBehaviour
{
    public TransformRuntimeSet transformRuntimeSet;

    private void OnEnable()
    {
        transformRuntimeSet.Add(transform);
    }

    private void OnDisable()
    {
        transformRuntimeSet.Remove(transform);
    }
}
