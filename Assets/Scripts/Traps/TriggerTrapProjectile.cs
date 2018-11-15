using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class TriggerTrapProjectile : MonoBehaviour
{
    public bool overrideSpikeTimmings;
    [ShowIf("overrideSpikeTimmings")]
    public float initalDelay, delayBeforeUp, delayBeforeDown;

    [ShowIf("overrideSpikeTimmings")]
    public bool overrideDownState;
    [ShowIf("overrideDownState"), ShowInInspector]
    public bool hold = true;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        SpikeTrap spike = collision.GetComponent<SpikeTrap>();

        if (spike == null || gameObject.activeSelf == false)
            return;

        if (overrideSpikeTimmings)
        {
            spike.initalDelay = initalDelay;
            spike.delayBeforeUp = delayBeforeUp;
            spike.delayBeforeDown = delayBeforeDown;
            if (overrideDownState)
            {
                spike.overrideDownState = true;
                spike.hold = hold;
            }
                
        }

        spike.Trigger();
    }
}
