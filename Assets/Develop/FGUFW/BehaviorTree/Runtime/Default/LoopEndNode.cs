using System;
using System.Collections.Generic;
using FGUFW.BehaviorTree;
using UnityEngine;

namespace FGUFW.BehaviorTree
{
    public class LoopEndNode : BehaviorTreeNodeBase, IBehaviorTreeNodeOutPort, IBehaviorTreeNodeInPort
    {
        public override void OnEnter(BehaviorTreeNodeBase prev,float worldTime)
        {
            ToNexts(worldTime);
        }
    }
    
}