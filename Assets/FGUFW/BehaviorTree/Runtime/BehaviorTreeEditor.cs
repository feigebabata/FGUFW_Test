#if UNITY_EDITOR

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

        public abstract Type[] GetNodeTypes();
        public abstract string[] GetNodeNames();



        public void Save()
        {
            if(!UnityEditor.EditorApplication.isPlaying)
            {
                UnityEditor.EditorUtility.SetDirty(this);
                UnityEditor.AssetDatabase.SaveAssets();
            }
        }

        public virtual void InitTemplate()
        {
            
        }

        public void Clear()
        {
            foreach (var node in Nodes)
            {
                if(!UnityEditor.EditorApplication.isPlaying)
                {
                    UnityEditor.AssetDatabase.RemoveObjectFromAsset(node);
                }
            }
            Nodes.Clear();
        }

    }
}
#endif