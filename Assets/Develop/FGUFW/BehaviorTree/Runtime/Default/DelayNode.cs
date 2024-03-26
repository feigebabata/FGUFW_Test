using System;
using System.Collections.Generic;
using FGUFW.BehaviorTree;
using UnityEngine;

namespace FGUFW.BehaviorTree
{

    public class DelayNode : BehaviorTreeNodeBase,IBehaviorTreeNodeOutPort,IBehaviorTreeNodeInPort,IBehaviorTreeNodeProgress
    {
        [HideInInspector]
        public float Progress;
        public float Delay;

        private float _startTime;

        public override void OnInit(BehaviorTreeBase tree, float worldTime)
        {
            base.OnInit(tree, worldTime);
            _startTime = 0;
            Progress = 0;
        }

        public override void OnEnter(BehaviorTreeNodeBase prev,float worldTime)
        {
            Active = true;
            _startTime = worldTime;

        }

        public void UpdateProgress(float worldTime)
        {
            Progress = (worldTime-_startTime)/Delay;

            if(Progress>=1)
            {
                ToNexts(worldTime);
            }
        }

        public override void ToNexts(float worldTime)
        {
            Active = false;
            Progress=0;
            base.ToNexts(worldTime);
        }
    }
}