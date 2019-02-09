using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.Variables;
using Sirenix.OdinInspector;

public class TargetTags : MonoBehaviour
{
    public List<string> overridableTargets;

    public List<string> nonOverridableTargets { get { return _nonOverridable; } protected set { _nonOverridable = value; } }
    public List<string> _nonOverridable;

    public List<string> GetTargets()
    {
        var ret = new List<string>();
        ret.AddRange(overridableTargets);
        ret.AddRange(nonOverridableTargets);
        return ret;
    }

    public void SetTargets(List<string> targets)
    {
        overridableTargets = new List<string>(targets);
    }
}
