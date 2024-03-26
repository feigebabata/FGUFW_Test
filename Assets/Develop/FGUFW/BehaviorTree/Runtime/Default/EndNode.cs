
using System;
using FGUFW.BehaviorTree;
using UnityEngine;

namespace FGUFW.BehaviorTree
{

    public class EndNode : BehaviorTreeNodeBase,IBehaviorTreeNodeInPort
    {
        public Action ToEnded;

        public override void OnEnter(BehaviorTreeNodeBase prev,float worldTime)
        {
            Active = true;
            ToEnded?.Invoke();
        }

        void OnEnable()
        {
            TitleBackgroundColor = new Color(0.58f,0.07f,0.07f);
        }
    }
}