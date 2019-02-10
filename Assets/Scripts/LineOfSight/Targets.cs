using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


[System.Serializable]
public class TargetConstraint
{
    public virtual bool Check(Transform target) { return false; }
}

[System.Serializable]
public class HealthGreaterThanX : TargetConstraint
{
    public float x;
    Health hp;
    public override bool Check(Transform target)
    {
        hp = target.GetComponent<Health>();

        return hp != null && hp.currentHealth > x;
    }
}

public class Targets : SerializedMonoBehaviour
{
    public List<Transform> transforms = new List<Transform>();

    [Sirenix.Serialization.OdinSerialize]
    public List<TargetConstraint> constraints;

    public void Add(Transform t)
    {
        if (!transforms.Contains(t) && CheckConstraints(t))
            transforms.Add(t);
    }

    public void Remove(Transform t)
    {
        if (transforms.Contains(t))
            transforms.Remove(t);
    }

    public bool CheckConstraints(Transform t)
    {
        bool flag = true;
        foreach (var constraint in constraints)
        {
            flag = constraint.Check(t);
            if (flag == false)
                break;
        }
        return flag;
    }
}
