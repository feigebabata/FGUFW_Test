#define SHOW_ALL  //在编辑器显示所有数据

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UIElements;

namespace FGUFW.BehaviorTree
{
    public abstract class BehaviorTreeBase : ScriptableObject
    {
#if SHOW_ALL
        [HideInInspector]
#endif
        public List<BehaviorTreeNodeBase> Nodes = new List<BehaviorTreeNodeBase>();

        public abstract Type[] GetNodeTypes();
        public abstract string[] GetNodeNames();

        public BehaviorTreeNodeBase CreateNode(Type type, string nodeName)
        {
            var node = ScriptableObject.CreateInstance(type) as BehaviorTreeNodeBase;
            node.name = nodeName;
            Nodes.Add(node);

            #if UNITY_EDITOR
            if(!UnityEditor.EditorApplication.isPlaying)
            {
                UnityEditor.AssetDatabase.AddObjectToAsset(node,this);
            }
            #endif

            Save();

            return node;
        }

        public void DeleteNode(BehaviorTreeNodeBase node)
        {
            Nodes.Remove(node);

            #if UNITY_EDITOR
            if(!UnityEditor.EditorApplication.isPlaying)
            {
                UnityEditor.AssetDatabase.RemoveObjectFromAsset(node);
            }
            #endif

            Save();
        }

        public void AddChild(BehaviorTreeNodeBase parent,BehaviorTreeNodeBase child)
        {
            parent.Nexts.Add(child);
            Save();
        }

        public void RemoveChild(BehaviorTreeNodeBase parent,BehaviorTreeNodeBase child)
        {
            parent.Nexts.Remove(child);
            Save();
        }

        public void UpdateProgress(float worldTime)
        {
            foreach (var node in Nodes)
            {
                if(node is IBehaviorTreeNodeProgress behaviorTreeNodeProgress && node.Active)
                {
                    behaviorTreeNodeProgress.UpdateProgress(worldTime);
                }
            }
        }

        [Conditional("UNITY_EDITOR")]
        public void Save()
        {
            if(!UnityEditor.EditorApplication.isPlaying)
            {
                UnityEditor.EditorUtility.SetDirty(this);
                UnityEditor.AssetDatabase.SaveAssets();
            }
        }

        /// <summary>
        /// 运行时实例化 行为树配置 如果在外部引用了Node需修改引用为复制后的Node
        /// </summary>
        /// <returns></returns>
        public virtual BehaviorTreeBase Clone()
        {
            var tree = Instantiate(this);
            tree.Nodes = Nodes.ConvertAll(n=>Instantiate(n));
            foreach (var newNode in tree.Nodes)
            {
                for (int i = 0; i < newNode.Nexts.Count; i++)
                {
                    int idx = this.Nodes.IndexOf(newNode.Nexts[i]);
                    newNode.Nexts[i] = tree.Nodes[idx];
                }
            }

            return tree;
        }

        public void Clear()
        {
            foreach (var node in Nodes)
            {
                #if UNITY_EDITOR
                if(!UnityEditor.EditorApplication.isPlaying)
                {
                    UnityEditor.AssetDatabase.RemoveObjectFromAsset(node);
                }
                #endif
            }
            Nodes.Clear();
            Save();
        }

        public virtual void OnInit(float worldTime)
        {
            foreach (var node in Nodes)
            {
                node.OnInit(this,worldTime);
            }
        }
    }

    public abstract class BehaviorTreeNodeBase : ScriptableObject
    {
#if UNITY_EDITOR
        /// <summary>
        /// 节点在编辑器内的位置 不用搭理
        /// </summary>
#if SHOW_ALL
        [HideInInspector]
#endif
        public Vector2 Position;

        /// <summary>
        /// 节点头部背景色 可按需修改
        /// </summary>
#if SHOW_ALL
        [HideInInspector]
#endif
        public Color TitleBackgroundColor = new Color(0.25f,0.25f,0.25f,1f);

        /// <summary>
        /// 进入下一节点的记录 用于编辑器绘制
        /// </summary>
        // [HideInInspector]
        // public List<BehaviorTreeNodeBase> EnterNextRecord = new List<BehaviorTreeNodeBase>();
#endif

        /// <summary>
        /// 节点连接关系 唯一需要在意的
        /// </summary>
#if SHOW_ALL
        [HideInInspector]
#endif
        public List<BehaviorTreeNodeBase> Nexts = new List<BehaviorTreeNodeBase>();

        /// <summary>
        /// 是否处于活动状态
        /// </summary>
#if SHOW_ALL
        [HideInInspector]
#endif
        public bool Active;

        public abstract void OnEnter(BehaviorTreeNodeBase prev,float worldTime);

        public virtual void OnInit(BehaviorTreeBase tree,float worldTime)
        {
            Active = false;
        }

        public virtual void ToNexts(float worldTime)
        {
            foreach (var next in Nexts)
            {
                toNext(worldTime,next);   
            }
        }

        protected void toNext(float worldTime, BehaviorTreeNodeBase node)
        {
            node.OnEnter(this,worldTime);
// #if UNITY_EDITOR
//             EnterNextRecord.Add(node);
// #endif
        }

        
    }

    /// <summary>
    /// NodeView添加In端口
    /// </summary>
    public interface IBehaviorTreeNodeInPort
    {
    }

    /// <summary>
    /// NodeView添加Out端口
    /// </summary>
    public interface IBehaviorTreeNodeOutPort
    {
        
    }
    
    /// <summary>
    /// NodeView添加进图条 需要手动定义 float Progress字段 懒得写基类了
    /// </summary>
    public interface IBehaviorTreeNodeProgress
    {
        // [HideInInspector]public float Progress;
        
        void UpdateProgress(float worldTime);
    }

}