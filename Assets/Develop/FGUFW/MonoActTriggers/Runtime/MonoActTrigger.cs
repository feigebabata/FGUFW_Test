using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FGUFW.MonoActTriggers
{
    public class MonoActTrigger : MonoBehaviour
    {
        /// <summary>
        /// 触发条件
        /// </summary>
        private List<MonoActTriggerRequire> _requires = new List<MonoActTriggerRequire>();

        /// <summary>
        /// 触发功能
        /// </summary>
        public List<TriggerActData> Acts;


        public T AddRequire<T>() where T:MonoActTriggerRequire
        {
            var requeie = gameObject.AddComponent<T>();
            _requires.Add(requeie);
            return requeie;
        }

        public T GetRequire<T>() where T:MonoActTriggerRequire
        {
            foreach (var item in _requires)
            {
                if(item is T)return item as T;
            }
            return default;
        }

        public void Clear()
        {
            foreach (var item in _requires)
            {
                GameObject.Destroy(item);
            }
        }
        
        public bool MatchRequire(MonoActTriggerRequireData k)
        {
            int length = _requires.Count;
            bool match = true;
            for (int i = 0; i < length; i++)
            {
                if(!_requires[i].Match(k))
                {
                    match = false;
                }
            }
            // gameObject.SetActive(match);

            if(match)
            {
                foreach (var require in _requires)
                {
                    if(require is IOnTriggerAct onTriggerAct)
                    {
                        onTriggerAct.OnTriggerAct(k.WorldTime);
                    }

                    if(require is MonoRequire_Layer monoRequire_Layer)
                    {
                        k.Layer.Set(monoRequire_Layer.Layer,monoRequire_Layer.Level);
                    }
                }
            }
            return match;
        }

        public interface IOnTriggerAct
        {
            void OnTriggerAct(float worldTime);
        }
    }

    public enum MonoActTriggerTarget
    {
        None = 0,
        Self = 1001,
        TouchSoldier = 1002,
        TouchPoint = 1003,
        SelfRangeEnemy = 1004,
        SelfRangeFriend = 1005,
        TouchRangeFriend = 1006,
        TouchRangeEnemy = 1007,
    }

    [Serializable]
    public class TriggerActData
    {
        public int Target;
        public int Act;

        #if UNITY_EDITOR
        public string TargetName;
        public string ActName;
        #endif
    }
}

