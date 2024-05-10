#define SHOW_ALL  //在编辑器显示所有数据

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UIElements;

namespace FGUFW.BehaviorTree
{
    public abstract partial class BehaviorTreeBase : ScriptableObject
    {
        [HideInInspector]
        public List<BehaviorTreeNodeBase> Nodes = new List<BehaviorTreeNodeBase>();

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

        }

        public void AddChild(BehaviorTreeNodeBase parent,BehaviorTreeNodeBase child)
        {
            // parent.Nexts.Add(child);
        }

        public void RemoveChild(BehaviorTreeNodeBase parent,BehaviorTreeNodeBase child)
        {
            // parent.Nexts.Remove(child);
        }

        public void UpdateProgress(float worldTime)
        {
            // foreach (var node in Nodes)
            // {
            //     if(node is IBehaviorTreeNodeUpdateProgress behaviorTreeNodeProgress && node.Active)
            //     {
            //         behaviorTreeNodeProgress.UpdateProgress(worldTime);
            //     }
            // }
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
                    // int idx = this.Nodes.IndexOf(newNode.Nexts[i]);
                    // newNode.Nexts[i] = tree.Nodes[idx];
                }
            }

            return tree;
        }


        public virtual void OnInit(float worldTime)
        {
            foreach (var node in Nodes)
            {
                // node.OnInit(this,worldTime);
            }
        }
    }


}