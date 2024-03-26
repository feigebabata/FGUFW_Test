using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FGUFW.MonoActTriggers
{
    public class MonoRequire_TextRules : MonoActTriggerRequire
    {
        public TextRulesMatch TextRules;

        #if UNITY_EDITOR
        public string RulesText;
        #endif

        void Start(){}

        public override bool Match(in MonoActTriggerRequireData key)
        {
            enabled = TextRules.Match(key.VariateOperate);
            return enabled;
        }

    }
}
