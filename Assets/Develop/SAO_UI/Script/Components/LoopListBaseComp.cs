
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
        private const int UNSELECT_INDEX = int.MaxValue;
        public float DecelerationRate = 0.135f;
        public float Spacing = 20f;
        public bool AutoSelect=false;
        public float MoveToItemPivot = 0;
        public Action<int, LoopListItemBaseComp> OnItemShow, OnItemHide, OnItemSelect, OnItemUnselect;
        protected RectTransform _rectTransform;
        protected float _viewSize, _itemSize, _offset, _velocity, _prevOffset;
        protected float _itemSpacing => _itemSize + Spacing;
        protected int _cacheIndex,_length, _currentIndex = UNSELECT_INDEX,_moveToItemIndex;
        protected LoopListState _listState = LoopListState.None;
        protected LoopListItemBaseComp[] _itemComps;

        protected bool _isDraging;
        private List<int> _viewItemIndexs = new List<int>();

        protected override void Awake()
        {
            _rectTransform = transform.AsRT();
            _viewSize = _rectTransform.sizeDelta.y;
            var itemRT = _rectTransform.GetChild(0).AsRT();
            _itemSize = itemRT.sizeDelta.y;

            base.Awake();
        }

        public virtual void Init(int length)
        {
            _length = length;
            var itemsSize = _itemSpacing * _length;
            if (itemsSize < _viewSize)
            {
                var sizeDelta = _rectTransform.sizeDelta;
                sizeDelta.y = itemsSize;
                _rectTransform.sizeDelta = sizeDelta;
                _viewSize = itemsSize;
            }
            
            var itemTemp = transform.GetChild(0);
            if(_itemComps==null)
            {
                _itemComps = new LoopListItemBaseComp[_length + 1];
                _itemComps[0] = itemTemp.GetComponent<LoopListItemBaseComp>();
            }
            else if(_itemComps.Length<_length+1)
            {
                var newItemComps = new LoopListItemBaseComp[_length+1];
                Array.Copy(_itemComps,newItemComps,newItemComps.Length-_itemComps.Length);
                _itemComps = newItemComps;
            }
            
            _cacheIndex = _length;

            for (int i = 0; i < _itemComps.Length; i++)
            {
                LoopListItemBaseComp comp = _itemComps[i];
                if(comp==null)
                {
                    comp = itemTemp.gameObject.Copy(transform).GetComponent<LoopListItemBaseComp>();
                    _itemComps[i] = comp;
                }
                setItemIndex(comp,i);
                comp.List = this;
                if(i<_cacheIndex)
                {
                    onItemShow(i.RoundIndex(_length), comp);
                }
                else
                {
                    comp.gameObject.SetActive(false);
                }
            }

            setListState(LoopListState.Opening);
        }

        public virtual void Close()
        {
            setListState(LoopListState.Closing);
            selectIndexChanged(UNSELECT_INDEX);
        }

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            setListState(LoopListState.Scrolling);
            _isDraging = true;
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            if (_listState != LoopListState.Scrolling) return;
            if (!_isDraging) return;
            _offset += eventData.delta.y;
            resetItemsPosition();
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            _isDraging = false;
        }

        protected virtual void LateUpdate()
        {
            var deltaTime = Time.fixedDeltaTime;
            switch (_listState)
            {
                case LoopListState.Opening:
                    break;
                case LoopListState.Select:
                    break;
                case LoopListState.Scrolling:
                    {
                        if (_isDraging)
                        {
                            var newVelocity = (_offset - _prevOffset) / deltaTime;
                            _velocity = Mathf.Lerp(_velocity, newVelocity, deltaTime * 10);
                            _prevOffset = _offset;
                        }
                        else
                        {
                            float offset = _velocity * deltaTime;
                            _velocity *= Mathf.Pow(DecelerationRate, deltaTime);

                            _offset += offset;
                            resetItemsPosition();
                            if (Mathf.Abs(_velocity) < 1)
                            {
                                _velocity = 0;
                                if(AutoSelect)
                                {
                                    _moveToItemIndex = getIndexByOffset();

                                    setListState(LoopListState.MoveToItem);
                                }
                                else
                                {
                                    onResetOffsetAndSelectIndex();
                                    // setListState(LoopListState.Select);
                                }
                            }
                        }
                    }
                    break;
                case LoopListState.MoveToItem:
                    {
                        var targetOffset = getTargetOffsetByMoveToItem();
                        _offset = Mathf.MoveTowards(_offset, targetOffset, _velocity * deltaTime);
                        resetItemsPosition();
                        if (_offset == targetOffset)
                        {
                            selectIndexChanged(_moveToItemIndex);
                            onResetOffsetAndSelectIndex();
                            setListState(LoopListState.Select);
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 重置offset和selectIndex 避免一直累加
        /// </summary>
        protected void onResetOffsetAndSelectIndex()
        {
            if (_offset == 0) return;
            var itemsSize = _itemSpacing * _length;
            int round = (int)(_offset / itemsSize);
            _currentIndex -= round * _length;
            _offset -= itemsSize * round;
            for (int i = 0; i < _cacheIndex; i++)
            {
                _itemComps[i].ItemIndex -= round * _length;
            }
        }

        protected virtual void setListState(LoopListState state)
        {
            _listState = state;
            //状态逻辑初始化
            switch (_listState)
            {
                case LoopListState.Opening:
                    {
                        allItemMoveState();
                    }
                    break;
                case LoopListState.Select:
                    break;
                case LoopListState.Closing:
                case LoopListState.Scrolling:
                    {
                        selectIndexChanged(UNSELECT_INDEX);
                        allItemMoveState();
                    }
                    break;
                case LoopListState.MoveToItem:
                    {
                        var targetOffset = getTargetOffsetByMoveToItem();
                        var space = MathHelper.Distance(targetOffset, _offset);
                        float time = Mathf.Lerp(0.5f, 1f, space / _viewSize);
                        _velocity = space / time;
                        
                        allItemMoveState();
                    }
                    break;
                default:
                    break;
            }
        }

        private void allItemMoveState()
        {
            int length = _cacheIndex;
            for (int i = 0; i < length; i++)
            {
                var item = _itemComps[i];
                item.SwitchState(LoopListItemBaseComp.LoopListItemState.Moving);
            }
        }

        protected virtual int getIndexByOffset()
        {
            int index = 0;
            var size = _itemSpacing;
            var pivot = Mathf.Clamp01(MoveToItemPivot);
            var offset = _offset + _itemSize / 2 + (_viewSize*pivot) - (_itemSpacing*pivot);
            index = Mathf.FloorToInt((offset / size));
            return index;
        }

        protected virtual float getTargetOffsetByMoveToItem()
        {
            var pivot = Mathf.Clamp01(MoveToItemPivot);
            return _moveToItemIndex * _itemSpacing - (pivot*_viewSize)+(_itemSpacing*pivot);
        }

        public virtual void OnClickItem(int itemIndex)
        {
            _moveToItemIndex = itemIndex;

            setListState(LoopListState.MoveToItem);
        }

        protected void resetItemsPosition()
        {
            onMoveCheckList();
            for (int i = 0; i < _cacheIndex; i++)
            {
                var item = _itemComps[i];
                int itemIndex = item.ItemIndex;
                RectTransform item_RT = item.transform.AsRT();

                item_RT.anchoredPosition = getIndexPosition(itemIndex);
            }
        }

        protected Vector2 getIndexPosition(int itemIndex)
        {
            var pos = Vector2.zero;
            float size = _itemSpacing;
            pos.y = _offset - size * itemIndex;
            return pos;
        }

        private void onMoveCheckList()
        {
            var viewList = getViewItemIndexs();
            // _cacheIndex = Length;
            for (int i = _cacheIndex - 1; i >= 0; i--)
            {
                var item = _itemComps[i];
                if (!viewList.Exists(idx => idx == item.ItemIndex))
                {
                    _cacheIndex--;
                    var temp = _itemComps[_cacheIndex];
                    _itemComps[_cacheIndex] = item;
                    _itemComps[i] = temp;
                    item.gameObject.SetActive(false);

                    onItemHide(item.ItemIndex.RoundIndex(_length), item);
                }
            }
            // Debug.Log(_cacheIndex);
            foreach (var item_index in viewList)
            {
                if (_itemComps.IndexOf<LoopListItemBaseComp>(item => item.ItemIndex == item_index, 0, _cacheIndex) == -1)
                {
                    var newItem = _itemComps[_cacheIndex];
                    _cacheIndex++;
                    setItemIndex(newItem,item_index);
                    newItem.gameObject.SetActive(true);

                    onItemShow(newItem.ItemIndex.RoundIndex(_length), newItem);
                }
            }
        }

        protected virtual void onItemShow(int itemIndex, LoopListItemBaseComp comp)
        {
            comp.name = itemIndex.ToString();
            if(OnItemShow!=null)OnItemShow(itemIndex, comp);
        }

        protected virtual void onItemHide(int itemIndex, LoopListItemBaseComp comp)
        {
            comp.name = "cache";
            if (OnItemHide != null) OnItemHide(itemIndex, comp);
        }

        protected virtual void onItemSelect(int itemIndex, LoopListItemBaseComp comp)
        {
            if (OnItemSelect != null) OnItemSelect(itemIndex, comp);
            comp.SwitchState(LoopListItemBaseComp.LoopListItemState.Selecting);

        }

        protected virtual void onItemUnselect(int itemIndex, LoopListItemBaseComp comp)
        {
            if (OnItemUnselect != null) OnItemUnselect(itemIndex, comp);
            comp.SwitchState(LoopListItemBaseComp.LoopListItemState.Unselecting);
        }

        private List<int> getViewItemIndexs()
        {
            _viewItemIndexs.Clear();
            float itemSize = _itemSpacing;

            int fristIndex = Mathf.FloorToInt(_offset / itemSize);

            for (int i = fristIndex; ; i++)
            {
                float y = -i * itemSize + _offset;
                if (y < -_viewSize) break;
                _viewItemIndexs.Add(i);
            }
            return _viewItemIndexs;
        }

        private void selectIndexChanged(int itemIndex)
        {
            // var prevIndex = _currentIndex;
            for (int i = 0; i < _cacheIndex; i++)
            {
                var itemComp = _itemComps[i];
                // if (itemComp.ItemIndex == prevIndex)
                // {
                //     onItemUnselect(itemComp.ItemIndex.RoundIndex(_length), itemComp);
                // }
                if (itemComp.ItemIndex == itemIndex)
                {
                    onItemSelect(itemComp.ItemIndex.RoundIndex(_length), itemComp);
                }
                else
                {
                    onItemUnselect(itemComp.ItemIndex.RoundIndex(_length), itemComp);
                }
            }
            _currentIndex = itemIndex;
        }

        private void setItemIndex(LoopListItemBaseComp comp,int itemIndex)
        {
            comp.ItemIndex = itemIndex;
            comp.name = itemIndex.ToString();
        }


        public enum LoopListState
        {
            Opening,//依次出现
            // Normal2Select,
            // Select2Normal,
            Select,
            Scrolling,
            MoveToItem,
            None,
            Closing
        }
    }
}