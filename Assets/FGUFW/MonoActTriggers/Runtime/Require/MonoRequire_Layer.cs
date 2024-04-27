using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FGUFW.MonoActTriggers
{
    public class MonoRequire_Layer : MonoActTriggerRequire
    {
        public int Layer;
        public int Level;
        void Start(){}

        public override bool Match(in MonoActTriggerRequireData key)
        {
            enabled = key.CacheLayer.Set(Layer,Level);
            return enabled;
        }

    }
}
