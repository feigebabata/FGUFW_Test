
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

        // [HideInInspector]
        public float Progress=0.5f;

        // /// <summary>
        // /// 是否处于活动状态
        // /// </summary>
        // [HideInInspector]
        // public bool Active;

        // public void Enter(BehaviorTreeNodeBase prev,int outIdx,float worldTime)
        // {
        //     int inIdx = Prevs.FindIndex(ls=>ls.Contains(prev));
        //     OnEnter(prev,outIdx,worldTime,inIdx);
        // }

        // public virtual void OnEnter(BehaviorTreeNodeBase prev,int outIdx,float worldTime,int inIdx)
        // {
        //     Progress = 0;
        //     Active = true;

        //     #if UNITY_EDITOR
        //     OnEnterCallback();
        //     #endif
        // }

        // public virtual void OnInit(BehaviorTreeBase tree,float worldTime)
        // {
        //     Progress = 0;
        //     Active = false;
        // }

        // public virtual void ToNexts(float worldTime)
        // {
        //     Progress = 0;
        //     Active = false;

        //     #if UNITY_EDITOR
        //     OnNextCallback();
        //     #endif
        //     for (int i = 0; i < Nexts.Count; i++)
        //     {
        //         foreach (var item in Nexts[i])
        //         {
        //             toNext(worldTime,item,i);   
        //         }
        //     }
        // }

        // protected void toNext(float worldTime, BehaviorTreeNodeBase node,int outIdx)
        // {
        //     node.Enter(this,outIdx,worldTime);
        // }

        
    }

    public class BehaviorTreeNodePort
    {
        public int Index;
        public string Name;

        /// <summary>
        /// BehaviorTreeNodeBase为纯连接 object,int,float,float2,float3,float4,string
        /// </summary>
        public Type Type;
        public BehaviorTreeNodeBase Node;
        public List<BehaviorTreeNodePort> Links = new List<BehaviorTreeNodePort>();

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

    
    /// <summary>
    /// NodeView添加进图条 需要手动定义 float Progress字段 懒得写基类了
    /// </summary>
    public interface IBehaviorTreeNodeUpdateProgress
    {   
        void UpdateProgress(float worldTime);
    }
}