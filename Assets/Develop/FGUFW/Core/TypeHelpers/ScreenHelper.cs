using System;
using UnityEngine;

namespace FGUFW
{
    static public class ScreenHelper
    {
        public static int FPS { get; private set; } = 60;
        private static int fpsCount, second;

        [RuntimeInitializeOnLoadMethod]
        private static void addListenerUpdate()
        {
            PlayerLoopHelper.RemoveToLoop<UnityEngine.PlayerLoop.Update>(updateCallvack);
            PlayerLoopHelper.AddToLoop<UnityEngine.PlayerLoop.Update,object>(updateCallvack);
        }

        private static void updateCallvack()
        {
            fpsCount++;
            if ((int)Time.unscaledTime>second)
            {
                FPS = fpsCount/((int)Time.unscaledTime- second);
                fpsCount = 0;
                second = (int)Time.unscaledTime;
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
}