using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FGUFW.MonoActTriggers
{
    public class MonoRequire_Cd : MonoActTriggerRequire,MonoActTrigger.IOnTriggerAct
    {
        void Start(){}

        public float LastTime;
        public float Cd;
        public override bool Match(in MonoActTriggerRequireData key)
        {
            enabled = key.WorldTime>=LastTime+Cd*(1-key.CD_Buff);
            return enabled;
        }

        public void OnTriggerAct(float worldTime)
        {
            LastTime = worldTime;
        }
    }
}
