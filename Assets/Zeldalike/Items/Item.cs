using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.Audio;

public abstract class Item : ScriptableObject
{
    public Sprite onBackSprite;
    public Sprite aboveHeadSprite;
    public SoundEffect pickupSfx;
    public abstract void Begin(ItemController user);
    public abstract void Hold(ItemController user);
    public abstract void End(ItemController user);
}
