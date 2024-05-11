
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FGUFW.BehaviorTree
{
    public partial class BehaviorTreeNodeBase : ScriptableObject
    {
        [HideInInspector]
        public List<BehaviorTreeNodePort> Nexts = new List<BehaviorTreeNodePort>();

        [HideInInspector]
        public List<BehaviorTreeNodePort> Prevs = new List<BehaviorTreeNodePort>();

        [HideInInspector]
        public float Progress=0;

        
        public BehaviorTreeNodeBase()
        {
            Nexts.Add(new BehaviorTreeNodePort(default,default)
            {
                Name = "输入1",
                Type = typeof(bool),
            });
            Nexts.Add(new BehaviorTreeNodePort(default,default)
            {
                Name = "输入2",
                Type = typeof(Vector3),
            });
        } 
        
    }


    
    /// <summary>
    /// NodeView添加进图条 需要手动定义 float Progress字段 懒得写基类了
    /// </summary>
    public interface IBehaviorTreeNodeUpdateProgress
    {   
        void UpdateProgress(float worldTime);
    }
}