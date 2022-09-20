using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FGUFW.ECS
{
    public static class MonoPool
    {
        private static Dictionary<Type,List<GameObject>> poolDict = new Dictionary<Type, List<GameObject>>();

        public static void InitPool<T>(GameObject template,int capacity)
        {
            
        }

        public static void Clear()
        {
            foreach (var kv in poolDict)
            {
                foreach (var gObj in kv.Value)
                {
                    GameObject.Destroy(gObj);
                }
            }
        }

    }
}
