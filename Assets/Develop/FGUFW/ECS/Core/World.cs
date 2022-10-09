using FGUFW;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace FGUFW.ECS
{
    public partial class World
    {
        /// <summary>
        /// 无效实体UID
        /// </summary>
        public const int ENTITY_NONE = 0;

        /// <summary>
        /// 存单例组件的实体
        /// </summary>
        public const int ENTITY_SINGLE = -1;

        public static World Current;

        /// <summary>
        /// 实体索引 依次加一
        /// </summary>
        private int _createEntityIndex=0;

        private Dictionary<int, IDictionary> _compDict = new Dictionary<int, IDictionary>();
        private int[] _filterKeyCache;
        private int _filterKeyCacheLength;
        private object[] _createArgs=new object[1];

        
        public World()
        {
            _maxRanderLength = (float)ScreenHelper.SmoothFPS/FRAME_COUNT;
            _worldCreateTime = TimeHelper.Time;
            initSystem();
            PlayerLoopHelper.AddToLoop<UnityEngine.PlayerLoop.Update,World>(update,true);
            Current = this;
        }

        public void Dispose()
        {
            PlayerLoopHelper.RemoveToLoop<UnityEngine.PlayerLoop.Update>(update);
            
            disposeSys();
            disposeComps();
            Current = null;
        }

        public bool GetComponent<T>(int entityUId,out T comp) where T: struct,IComponent
        {
            if(entityUId==ENTITY_NONE)
            {
                holdNoneEntityUId();
                comp = default(T);
                return false;
            }

            var key = ComponentTypeHelper.GetTypeValue<T>();
            var comps = getComps<T>();
            if(comps==null || !comps.ContainsKey(entityUId))
            {
                comp = default(T);
                return false;
            }
            comp = comps[entityUId];
            return true;
        }

        public Dictionary<int, T>.ValueCollection GetComponents<T>()where T: struct,IComponent
        {
            var comps = getComps<T>();
            if(comps==null || comps.Count==0)
            {
                return null;
            }
            return comps.Values;
        }

        public void AddOrSetComponent<T>(int entityUId,T comp) where T : struct, IComponent
        {
            if(entityUId==ENTITY_NONE)
            {
                holdNoneEntityUId();
                return;
            }

            comp.EntityUId = entityUId;
            int key = comp.CompType;
            
            if(!_compDict.ContainsKey(key))
            {
                var comps = new Dictionary<int,T>();
                _compDict.Add(key,comps);
                comps.Add(entityUId,comp);
            }
            else
            {
                var comps = _compDict[key] as Dictionary<int,T>;
                if(!comps.ContainsKey(entityUId))
                {
                    comps.Add(entityUId,comp);
                }
                else
                {
                    comps[entityUId] =comp;
                }
            }
        }

        public void RemoveComponent<T>(int entityUId) where T:IComponent
        {
            if(entityUId==ENTITY_NONE)
            {
                holdNoneEntityUId();
                return;
            }
            var comps = getComps<T>();
            if(comps==null || !comps.ContainsKey(entityUId))return;
            comps[entityUId].Dispose();
            comps.Remove(entityUId);
        }

        /// <summary>
        /// 泛型的性能更好
        /// </summary>
        /// <param name="entityUId"></param>
        /// <param name="compTypeVal"></param>
        public void RemoveComponent(int entityUId,int compTypeVal)
        {
            if(entityUId==ENTITY_NONE)
            {
                holdNoneEntityUId();
                return;
            }
            var key = compTypeVal;
            if(!_compDict.ContainsKey(key) || _compDict[key].Count==0)return;
            var comps = _compDict[key];
            if(comps.Contains(entityUId))
            {
                var comp = (IComponent)comps[entityUId];
                comp.Dispose();
                comps.Remove(entityUId);
            }

        }

        public void DestroyEntity(int entityUId)
        {
            if(entityUId==ENTITY_NONE)
            {
                holdNoneEntityUId();
                return;
            }

            foreach (var kv in _compDict)
            {
                var comps = kv.Value;
                if(comps.Contains(entityUId))
                {
                    var comp = (IComponent)comps[entityUId];
                    comp.Dispose();
                    comps.Remove(entityUId);
                }
            }
        }

        /// <summary>
        /// 实体唯一索引 0:未知 -1:单例
        /// </summary>
        /// <returns>EntityUID</returns>
        public int CreateEntity()
        {
            _createEntityIndex++;
            return _createEntityIndex;
        }

        private void disposeComps()
        {
            foreach (var kv in _compDict)
            {
                foreach (var item in kv.Value.Values)
                {
                    var comp = (IComponent)item;
                    comp.Dispose();
                }
                kv.Value.Clear();
            }
            _compDict.Clear();
        }

        private Dictionary<int,T> getComps<T>()
        {
            var key = ComponentTypeHelper.GetTypeValue<T>();
            if(!_compDict.ContainsKey(key))
            {
                return null;
            }
            return _compDict[key] as Dictionary<int,T>;
        }

        private void holdNoneEntityUId()
        {
            // Debug.LogError("无效ID");
        }

        private void setFilterKeyCache(ICollection<int> keys)
        {
            int length = keys.Count;
            if(_filterKeyCacheLength<length)
            {
                _filterKeyCache = new int[length];
            }

            keys.CopyTo(_filterKeyCache,0);
            
            _filterKeyCacheLength = length;
        }

        
    }
}