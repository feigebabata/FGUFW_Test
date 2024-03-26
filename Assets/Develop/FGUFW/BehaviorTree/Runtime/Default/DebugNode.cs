using System;
using System.Collections.Generic;
using FGUFW.BehaviorTree;
using UnityEngine;

namespace FGUFW.BehaviorTree
{
    public class DebugNode : BehaviorTreeNodeBase,IBehaviorTreeNodeOutPort,IBehaviorTreeNodeInPort
    {
        public LogType Type = LogType.Log;
        public string Message;

        public override void OnEnter(BehaviorTreeNodeBase prev,float worldTime)
        {
            switch (Type)
            {
                case LogType.Error:
                    Debug.LogError(Message);
                break;
                case LogType.Warning:
                    Debug.LogWarning(Message);
                break;
                default:
                    Debug.Log(Message);
                break;
            }
            ToNexts(worldTime);
        }
    }
}
