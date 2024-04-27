#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using UnityEngine;

namespace FGUFW.BehaviorTree
{
    public abstract partial class BehaviorTreeNodeBase : ScriptableObject
    {
        
        [HideInInspector]
        public Action OnEnterCallback,OnNextCallback;

        [HideInInspector]
        public Vector2 Position;

        /// <summary>
        /// 节点头部背景色 可按需修改
        /// </summary>
        [HideInInspector]
        public Color TitleBackgroundColor = new Color(0.25f,0.25f,0.25f,1f);

    }
}

#endif