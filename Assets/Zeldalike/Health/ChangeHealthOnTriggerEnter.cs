using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.Variables;
using ShadyPixel.Audio;

public class ChangeHealthOnTriggerEnter : MonoBehaviour {

    public IntReference change = new IntReference(-1);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Health healthComponenet = collision.gameObject.GetComponentInChildren<Health>();
        if(healthComponenet!=null)
        {
            healthComponenet.ChangeHealth(change.Value);
        }
    }
}
