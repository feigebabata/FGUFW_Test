using System;
using System.Collections.Generic;

namespace FGUFW.BehaviorTree
{
    
    public class BehaviorTreeNodePort
    {
        public int Index;
        public string Name;
        public Type Type;
        public BehaviorTreeNodeBase Node;
        public List<BehaviorTreeNodePort> Links = new List<BehaviorTreeNodePort>();

        public BehaviorTreeNodePort(Action<object> setCallback,Func<object> getCallback)
        {

        }

        public bool IsLink()
        {
            return this.Type == typeof(BehaviorTreeNodeBase);
        }

        public bool EqualsType(BehaviorTreeNodePort port)
        {
            return port.Type == this.Type;
        }

        public object GetOutData()
        {
            return default;
        }

    }
}