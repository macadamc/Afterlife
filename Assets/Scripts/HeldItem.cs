using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeldItem : MonoBehaviour
{
    public ItemControllerEvent onStart;
    public ItemControllerEvent onHeld;
    public ItemController User { get; private set; }


    public virtual void Init(ItemController user)
    {
        user.OnHold.AddListener(Hold);
        user.OnEnd.AddListener(End);
    }
    public virtual void Clean(ItemController user)
    {
        user.OnHold.RemoveListener(Hold);
        user.OnEnd.RemoveListener(End);
    }

    public virtual void Begin(ItemController user)
    {
        if (User == null)
            User = user;

        onStart.Invoke(user);
    }

    public virtual void Hold(ItemController user)
    {
        onHeld.Invoke(user);
    }

    public virtual void End(ItemController user)
    {

    }
}