using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.Variables;
using ShadyPixel.Audio;

public class ChangeHealthOnTriggerEnter : TriggerZone {

    public IntReference change = new IntReference(-1);

    protected override void OnEnter(Collider2D collision)
    {
        Health self = GetComponentInParent<Health>();
        Health healthComponenet = collision.gameObject.GetComponentInChildren<Health>();

        if(healthComponenet != null && ((self != null && self != healthComponenet) || self == null))
        {
            healthComponenet.ChangeHealth(change.Value);
        }
    }
}
