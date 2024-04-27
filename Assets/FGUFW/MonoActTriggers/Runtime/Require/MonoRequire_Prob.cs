using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FGUFW.MonoActTriggers
{
    public class MonoRequire_Prob : MonoActTriggerRequire
    {
        public float Prob;
        void Start(){}
        public override bool Match(in MonoActTriggerRequireData key)
        {
            enabled = key.Random(Prob);
            return enabled;
        }
    }
}
