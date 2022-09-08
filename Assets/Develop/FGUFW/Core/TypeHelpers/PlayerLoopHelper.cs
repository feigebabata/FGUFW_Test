using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

namespace FGUFW
{
    public static class PlayerLoopHelper
    {
        /// <summary>
        /// 添加自定义的回调在循环 T类型须在命名空间 UnityEngine.PlayerLoop
        /// </summary>
        /// <param name="callback"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static void AddToLoop<T>(PlayerLoopSystem.UpdateFunction callback)
        {
            var currentPlayerLoop = PlayerLoop.GetCurrentPlayerLoop();
            var updateIndex = currentPlayerLoop.subSystemList.IndexOf<PlayerLoopSystem>(p=> p.type==typeof(T));
            var updateSys = currentPlayerLoop.subSystemList[updateIndex];
            var ls = new List<PlayerLoopSystem>(updateSys.subSystemList);
            var newLoop = new PlayerLoopSystem
            {
                type = typeof(T),
                updateDelegate = callback
            };
            ls.Add(newLoop);
            updateSys.subSystemList = ls.ToArray();
            currentPlayerLoop.subSystemList[updateIndex] = updateSys;
            PlayerLoop.SetPlayerLoop(currentPlayerLoop);
        }
        
        public static void RemoveToLoop<T>(PlayerLoopSystem.UpdateFunction callback)
        {
            var currentPlayerLoop = PlayerLoop.GetCurrentPlayerLoop();
            var updateIndex = currentPlayerLoop.subSystemList.IndexOf<PlayerLoopSystem>(p=> p.type==typeof(T));
            var updateSys = currentPlayerLoop.subSystemList[updateIndex];
            var ls = new List<PlayerLoopSystem>(updateSys.subSystemList);
            ls.RemoveAll(pls=>pls.updateDelegate==callback);
            updateSys.subSystemList = ls.ToArray();
            currentPlayerLoop.subSystemList[updateIndex] = updateSys;
            PlayerLoop.SetPlayerLoop(currentPlayerLoop);
        }

    }
}