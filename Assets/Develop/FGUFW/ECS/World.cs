using FGUFW;
using System;
using System.Collections;
using System.Collections.Generic;
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

        /// <summary>
        /// 实体索引 依次加一
        /// </summary>
        private int _createEntityIndex=0;

        private Dictionary<int, IDictionary> _compDict = new Dictionary<int, IDictionary>();

        
        public World()
        {
            _worldCreateTime = Time.unscaledTime;
            PlayerLoopHelper.AddToLoop<UnityEngine.PlayerLoop.Update,World>(update);
        }

        public void Dispose()
        {
            PlayerLoopHelper.RemoveToLoop<UnityEngine.PlayerLoop.Update>(update);
            
            disposeSys();
            disposeComps();
        }

        public bool GetComponent<T>(int entityUId,out T comp) where T: struct,IComponent
        {
            var key = ComponentTypeHelper.GetTypeValue<T>();
            if(!_compDict.ContainsKey(key) || _compDict[key].Count==0 || !_compDict[key].Contains(entityUId))
            {
                comp = default(T);
                return false;
            }
            comp = (T)_compDict[key][entityUId];
            return true;
        }

        public void AddOrSetComponent<T>(int entityUId,T comp) where T : struct, IComponent
        {
            comp.EntityUId = entityUId;
            int key = comp.CompType;
            if(!_compDict.ContainsKey(key))_compDict.Add(key,new Dictionary<int,T>());
            var comps = _compDict[key];
            
            if(comps.Contains(entityUId))
            {
                comps.Add(entityUId,comp);
            }
            else
            {
                comps[entityUId] =comp;
            }
        }

        public void RemoveComponent<T>(int entityUId)
        {
            var key = ComponentTypeHelper.GetTypeValue<T>();
            RemoveComponent(entityUId,key);
        }

        public void RemoveComponent(int entityUId,int compTypeVal)
        {
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
            if(entityUId<1)return;//无效id

            foreach (var kv in _compDict)
            {
                var dict = kv.Value;
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

        private static void setComp<T>(Dictionary<int,T> dict,int entityUId,T comp)
        {
            dict[entityUId] = comp;
        }
        
    }
}