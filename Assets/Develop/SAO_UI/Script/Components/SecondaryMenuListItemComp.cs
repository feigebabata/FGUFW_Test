using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

namespace SAO_UI
{
    public class SecondaryMenuListItemComp : LoopListItemBaseComp
    {
        public Sprite Unselect,Select;
        public Image Img_BG, Img_Select, Img_Icon;
        public Text Title;
        public CanvasGroup Img_Group;
    }

}
