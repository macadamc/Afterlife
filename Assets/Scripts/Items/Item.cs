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
}
