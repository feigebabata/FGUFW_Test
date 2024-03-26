using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FGUFW.MonoActTriggers
{
    public class MonoRequire_LifeTime : MonoActTriggerRequire
    {
        public float StartTime;
        public float Time;
        void Start(){}
        public override bool Match(in MonoActTriggerRequireData key)
        {
            enabled = key.WorldTime>StartTime+Time;
            return enabled;
        }
    }
}
