using System;
using System.Collections.Generic;
using FGUFW.BehaviorTree;
using UnityEngine;

namespace FGUFW.BehaviorTree
{
    
    public class LoopByCountNode : BehaviorTreeNodeBase,IBehaviorTreeNodeOutPort,IBehaviorTreeNodeInPort
    {
        public int Count;

        private int _count;

        public override void OnInit(BehaviorTreeBase tree, float worldTime)
        {
            Count = _count;
            
            base.OnInit(tree,worldTime);
        }

        void OnEnable()
        {
            _count = Count;
        }

        public override void OnEnter(BehaviorTreeNodeBase prev,float worldTime)
        {
            Count--;

            if(Count<=0)
            {
                foreach (var next in Nexts)
                {
                    if(next is LoopEndNode)
                    {
                        toNext(worldTime,next);
                    }
                }
            }
            else
            {
                foreach (var next in Nexts)
                {
                    if(!(next is LoopEndNode))
                    {
                        toNext(worldTime,next);
                    }
                }
            }

        }
    }
}