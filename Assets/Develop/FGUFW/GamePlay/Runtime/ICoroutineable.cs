using System.Collections;
using FGUFW;
using UnityEngine;

namespace FGUFW.GamePlay
{
    public interface ICoroutineable
    {
        CoroutineBehaviour CoroutineBehaviour{get;set;}
    }

    public static class ICoroutineableExtensions
    {
        public static Coroutine StartCoroutine(this ICoroutineable self,IEnumerator routine)
        {
            if(self.CoroutineBehaviour==null)
            {
                Debug.LogError($"CoroutineBehaviour为空");
                return null;
            }
            return self.CoroutineBehaviour.StartCoroutine(routine);
        }

        public static void StopCoroutine(this ICoroutineable self,Coroutine routine)
        {
            if(self.CoroutineBehaviour==null)
            {
                Debug.LogError($"CoroutineBehaviour为空");
                return;
            }
            self.CoroutineBehaviour.StopCoroutine(routine);
        }

        public static void AddCoroutineBehaviour(this ICoroutineable self)
        {
            if(self.CoroutineBehaviour!=null)return;
            self.CoroutineBehaviour = new GameObject($"{self.GetType().Name}CoroutineBehaviour").AddComponent<CoroutineBehaviour>();
            GameObject.DontDestroyOnLoad(self.CoroutineBehaviour.gameObject);
        }

        public static void RemoveCoroutineBehaviour(this ICoroutineable self)
        {
            if(self.CoroutineBehaviour==null)return;
            GameObject.Destroy(self.CoroutineBehaviour.gameObject);
            self.CoroutineBehaviour=null;
        }
    }
}