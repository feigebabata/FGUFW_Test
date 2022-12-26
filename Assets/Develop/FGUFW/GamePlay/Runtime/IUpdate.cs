using System.Collections.Generic;
using System.Threading.Tasks;
using FGUFW;
using UnityEngine;

namespace FGUFW.GamePlay
{
    public interface IUpdate
    {
        void Update();
    }

    public static class IUpdateExtensions
    {
        public static void AddUpdateToPlayerLoop(this IUpdate self)
        {
            PlayerLoopHelper.AddToLoop<UnityEngine.PlayerLoop.Update>(self.Update,self.GetType());
        }

        public static void RemoveUpdateToPlayerLoop(this IUpdate self)
        {
            PlayerLoopHelper.RemoveToLoop<UnityEngine.PlayerLoop.Update>(self.Update);
        }
    }
}