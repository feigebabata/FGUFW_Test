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
        protected LoopListItemState _prevState = LoopListItemState.None;

        public void OnPointerClick(PointerEventData eventData)
        {
            List.OnClickItem(ItemIndex);
        }

        public virtual void SwitchState(LoopListItemState state)
        {
            _prevState = state;
        }

        public virtual void SetItemIndex(int itemIndex)
        {
            ItemIndex = itemIndex;
            this.name = itemIndex.ToString();
        }

        public enum LoopListItemState
        {
            None,
            Moving,
            Selecting,
            Unselecting
        }
    }

}