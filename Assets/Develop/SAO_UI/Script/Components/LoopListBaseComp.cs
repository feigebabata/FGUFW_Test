
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using FGUFW;
using System;

namespace SAO_UI
{
    //循环列表 
    public class LoopListBaseComp : UIBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        public float DecelerationRate = 0.135f;
        public float Spacing = 20f;
        public Action<int,LoopListItemBaseComp> OnItemShow,OnItemHide,OnItemSelect,OnItemUnselect;
        protected RectTransform _rectTransform;
        protected float _viewSize,_itemSize,_offset;
        protected float _itemSpacing => _itemSize+Spacing;
        protected int _cacheIndex,_length,_currentIndex=-1;
        protected LoopListState _listState;
        protected LoopListItemBaseComp[] _itemComps;

        private float _velocity,_prevOffset;
        private bool _isDraging;

        protected override void Awake()
        {
            _rectTransform = transform.AsRT();
            _viewSize = _rectTransform.sizeDelta.y;
            _itemSize = _rectTransform.GetChild(0).AsRT().sizeDelta.y;
            // var poive = 
            base.Awake();
        }

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            _listState = LoopListState.Scrolling;
            _isDraging = true;
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            _isDraging = false;
        }

        protected virtual void LateUpdate()
        {

        }

        public virtual void OnClickItem(int itemIndex)
        {
            selectIndexChanged(itemIndex);
            _listState = LoopListState.Select;
        }

        private void selectIndexChanged(int itemIndex)
        {
            var prevIndex = itemIndex;
            for (int i = 0; i < _cacheIndex; i++)
            {
                var itemComp = _itemComps[i];
                if(itemComp.ItemIndex==prevIndex)
                {
                    if(OnItemUnselect!=null)OnItemUnselect(itemComp.ItemIndex.RoundIndex(_length),itemComp);
                }
                if(itemComp.ItemIndex==itemIndex)
                {
                    if(OnItemSelect!=null)OnItemSelect(itemComp.ItemIndex.RoundIndex(_length),itemComp);
                }
            }
            _currentIndex = itemIndex;
        }

        
        public enum LoopListState
        {
            Opening,//依次出现
            // Normal2Select,
            // Select2Normal,
            Select,
            Scrolling,
            MoveToItem,
        }
    }
}