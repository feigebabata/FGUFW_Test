using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

namespace SAO_UI
{
    public class LoopListItemBaseComp : MonoBehaviour,IPointerClickHandler
    {
        public int ItemIndex = -1;
        public LoopListBaseComp List;

        public void OnPointerClick(PointerEventData eventData)
        {
            List.OnClickItem(ItemIndex);
        }

        public virtual void OnSelect() { }

        public virtual void OnUnselect() { }
    }

}