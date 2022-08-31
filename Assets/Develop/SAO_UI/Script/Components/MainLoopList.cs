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
    public class MainLoopList : UIBehaviour, IInitializePotentialDragHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        public float DecelerationRate = 0.135f;
        public float Spacing = 20f;
        public float Select_Alpha=0.9f,Unselect_Alpha=0.5f;
        private int _currentIndex=-1;
        private State _listState;
        private float _itemSize;
        private RectTransform _viewRect;
        private float _viewSize;

        private float _velocity;//衰减: _velocity = Mathf.Pow(DecelerationRate, deltaTime); 
        private float _offset;
        private IListBehaviour _listBehaviour;
        private List<ListItemSpriteComp> _itemList = new List<ListItemSpriteComp>();

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

        public Action<int,ListItemSpriteComp> OnItemSelect,OnItemUnselect,OnItemCreate;
        private List<int> _viewItemIndexs;



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
            if(_itemList.Count>0)updateItemRender();
        }

        public int GetOffsetIndex()
        {
            int index = 0;
            int size = (int)(_itemSize+Spacing);
            int offset = (int)Offset;
            index = offset/size;
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
            float itemsSize = itemSize*Length;
            float offset = Offset;
            int fristIndex = Mathf.CeilToInt(offset/itemSize)%Length;
            offset %= itemsSize;
            
            for (int i = fristIndex; i < Length; i++)
            {
                float y = offset - i*itemSize;
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
                foreach (var item in _itemList)
                {
    
                    if(item.ItemIndex==_currentIndex)
                    {
                        if(item.Img.sprite==item.Select)
                        {
                            var color = item.Img.color;
                            if(color.a==Select_Alpha)continue;
                            float speed = (1-Select_Alpha)/0.5f;
                            color.a = Mathf.MoveTowards(color.a,Select_Alpha,speed*deltaTime);
                            item.Img.color = color;
                        }
                        else
                        {
                            var color = item.Img.color;
                            float speed = (1-Unselect_Alpha)/0.5f;
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
                            float speed = (1-Unselect_Alpha)/0.5f;
                            color.a = Mathf.MoveTowards(color.a,Unselect_Alpha,speed*deltaTime);
                            item.Img.color = color;
                        }
                        else
                        {
                            var color = item.Img.color;
                            float speed = (1-Select_Alpha)/0.5f;
                            color.a = Mathf.MoveTowards(color.a,1,speed*deltaTime);
                            item.Img.color = color;
                            if(color.a==1)item.Img.sprite = item.Unselect;
                        }
                    }
                }
            }
            else if(ListState==State.Opening || ListState==State.Scrolling || ListState==State.MoveToItem)
            {
                foreach (var item in _itemList)
                {
                    var color = item.Img.color;
                    if(item.Img.sprite != item.Unselect)item.Img.sprite = item.Unselect;
                    if(color.a==1)continue;
                    float speed = (1-Select_Alpha)/0.5f;
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
            float offset = Offset;//%_viewSize;
            foreach (var item in _itemList)
            {
                int itemIndex = item.ItemIndex;
                RectTransform item_RT = item.transform.AsRT();

                item_RT.anchoredPosition = GetIndexPosition(itemIndex);
            }
        }

        internal void OnClickItem(int itemIndex)
        {
            Debug.Log(itemIndex);
            if(itemIndex==-1)return;
            _currentIndex = itemIndex;
            foreach (var item in _itemList)
            {
                if(item.ItemIndex==itemIndex)
                {
                    if(OnItemSelect!=null)
                    {
                        OnItemSelect(itemIndex,item);
                    }
                }
                else
                {
                    if(OnItemUnselect!=null)
                    {
                        OnItemUnselect(itemIndex,item);
                    }
                }
            }
            this.ListState = State.MoveToItem;
        }

        public void Init(int length)
        {
            Length = length;
            var itemTemp = transform.GetChild(0);
            for (int i = 0; i < length; i++)
            {
                var item = itemTemp.gameObject.Copy(transform).transform.AsRT();
                var comp = item.GetComponent<ListItemSpriteComp>();
                _itemList.Add(comp);
                comp.ItemIndex = i;
                comp.List = this;
                if(OnItemCreate!=null)
                {
                    OnItemCreate(i,comp);
                }
            }
            ListState = State.Opening;
        }

        public interface IListBehaviour : IInitializePotentialDragHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
        {
            void LateUpdate();
            void Init(MainLoopList list);
        }

        public class OpeningBehaviour : IListBehaviour
        {
            MainLoopList _list;

            public void Init(MainLoopList list)
            {
                _list = list;

                foreach (var item in list._itemList)
                {
                    item.transform.AsRT().anchoredPosition = new Vector2(0,_list._viewSize/*(_list._itemSize+_list.Spacing)*4*/);
                }
            }

            public void LateUpdate()
            {
                var deltaTime = Time.fixedDeltaTime;
                // _list.Offset = Mathf.MoveTowards(_list.Offset,0,speed*deltaTime);
                // _list.ResetAllItemPosition();
                bool moveEnd = true;
                foreach (var item in _list._itemList)
                {
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
            MainLoopList _list;

            public void Init(MainLoopList list)
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
            MainLoopList _list;
            bool _isDraging = true;
            private float _prevPosition;

            public void Init(MainLoopList list)
            {
                _list = list;
                _list._currentIndex =-1;
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
                        _list._currentIndex = _list.GetOffsetIndex();
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
            MainLoopList _list;

            public void Init(MainLoopList list)
            {
                _list = list;
            }

            public void LateUpdate()
            {
                var targetOffset = _list._currentIndex * (_list._itemSize+_list.Spacing);
                float speed = _list._viewSize/1.5f;
                float deltaTime = Time.fixedDeltaTime;
                _list.Offset = Mathf.MoveTowards(_list.Offset,targetOffset,speed*deltaTime);
                _list.ResetAllItemPosition();
                if(_list.Offset==targetOffset)
                {
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
