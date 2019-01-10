using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.Audio;

public class Item : ScriptableObject
{
    [TextArea]
    public string description;
    public Sprite onBackSprite;
    public Sprite aboveHeadSprite;
    public SoundEffect pickupSfx;
    public bool strafeLockedWhileHeld;

    public virtual void Begin(ItemController user) { }
    public virtual void Hold(ItemController user) { }
    public virtual void End(ItemController user) { }
    public virtual void Init(ItemController user)
    {
        user.OnStart.AddListener(Begin);
        user.OnHold.AddListener(Hold);
        user.OnEnd.AddListener(End);
    }
    public virtual void Clean(ItemController user)
    {
        user.OnStart.RemoveListener(Begin);
        user.OnHold.RemoveListener(Hold);
        user.OnEnd.RemoveListener(End);
    }
}
