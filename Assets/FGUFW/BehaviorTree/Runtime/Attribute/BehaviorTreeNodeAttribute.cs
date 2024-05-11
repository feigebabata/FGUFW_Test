using System;
using UnityEngine;

namespace FGUFW.BehaviorTree
{
    public class BehaviorTreeNodeTitleAttribute:Attribute
    {
        public string Title;
    }

    public class BehaviorTreeNodeProgressColorAttribute:Attribute
    {
        public Color Color;
    }

    public class BehaviorTreeNodeSearchPathAttribute:Attribute
    {
        public string Path;
    }
}