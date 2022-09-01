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
    public class MainLoopListComp : UIBehaviour, IInitializePotentialDragHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        public float DecelerationRate = 0.135f;
        public float Spacing = 20f;
        public float Select_Alpha=0.9f,Unselect_Alpha=0.5f;
        private int _currentIndex=-1;
        private State _listState;
        private float _itemSize;
        private RectTransform _viewRect;
        private float _viewSize;

        private float _velocity;//衰减: _velocity *= Mathf.Pow(DecelerationRate, deltaTime); 
        private float _offset;
        private IListBehaviour _listBehaviour;
        // private List<MainLoopListItemSpriteComp> _itemList = new List<MainLoopListItemSpriteComp>();

        private MainLoopListItemSpriteComp[] _itemComps;

        public int Length{get;private set;}

        public State ListState 
        {   
            get => _listState; 
            set
            {
                _listState = value;
                switch (value)
                {
                    case State.Opening:
                        _listBehaviour = new OpeningBehaviour();
                    break;
                    case State.MoveToItem:
                        _listBehaviour = new MoveToItemBehaviour();
                    break;
                    case State.Scrolling:
                        _listBehaviour = new ScrollingBehaviour();
                    break;
                    case State.Select:
                        _listBehaviour = new SelectBehaviour();
                    break;
                }
                _listBehaviour.Init(this);
            } 
        }

        public float Offset 
        { 
            get => _offset; 
            set
            {
                _offset = value;
            } 
        }

        public Action<int,MainLoopListItemSpriteComp> OnItemSelect,OnItemUnselect,OnItemShow,OnItemHide;
        private List<int> _viewItemIndexs = new List<int>();
        private int _cacheIndex;



        //------------------------------------

        protected override void Awake()
        {
            _viewRect = transform.AsRT();
            _itemSize = transform.GetChild(0).transform.AsRT().sizeDelta.y;
            _viewSize = _viewRect.sizeDelta.y;
            base.Awake();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            ListState = State.Scrolling;
            _listBehaviour.OnBeginDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            _listBehaviour.OnDrag(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _listBehaviour.OnEndDrag(eventData);
        }

        public void OnInitializePotentialDrag(PointerEventData eventData)
        {
            _listBehaviour.OnInitializePotentialDrag(eventData);
        }

        protected virtual void LateUpdate()
        {
            _listBehaviour.LateUpdate();
            if(Length>0)updateItemRender();
        }

        public int GetOffsetIndex()
        {
            int index = 0;
            var size = _itemSize+Spacing;
            var offset = Offset+_itemSize/2;
            index = (offset/size).Ceil_Z();
            return index;
        }

        public Vector2 GetIndexPosition(int index)
        {
            var pos = Vector2.zero;
            float size = _itemSize+Spacing;
            pos.y = Offset-size*index;
            return pos;
        }

        public List<int> getViewItemIndexs()
        {
            _viewItemIndexs.Clear();
            float itemSize = _itemSize+Spacing;
            
            float offset = Offset;
            int fristIndex = (offset/itemSize).Ceil_Z();
            
            for (int i = fristIndex;; i++)
            {
                float y = - i*itemSize + offset ;
                if(y<-_viewSize)break;
                _viewItemIndexs.Add(i);
            }
            return _viewItemIndexs;
        }

        private void updateItemRender()
        {
            float deltaTime = Time.fixedDeltaTime;
            if(ListState==State.Select)
            {
                for (int i = 0; i < _cacheIndex; i++)
                {   
                    var item = _itemComps[i];
    
                    if(item.ItemIndex==_currentIndex)
                    {
                        if(item.Img.sprite==item.Select)
                        {
                            var color = item.Img.color;
                            if(color.a==Select_Alpha)continue;
                            float speed = (1-Select_Alpha)/0.25f;
                            color.a = Mathf.MoveTowards(color.a,Select_Alpha,speed*deltaTime);
                            item.Img.color = color;
                        }
                        else
                        {
                            var color = item.Img.color;
                            float speed = (1-Unselect_Alpha)/0.25f;
                            color.a = Mathf.MoveTowards(color.a,1,speed*deltaTime);
                            item.Img.color = color;
                            if(color.a==1)item.Img.sprite = item.Select;
                        }
                    }
                    else
                    {
                        if(item.Img.sprite==item.Unselect)
                        {
                            var color = item.Img.color;
                            if(color.a==Unselect_Alpha)continue;
                            float speed = (1-Unselect_Alpha)/0.25f;
                            color.a = Mathf.MoveTowards(color.a,Unselect_Alpha,speed*deltaTime);
                            item.Img.color = color;
                        }
                        else
                        {
                            var color = item.Img.color;
                            float speed = (1-Select_Alpha)/0.25f;
                            color.a = Mathf.MoveTowards(color.a,1,speed*deltaTime);
                            item.Img.color = color;
                            if(color.a==1)item.Img.sprite = item.Unselect;
                        }
                    }
                }
            }
            else if(ListState==State.Opening || ListState==State.Scrolling || ListState==State.MoveToItem)
            {
                for (int i = 0; i < _cacheIndex; i++)
                {   
                    var item = _itemComps[i];
                    var color = item.Img.color;
                    if(item.Img.sprite != item.Unselect)item.Img.sprite = item.Unselect;
                    if(color.a==1)continue;
                    float speed = (1-Select_Alpha)/0.25f;
                    color.a = Mathf.MoveTowards(color.a,Select_Alpha,speed*deltaTime);
                    item.Img.color = color;
                }
            }
            else
            {

            }

        }

        public void ResetAllItemPosition()
        {
            OnMoveCheckList();
            float offset = Offset;//%_viewSize;
            for (int i = 0; i < _cacheIndex; i++)
            {
                var item = _itemComps[i];
                int itemIndex = item.ItemIndex;
                RectTransform item_RT = item.transform.AsRT();

                item_RT.anchoredPosition = GetIndexPosition(itemIndex);
            }
        }

        public void OnMoveCheckList()
        {
            var viewList = getViewItemIndexs();
            // _cacheIndex = Length;
            for (int i = _cacheIndex-1; i >= 0; i--)
            {
                var item = _itemComps[i];
                if(!viewList.Exists(idx=>idx==item.ItemIndex))
                {
                    _cacheIndex--;
                    var temp = _itemComps[_cacheIndex];
                    _itemComps[_cacheIndex]=item;
                    _itemComps[i] = temp;
                    item.gameObject.SetActive(false);
                    
                    if(OnItemHide!=null)
                    {
                        OnItemHide(item.ItemIndex.RoundIndex(Length),item);
                    }
                }
            }
            foreach (var item_index in viewList)
            {
                if(_itemComps.IndexOf<MainLoopListItemSpriteComp>(item=>item.ItemIndex==item_index,0,_cacheIndex)==-1)
                {
                    var newItem = _itemComps[_cacheIndex];
                    _cacheIndex++;
                    newItem.ItemIndex = item_index;
                    newItem.gameObject.SetActive(true);

                    if(OnItemShow!=null)
                    {
                        OnItemShow(newItem.ItemIndex.RoundIndex(Length),newItem);
                    }
                }
            }
        }

        internal void OnClickItem(int itemIndex)
        {
            IndexChanged(itemIndex);
            this.ListState = State.MoveToItem;
        }

        public void IndexChanged(int itemIndex)
        {
            int prevIndex = _currentIndex;
            _currentIndex = itemIndex;
            for (int i = 0; i < _cacheIndex; i++)
            {
                var item = _itemComps[i];
                var index = item.ItemIndex%Length;
                if(index==itemIndex)
                {
                    if(OnItemSelect!=null)
                    {
                        OnItemSelect(itemIndex.RoundIndex(Length),item);
                    }
                }
                if(index==prevIndex)
                {
                    if(OnItemUnselect!=null)
                    {
                        OnItemUnselect(itemIndex.RoundIndex(Length),item);
                    }
                }
            }
        }

        public void Init(int length)
        {
            Length = length;
            var itemsSize = (_itemSize+Spacing)*Length;
            if(itemsSize<_viewSize)
            {
                var sizeDelta = _viewRect.sizeDelta;
                sizeDelta.y = itemsSize;
                _viewRect.sizeDelta = sizeDelta;
                _viewSize = itemsSize;
            }
            _itemComps = new MainLoopListItemSpriteComp[Length+1];
            var itemTemp = transform.GetChild(0);
            for (int i = 0; i < length; i++)
            {
                var item = itemTemp.gameObject.Copy(transform).transform.AsRT();
                var comp = item.GetComponent<MainLoopListItemSpriteComp>();
                _itemComps[i]=comp;
                comp.ItemIndex = i;
                comp.List = this;
                if(OnItemShow!=null)
                {
                    OnItemShow(i.RoundIndex(Length),comp);
                }
            }
            _itemComps[Length]=itemTemp.GetComponent<MainLoopListItemSpriteComp>();
            _cacheIndex = Length;
            ListState = State.Opening;
        }

        public interface IListBehaviour : IInitializePotentialDragHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
        {
            void LateUpdate();
            void Init(MainLoopListComp list);
        }

        public class OpeningBehaviour : IListBehaviour
        {
            MainLoopListComp _list;

            public void Init(MainLoopListComp list)
            {
                _list = list;

                for (int i = 0; i < _list.Length; i++)
                {   
                    var item = _list._itemComps[i];
                    item.transform.AsRT().anchoredPosition = new Vector2(0,_list._viewSize/*(_list._itemSize+_list.Spacing)*4*/);
                }
            }

            public void LateUpdate()
            {
                var deltaTime = Time.fixedDeltaTime;
                // _list.Offset = Mathf.MoveTowards(_list.Offset,0,speed*deltaTime);
                // _list.ResetAllItemPosition();
                bool moveEnd = true;
                for (int i = 0; i < _list.Length; i++)
                {   
                    var item = _list._itemComps[i];
                    var item_RT = item.transform.AsRT();
                    var y = -(_list._itemSize+_list.Spacing)*item.ItemIndex;
                    if(item_RT.anchoredPosition.y==y)continue;
                    moveEnd = false;
                    var speed = (item.ItemIndex+1) * _list._viewSize*0.25f;
                    y = Mathf.MoveTowards(item_RT.anchoredPosition.y,y,speed*deltaTime);
                    item_RT.anchoredPosition = new Vector2(0,y);
                }
                if(moveEnd)
                {
                    _list.OnClickItem(0);
                }
            }

            public void OnBeginDrag(PointerEventData eventData)
            {
                
            }

            public void OnDrag(PointerEventData eventData)
            {
                
            }

            public void OnEndDrag(PointerEventData eventData)
            {
                
            }

            public void OnInitializePotentialDrag(PointerEventData eventData)
            {
                
            }
        }

        public class SelectBehaviour : IListBehaviour
        {
            MainLoopListComp _list;

            public void Init(MainLoopListComp list)
            {
                _list = list;
            }

            public void LateUpdate()
            {
                
            }

            public void OnBeginDrag(PointerEventData eventData)
            {
                
            }

            public void OnDrag(PointerEventData eventData)
            {
                
            }

            public void OnEndDrag(PointerEventData eventData)
            {
                
            }

            public void OnInitializePotentialDrag(PointerEventData eventData)
            {
                
            }
        }

        public class ScrollingBehaviour : IListBehaviour
        {
            MainLoopListComp _list;
            bool _isDraging = true;
            private float _prevPosition;

            public void Init(MainLoopListComp list)
            {
                _list = list;
                _list.IndexChanged(-1);
            }

            public void LateUpdate()
            {
                float deltaTime = Time.fixedDeltaTime;
                if(_isDraging)
                {
                    var newVelocity = (_list.Offset - _prevPosition) / deltaTime;
                    _list._velocity = Mathf.Lerp(_list._velocity, newVelocity, deltaTime * 10);
                    _prevPosition = _list.Offset;
                }
                else
                {
                    float offset = deltaTime * _list._velocity;
                    _list._velocity *= Mathf.Pow(_list.DecelerationRate, deltaTime); 
                    if(Mathf.Abs(_list._velocity)<1)
                    {
                        _list.IndexChanged(_list.GetOffsetIndex());
                        _list.ListState = State.MoveToItem;
                    }
                    _list.Offset += offset;
                    _list.ResetAllItemPosition();
                }
            }

            public void OnBeginDrag(PointerEventData eventData)
            {
                _prevPosition = _list.Offset;
            }

            public void OnDrag(PointerEventData eventData)
            {
                var delta = eventData.delta.y;
                _list.Offset+=delta;
                _list.ResetAllItemPosition();
            }

            public void OnEndDrag(PointerEventData eventData)
            {
                _isDraging = false;
            }

            public void OnInitializePotentialDrag(PointerEventData eventData)
            {
                
            }
        }

        public class MoveToItemBehaviour : IListBehaviour
        {
            MainLoopListComp _list;
            private float _targetOffset;
            private float _speed;

            public void Init(MainLoopListComp list)
            {
                _list = list;
                _targetOffset = _list._currentIndex * (_list._itemSize+_list.Spacing);
                var space = MathHelper.Distance(_targetOffset,_list.Offset);
                float time = Mathf.Lerp(0.5f,1f,space/_list._viewSize);
                _speed = space/time;
                // Debug.Log($"{time}  {_targetOffset}");
            }

            public void LateUpdate()
            {
                float deltaTime = Time.fixedDeltaTime;
                _list.Offset = Mathf.MoveTowards(_list.Offset,_targetOffset,_speed*deltaTime);
                _list.ResetAllItemPosition();
                if(_list.Offset==_targetOffset)
                {
                    _list.OnResetOffsetAndSelectIndex();
                    _list.ListState = State.Select;
                }
            }

            public void OnBeginDrag(PointerEventData eventData)
            {
                
            }

            public void OnDrag(PointerEventData eventData)
            {
                
            }

            public void OnEndDrag(PointerEventData eventData)
            {
                
            }

            public void OnInitializePotentialDrag(PointerEventData eventData)
            {
                
            }
        }

        private void OnResetOffsetAndSelectIndex()
        {
            if(Offset==0)return;
            var itemsSize = (_itemSize+Spacing)*Length;
            int round = (int)(Offset/itemsSize);
            _currentIndex-=round*Length;
            Offset-=itemsSize*round;
            for (int i = 0; i < _cacheIndex; i++)
            {
                _itemComps[i].ItemIndex-=round*Length;
            }
        }

        public enum State
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
