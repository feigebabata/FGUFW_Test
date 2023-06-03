using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using UnityEngine.Jobs;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Collections;
using System;
using FGUFW;
using UnityEngine.Pool;

namespace FGUFW.SimpleECS
{
    public abstract class ComponentGroup:IDisposable
    {
        public int Length{get;protected set;}
        public bool ContainsTransform{get;private set;}
        public TransformAccessArray Transforms{get;protected set;}
        public NativeList<int> EntityIds{get;private set;}
        public NativeHashMap<int,int> EntityIdToIndexs => _entityIdToIndexs;

        private NativeHashMap<int,int> _entityIdToIndexs;
        private int _lastEntityId;
        protected ObjectPool<Transform> _objectPool;
        protected GameObject _objectPoolPrefab;
        protected Vector3 _objectPoolRelease;

        /// <summary>
        /// 初始化组件容器
        /// </summary>   
        public ComponentGroup(GameObject prefab,int capacity=64)
        {
            if(capacity<0)capacity=64;

            ContainsTransform = prefab;
            _objectPoolPrefab = prefab;


            EntityIds = new NativeList<int>(capacity,Allocator.Persistent);
            _entityIdToIndexs = new NativeHashMap<int, int>(capacity,Allocator.Persistent);
            if(ContainsTransform)
            {
                Transforms = new TransformAccessArray(capacity);
                _objectPool = new ObjectPool<Transform>(objectPoolCreateFunc,objectPoolActionOnRelease,objectPoolActionOnDestroy);
                _objectPoolRelease = new Vector3(-10000,-10000,-10000);
            }
        }

        /// <summary>
        /// 返回的是Id不是Index
        /// </summary>        
        public int CreateEntity()
        {
            int id = _lastEntityId++;
            EntityIds.Add(id);
            _entityIdToIndexs.Add(id,Length);
            Length++;
            return id;
        }

        /// <summary>
        /// 返回的是Id不是Index
        /// </summary>
        public int CreateEntity(Transform t)
        {
            if(!ContainsTransform)throw new Exception("当前组件Group不包含Transform!");
            Transforms.Add(t);
            return CreateEntity();
        }


        public void DestroyEntity(int entityId)
        {
            var entityIndex = IdToIndex(entityId);
            if(entityIndex == -1)return;
  
            Length--;

            int replaceId = EntityIds[Length];
            _entityIdToIndexs.Remove(entityId);
            _entityIdToIndexs[replaceId] = entityIndex;
            EntityIds.RemoveAtSwapBack(entityIndex);
            if(ContainsTransform)
            {
                _objectPool.Release(Transforms[entityIndex]);
                Transforms.RemoveAtSwapBack(entityIndex);
            }

            OnDestroyEntity(entityIndex);
        }

        /// <summary>
        /// 移动最后一个组件到被移除的索引
        /// </summary>
        protected abstract void OnDestroyEntity(int entityIndex);

        public void Dispose()
        {
            Length = 0;

            Transforms.Dispose();
            _entityIdToIndexs.Dispose();
            EntityIds.Dispose();

            OnDispose();
        }

        /// <summary>
        /// 释放其他组件
        /// </summary>
        protected abstract void OnDispose();

        public NativeArray<int> Instantiate(int count)
        {
            var ids = new NativeArray<int>(count,Allocator.Temp);
            for (int i = 0; i < count; i++)
            {
                var t = _objectPool.Get();
                ids[i] = CreateEntity(t);
                OnInstantiate(ids[i],t);
            }
            return ids;
        }

        protected virtual void OnInstantiate(int id,Transform t){}

        public int IdToIndex(int entityId)
        {
            return EntityIdToIndex(_entityIdToIndexs,entityId);
        }

        public int IndexToId(int index)
        {
            return EntityIndexToId(EntityIds,index);
        }

        public static int EntityIdToIndex(NativeHashMap<int,int> entityIndexs, int id)
        {
            if(entityIndexs.ContainsKey(id))return entityIndexs[id];
            return -1;
        }

        public static int EntityIndexToId(NativeList<int> entityIds, int index)
        {
            if(index<entityIds.Length)return entityIds[index];
            return -1;
        }

        private void objectPoolActionOnDestroy(Transform transform)
        {
            GameObject.Destroy(transform.gameObject);
        }

        protected virtual void objectPoolActionOnRelease(Transform transform)
        {
            transform.parent = null;
            transform.position = _objectPoolRelease;
        }

        private Transform objectPoolCreateFunc()
        {
            return GameObject.Instantiate(_objectPoolPrefab).transform;
        }

    } 
    
}
