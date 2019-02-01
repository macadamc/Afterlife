using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShadyPixel.Extentions
{
    public static class LayerMaskExtensions
    {
        #region Instance Methods
        public static bool Contains(this LayerMask layers, GameObject gameObject)
        {
            return 0 != (layers.value & 1 << gameObject.layer);
        }
        public static bool Contains(this LayerMask layers, int layer)
        {
            return 0 != (layers.value & 1 << layer);
        }
        public static bool Contains(this LayerMask layers, string layerName)
        {
            return 0 != (layers.value & 1 << LayerMask.NameToLayer(layerName));
        }

        public static LayerMask Combine(this LayerMask self, int mask2)
        {
            self |= mask2;
            return self;
        }
        public static LayerMask Combine(this LayerMask self, params int[] masks)
        {
            for (int i = 0; i < masks.Length; i++)
            {
                self |= masks[i];
            }
            return self;
        }
        public static LayerMask IgnoreFlagsInMask(this LayerMask self, int ignoreMask)
        {
            self &= ~ignoreMask;
            return self;
        }

        public static LayerMask SetMask(this LayerMask self, params string[] LayerNames)
        {
            var layerIDs = new int[LayerNames.Length];
            for (int i = 0; i < LayerNames.Length; i++)
            {
                layerIDs[i] = LayerMask.NameToLayer(LayerNames[i]);
            }
            return SetMask(self, layerIDs);
        }
        public static LayerMask SetMask(this LayerMask self, params int[] LayerMasks)
        {
            self.value = 0;
            foreach (var item in LayerMasks)
            {
                self.value |= item;
            }
            return self;
        }
        public static LayerMask SetMaskByLayerNumbers(this LayerMask self, params int[] LayerNumbers)
        {
            self.value = 0;
            foreach (var item in LayerNumbers)
            {
                self.value |= (1 << item);
            }
            return self;
        }
        #endregion

        #region Static Methods
        public static bool MaskContains(int mask, GameObject gameObject)
        {
            return 0 != (mask & 1 << gameObject.layer);
        }
        public static int CombineMasks(int mask1, int mask2)
        {
            return mask1 | mask2;
        }
        public static int CombineMasks(params int[] masks)
        {
            int value = 0;
            for (int i = 0; i < masks.Length; i++)
            {
                value |= masks[i];
            }
            return value;
        }
        public static int FilterMask(int mask, int ignoreMask)
        {
            return mask & ~ignoreMask;
        }
        #endregion
    }
}

