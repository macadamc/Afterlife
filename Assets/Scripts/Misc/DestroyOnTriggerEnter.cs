using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnTriggerEnter : TriggerZone
{
    protected override void OnEnter(Collider2D collision)
    {
        Destroy(gameObject);
    }
}
