using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace SAO_UI
{
    public class ListItemSpriteComp : MonoBehaviour
    {
        public TexUnit NormalTex;
        public TexUnit SelectTex;
        public TexUnit UnselectTex;
        
        [Serializable]
        public class TexUnit
        {
            public Sprite Tex;
            public Material Mat;
        }
    }

}
