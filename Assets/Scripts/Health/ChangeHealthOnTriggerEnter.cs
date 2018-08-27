using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.Variables;
using ShadyPixel.Audio;

public class ChangeHealthOnTriggerEnter : InteractOnTrigger2D {

    public IntReference change = new IntReference(-1);
    public bool onlyHitTargets = false;
    public Targets targets;
    protected override void ExecuteOnEnter(Collider2D other)
    {
        base.ExecuteOnEnter(other);

        Health self = GetComponentInParent<Health>();
        Health hp = other.gameObject.GetComponentInChildren<Health>();

        bool targetCheck = onlyHitTargets ? targets.Contains(other.transform) : true;

        if (hp != null && ((self != null && self != hp) || self == null) && targetCheck)
        {
            hp.ChangeHealth(change.Value);
        }

    }
}
