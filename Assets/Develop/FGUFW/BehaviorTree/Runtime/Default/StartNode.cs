
using FGUFW.BehaviorTree;
using UnityEngine;

namespace FGUFW.BehaviorTree
{
    
    public class StartNode : BehaviorTreeNodeBase,IBehaviorTreeNodeOutPort
    {
        public override void OnEnter(BehaviorTreeNodeBase prev,float worldTime)
        {
            ToNexts(worldTime);
        }

        void OnEnable()
        {
            TitleBackgroundColor = new Color(0.63f,0.35f,0.06f);
        }
    }
}