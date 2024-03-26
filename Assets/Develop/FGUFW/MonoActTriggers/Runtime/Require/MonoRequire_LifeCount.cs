using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FGUFW.MonoActTriggers
{
    public class MonoRequire_LifeCount : MonoActTriggerRequire,MonoActTrigger.IOnTriggerAct
    {
        public int Count;
        void Start(){}

        public override bool Match(in MonoActTriggerRequireData key)
        {
            enabled= Count>0;
            return enabled;
        }

        public void OnTriggerAct(float worldTime)
        {
            Count--;
        }
    }
}
