
using System;
using UnityEngine;

namespace FGUFW.MonoActTriggers
{
    public abstract class MonoActTriggerRequire : MonoBehaviour
    {
        public abstract bool Match(in MonoActTriggerRequireData key);
    }

    [Serializable]
    public struct MonoActTriggerRequireData
    {
        public float WorldTime;
        public float DeltaTime;
        public int Events;
        public MonoActTriggerLayer Layer;
        public MonoActTriggerLayer CacheLayer;
        public IVariateOperate VariateOperate;
        public float CD_Buff;

        public bool Random(float prob)
        {
            return UnityEngine.Random.Range(0f,1f)<prob;
        }

        public void Clear()
        {
            WorldTime = 0;
            DeltaTime = 0;
            Events = 0;
            CD_Buff = 0;
        }
    }
}