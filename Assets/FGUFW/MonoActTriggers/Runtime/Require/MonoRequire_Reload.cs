using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FGUFW.MonoActTriggers
{
    public class MonoRequire_Reload : MonoActTriggerRequire,MonoActTrigger.IOnTriggerAct
    {
        public float Cd;
        public int Count;
        public float LastReloadTime;
        public int UsedCount;
        void Start(){}

        public override bool Match(in MonoActTriggerRequireData key)
        {
            if(key.WorldTime>LastReloadTime+Cd && UsedCount==Count)
            {
                UsedCount=0;
            }
            enabled = UsedCount<Count;
            return enabled;
        }

        public void OnTriggerAct(float worldTime)
        {
            UsedCount++;
            if(UsedCount==Count)
            {
                LastReloadTime = worldTime;
            }
        }
    }
}
