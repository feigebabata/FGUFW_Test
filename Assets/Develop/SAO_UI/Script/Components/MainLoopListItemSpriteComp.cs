using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

namespace SAO_UI
{
    public class MainLoopListItemSpriteComp : MonoBehaviour,IPointerClickHandler
    {
        public Image Img;
        public Sprite Unselect,Select;
        public int ItemIndex = -1;
        public MainLoopListComp List;

        public void OnPointerClick(PointerEventData eventData)
        {
            List.OnClickItem(ItemIndex);
        }
    }

}
