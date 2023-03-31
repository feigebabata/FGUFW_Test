using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGUFW.Entities;

namespace RogueGamePlay
{
    public class AnimationDatasAuthoring : MonoBehaviour
    {
        public Animation[] Animations;

        [System.Serializable]
        public class Animation
        {
            public int AnimationID;
            public MaterialFlipbookAnimation Anim;
            public MaterialFlipbookAnimEventData[] Events;
        }    
    }

    

}
