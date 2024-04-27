using UnityEngine;

namespace FGUFW.BehaviorTree
{
    public class BehaviorTreeComp : MonoBehaviour
    {
        [SerializeField]
        protected BehaviorTreeBase _behaviorTree;
        public BehaviorTreeBase BehaviorTree => _behaviorTree;
        public bool AutoClone;

        void Awake()
        {
            if(!_behaviorTree.IsNull() && AutoClone)
            {
                SetBehaviorTreeBase(_behaviorTree);
            }
        }

        public void SetBehaviorTreeBase(BehaviorTreeBase behaviorTreeBase)
        {
            _behaviorTree = behaviorTreeBase.Clone();
        }


    }
}