using System.Collections.Generic;
using System.Threading.Tasks;
using FGUFW;
using UnityEngine;

namespace FGUFW.GamePlay
{
    public interface IUpdateable
    {
        void OnUpdate();
    }

    public static class IUpdateableExtensions
    {
        public static void AddUpdateToPlayerLoop(this IUpdateable self,bool first=false)
        {
            PlayerLoopHelper.AddToLoop<UnityEngine.PlayerLoop.Update>(self.OnUpdate,self.GetType());
        }

        public static void RemoveUpdateToPlayerLoop(this IUpdateable self)
        {
            PlayerLoopHelper.RemoveToLoop<UnityEngine.PlayerLoop.Update>(self.OnUpdate);
        }
    }
}