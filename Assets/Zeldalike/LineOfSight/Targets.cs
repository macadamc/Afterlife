using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.RuntimeSets;

public class Targets : MonoBehaviour
{
    public List<Transform> transforms = new List<Transform>();
    public List<TransformRuntimeSet> enemys;

    public void Add(Transform transform)
    {
        if (!transforms.Contains(transform) && (enemys == null || enemysContains(transform)))
            transforms.Add(transform);
    }

    public void Remove(Transform transform)
    {
        if (transforms.Contains(transform))
            transforms.Remove(transform);
    }

    public bool Contains(Transform transform)
    {
        return transforms.Contains(transform);
    }

    private bool enemysContains(Transform t)
    {

        foreach(TransformRuntimeSet set in enemys)
        {
            if(set.Items.Contains(t))
            {
                return true;
            }
        }
        return false;
    }

}
