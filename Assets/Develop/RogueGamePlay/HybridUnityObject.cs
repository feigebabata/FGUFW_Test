using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGUFW;

namespace RogueGamePlay
{
    public class HybridUnityObject : MonoSingleton<HybridUnityObject>
    {
        public Transform[] ViewFollowItems;

        protected override bool IsDontDestroyOnLoad()
        {
            return false;
        }
    }
}
