using FGUFW;
using System;
using System.Collections.Generic;

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

        private Dictionary<int,Slice<Component>> _compDict = new Dictionary<int, Slice<Component>>();

        public T GetComponent<T>(int entityUId) where T:Component
        {
            var key = ComponentTypeHelper.GetTypeValue<T>();
            if(!_compDict.ContainsKey(key) || _compDict[key].Count==0)return null;
            foreach (var comp in _compDict[key])
            {
                if(comp.EntityUId==entityUId)return (T)comp;
            }
            return null;
        }

        public void AddOrSetComponent(int entityUId,Component comp)
        {
            comp.EntityUId = entityUId;
            int key = comp.CompType;
            if(!_compDict.ContainsKey(key))_compDict.Add(key,new Slice<Component>());
            var comps = _compDict[key];
            int index = comps.FindIndex(c=>c.EntityUId==entityUId);
            if(index == -1)
            {
                comps.Add(comp);
            }
            else
            {
                comps[index]=comp;
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
            comps.Remove(c=>c.EntityUId==entityUId);
        }

        public void DestroyEntity(int entityUId)
        {
            if(entityUId<1)return;//无效id
            var compTypes = _compDict.Keys;
            foreach (var compType in compTypes)
            {
                RemoveComponent(entityUId,compType);
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
        
    }
}