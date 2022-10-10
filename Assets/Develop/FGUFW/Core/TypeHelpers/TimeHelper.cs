using System;
using UnityEngine;

namespace FGUFW
{
    static public class TimeHelper
    {
        public static bool UnscaleTimeMode = true;

        public static float Time => UnscaleTimeMode ? UnityEngine.Time.unscaledTime : UnityEngine.Time.time;
    }
}