using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FGUFW.MonoActTriggers
{
    public class MonoRequire_Event : MonoActTriggerRequire
    {
        public int Events;
        void Start(){}
        public override bool Match(in MonoActTriggerRequireData key)
        {
            enabled = BitHelper.Overlap(Events,key.Events);
            return enabled;
        }

    }
}
