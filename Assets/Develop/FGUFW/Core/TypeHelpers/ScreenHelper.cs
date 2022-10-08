using System;
using UnityEngine;

namespace FGUFW
{
    static public class ScreenHelper
    {
        /// <summary>
        /// 当前帧率
        /// </summary>
        /// <value></value>
        public static int FPS { get; private set; } = 60;

        private static int fpsCount, second;

        [RuntimeInitializeOnLoadMethod]
        private static void addListenerUpdate()
        {
            PlayerLoopHelper.RemoveToLoop<UnityEngine.PlayerLoop.PreUpdate>(updateCallvack);
            PlayerLoopHelper.AddToLoop<UnityEngine.PlayerLoop.PreUpdate,FPSCount>(updateCallvack);
        }

        private static void updateCallvack()
        {
            fpsCount++;
            int time = (int)Time.unscaledTime;
            if (time>second)
            {
                FPS = fpsCount/(time - second);
                fpsCount = 0;
                second = time;
            }
        }

        static public void Landscape()
        {
            Screen.orientation = ScreenOrientation.AutoRotation;
            Screen.autorotateToPortrait = false;
            Screen.autorotateToPortraitUpsideDown = false;
        }

        static public void Portrait()
        {
            Screen.orientation = ScreenOrientation.Portrait;
        }
    }

    public class FPSCount{}
}