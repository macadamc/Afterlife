using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using ShadyPixel.Extentions;

public class LayersTest : MonoBehaviour
{
    public LayerMask maskToCombine1;
    public LayerMask MasktoCombine2;

    public LayerMask mask1;
    public LayerMask ignoreMask;

    public LayerMask output;

    private void Reset()
    {
        mask1.SetMask("Everything");
    }


    //      Creating Masks
    [Button]
    void DoNewMask()
    {
        LayerMask value;
        //using Layer Names. (Default Unity)
        value = LayerMask.GetMask(Layers.Trigger, Layers.Entity, Layers.Default);

        //or for an empty mask;
        value = 0;

        //or

        // using Layer Masks (this is probably a little faster).
        value = LayerMaskExtensions.CombineMasks(Layers.TriggerMask, Layers.EntityMask, Layers.DefaultMask);

        output = value;
    }

    //      Combining Masks
    [Button]
    void DoCombine()
    {
        // static CombineMasks creates a new mask from any number of existing masks. 
        output = LayerMaskExtensions.CombineMasks(maskToCombine1, MasktoCombine2);

        output.SetMask("Nothing");
        //or

        // Updates this LayerMask with any number of other layerMasks.
        output.Combine(maskToCombine1, MasktoCombine2);
        //or

        //if we wanted maskToCombine1 to be updated with the new mask instead, we can do something like this.
        maskToCombine1.Combine(MasktoCombine2);
    }

    //          removing/filtering out Masks.
    [Button]
    void DoIgnore()
    {
        // does not change any instance values.
        output = LayerMaskExtensions.FilterMask(mask1, ignoreMask);

        output.SetMask("Nothing");

        // another way to write it but changes outputs value.
        output.Combine(mask1).IgnoreFlagsInMask(ignoreMask);

        //if we wanted mask1 to be updated with the new mask instead, we can do something like this.
        mask1.IgnoreFlagsInMask(ignoreMask);
    }
    void DoTests()
    {
        LayerMask op = LayerMask.GetMask(Layers.Default, Layers.Entity, Layers.Trigger);
        LayerMask other = LayerMask.GetMask(Layers.UI, Layers.Water);

        LayerMask combined = LayerMaskExtensions.CombineMasks(op, other);
    }
}