using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : InteractOnInteractButton2D
{
    public bool executing;

    protected override void OnInteractButtonPress()
    {
        if(!executing)
        {
            base.OnInteractButtonPress();
            executing = true;
        }
    }

    protected override void ExecuteOnExit(Collider2D other)
    {
        base.ExecuteOnExit(other);
        executing = false;
    }

}
