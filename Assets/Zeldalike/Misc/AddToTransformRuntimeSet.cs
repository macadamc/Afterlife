using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.RuntimeSets;

public class AddToTransformRuntimeSet : MonoBehaviour
{
    public TransformRuntimeSet[] transformRuntimeSets;

    private void OnEnable()
    {
        foreach(TransformRuntimeSet set in transformRuntimeSets)
        {
            set.Add(transform);
        }
        
    }

    private void OnDisable()
    {
        foreach (TransformRuntimeSet set in transformRuntimeSets)
        {
            set.Remove(transform);
        }
    }
}
