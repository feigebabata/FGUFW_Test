using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FGUFW.ECS
{
    public static class GameObjectPool
    {
        private static Dictionary<Type,Pool<GameObject>> poolDict = new Dictionary<Type, Pool<GameObject>>();
        private static Transform poolParent;

        public static void InitPool<T>(GameObject template,int capacity) where T:MonoBehaviour
        {
            if(poolParent==null)
            {
                poolParent = new GameObject("GameObjectPool").transform;
                GameObject.DontDestroyOnLoad(poolParent.gameObject);
                poolParent.transform.position = Vector3.one * 10000;
            }
            var pool = new Pool<GameObject>(template,capacity,true,onCopyT,onReCycle);
            poolDict.Add(typeof(T),pool);
        }

        public static void ReCycle<T>(T obj) where T:MonoBehaviour
        {
            var pool = poolDict[typeof(T)];
            pool.ReCycle(obj.gameObject);
        }

        public static void Get<T>() where T:MonoBehaviour
        {
            var pool = poolDict[typeof(T)];
            pool.Get().GetComponent<T>();
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
