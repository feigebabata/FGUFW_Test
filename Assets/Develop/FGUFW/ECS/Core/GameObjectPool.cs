using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FGUFW.ECS
{
    public static class GameObjectPool
    {

        private static Dictionary<int,Pool<GameObject>> poolDict = new Dictionary<int, Pool<GameObject>>();
        private static Transform poolParent;

        public static void InitPool(int type,GameObject template,int capacity)
        {
            if(poolParent==null)
            {
                poolParent = new GameObject("GameObjectPool").transform;
                GameObject.DontDestroyOnLoad(poolParent.gameObject);
                poolParent.transform.position = Vector3.one * 10000;
            }
            var pool = new Pool<GameObject>(template,capacity,true,onCopyT,onReCycle);
            poolDict.Add(type,pool);
        }

        public static void ReCycle(int type,GameObject obj)
        {
            var pool = poolDict[type];
            pool.ReCycle(obj.gameObject);
        }

        public static GameObject Get(int type)
        {
            var pool = poolDict[type];
            return pool.Get();
        }

        

        private static void onReCycle(bool inPool,GameObject obj)
        {
            if(inPool)
            {
                obj.transform.SetParent(poolParent);
                obj.transform.localPosition = Vector3.zero;
            }
            else
            {
                GameObject.Destroy(obj);
            }
        }

        private static GameObject onCopyT(GameObject arg)
        {
            return GameObject.Instantiate(arg);
        }

        public static void Clear()
        {
            foreach (var kv in poolDict)
            {
                foreach (var gObj in kv.Value)
                {
                    GameObject.Destroy(gObj);
                }
                kv.Value.Clear();
            }
            poolDict.Clear();
        }

    }
}
