using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targets : MonoBehaviour
{
    public List<Transform> transforms = new List<Transform>();

    public void Add(Transform t)
    {
        if (!transforms.Contains(t))
            transforms.Add(t);
    }

    public void Remove(Transform t)
    {
        if (transforms.Contains(t))
            transforms.Remove(t);
    }
}
