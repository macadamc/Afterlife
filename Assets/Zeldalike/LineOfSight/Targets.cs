using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targets : MonoBehaviour
{
    public List<Transform> transforms = new List<Transform>();

    public void Add(Transform transform)
    {
        if (!transforms.Contains(transform))
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

}
