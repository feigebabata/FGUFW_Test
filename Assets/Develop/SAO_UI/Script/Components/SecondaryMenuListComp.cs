using FGUFW;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SAO_UI
{
    public class SecondaryMenuListComp : LoopListBaseComp
    {//   3.6/1/0.54
        public static readonly Color Select_BG_Color = new Color32(236,166,0,230), Unselect_BG_Color = new Color32(255, 255, 255, 128), Select_Text_Color = new Color32(255, 255, 255, 255), Unselect_Text_Color = new Color32(76, 76, 76, 255);
        public Image Img_Line,Img_Arrow;

        protected override void setListState(LoopListState state)
        {
            base.setListState(state);
            if (_listState == LoopListState.Opening)
            {
                for (int i = 0; i < _cacheIndex; i++)
                {
                    var itemComp = _itemComps[i];
                    itemComp.transform.AsRT().anchoredPosition = new Vector2(0, (_cacheIndex - i) * _itemSpacing);
                }
            }
        }

        protected override int getIndexByOffset()
        {
            int index = 0;
            var size = _itemSpacing;
            var offset = _offset + _itemSize / 2 + _viewSize/2;
            index = (offset / size).Ceil_Z();
            return index;
        }

        protected override void LateUpdate()
        {
            var deltaTime = Time.fixedDeltaTime;
            if (_listState == LoopListState.Scrolling)
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
                        onResetOffsetAndSelectIndex();
                        setListState(LoopListState.Select);
                    }
                }
                return;
            }

            base.LateUpdate();

            if (_listState == LoopListState.Opening)
            {
                bool moveEnd = true;
                for (int i = 0; i < _cacheIndex; i++)
                {
                    var item = _itemComps[i];
                    var item_RT = item.transform.AsRT();
                    var y = -_itemSpacing * item.ItemIndex;
                    if (Mathf.Approximately(item_RT.anchoredPosition.y, y)) continue;
                    moveEnd = false;

                    var speed = Mathf.Pow(item.ItemIndex + 1, 0.25f) * _itemSpacing * _length * 0.35f;
                    y = Mathf.MoveTowards(item_RT.anchoredPosition.y, y, speed * deltaTime);
                    item_RT.anchoredPosition = new Vector2(0, y);
                }
                //if (moveEnd)
                //{
                //    OnClickItem(0);
                //}
            }
            updateItemRender();
        }

        private void updateItemRender()
        {
            float deltaTime = Time.fixedDeltaTime;

            switch (_listState)
            {
                case LoopListState.Opening:
                case LoopListState.Scrolling:
                case LoopListState.MoveToItem:
                    {
                        for (int i = 0; i < _cacheIndex; i++)
                        {
                            var item = (SecondaryMenuListItemComp)_itemComps[i];
                            var bg_color = item.Img_BG.color;
                            if (item.Img_Icon.sprite != item.Unselect) item.Img_Icon.sprite = item.Unselect;
                            if (item.Img_Group.alpha == 1) continue;
                            float speed = (1 - Unselect_BG_Color.a) / 0.25f;
                            item.Img_Group.alpha = Mathf.MoveTowards(item.Img_Group.alpha, Select_BG_Color.a, speed * deltaTime);
                            item.Img_BG.color = bg_color;
                            bg_color = Select_BG_Color;
                            bg_color.a = 1 - bg_color.a;
                            item.Img_Select.color = bg_color;
                        }
                    }
                    break;
                case LoopListState.Select:
                    {
                        for (int i = 0; i < _cacheIndex; i++)
                        {
                            var item = (SecondaryMenuListItemComp)_itemComps[i];

                            if (item.ItemIndex == _currentIndex)
                            {
                                if (item.Img_Icon.sprite == item.Select)
                                {
                                    var bg_color = item.Img_BG.color;
                                    if (bg_color.a == Select_BG_Color.a) continue;
                                    float speed = (1 - Select_BG_Color.a) / 0.25f;
                                    item.Img_Group.alpha = Mathf.MoveTowards(item.Img_Group.alpha, Select_BG_Color.a, speed * deltaTime);
                                    
                                }
                                else
                                {
                                    var bg_color = item.Img_BG.color;
                                    float speed = (1 - Unselect_BG_Color.a) / 0.25f;
                                    item.Img_Group.alpha = Mathf.MoveTowards(item.Img_Group.alpha, 1, speed * deltaTime);

                                    if (item.Img_Group.alpha == 1)
                                    {
                                        item.Img_Icon.sprite = item.Select;
                                        bg_color = Select_BG_Color;
                                        bg_color.a = 1;
                                        item.Img_BG.color = bg_color;
                                        item.Img_Select.color = bg_color;
                                        item.Title.color = Select_Text_Color;
                                    }
                                }
                            }
                            else
                            {
                                if (item.Img_Icon.sprite == item.Unselect)
                                {
                                    var color = item.Img_BG.color;
                                    if (color.a == Unselect_BG_Color.a) continue;
                                    float speed = (1 - Unselect_BG_Color.a) / 0.25f;
                                    item.Img_Group.alpha = Mathf.MoveTowards(item.Img_Group.alpha, Unselect_BG_Color.a, speed * deltaTime);
                                }
                                else
                                {
                                    var color = item.Img_BG.color;
                                    float speed = (1 - Select_BG_Color.a) / 0.25f;
                                    item.Img_Group.alpha = Mathf.MoveTowards(item.Img_Group.alpha, 1, speed * deltaTime);
                                    color.a = 1 - color.a;
                                    item.Img_Select.color = color;
                                    if (item.Img_Group.alpha == 1)
                                    {
                                        item.Img_Icon.sprite = item.Unselect;
                                        item.Img_BG.color = new Color32(255, 255, 255, 255);
                                        item.Img_Select.color = new Color32(255, 255, 255, 255);
                                        item.Title.color = Unselect_Text_Color;
                                    }
                                }
                            }
                        }
                    }
                    break;
            }



        }

        protected override float getTargetOffsetByMoveToItem()
        {
            return _currentIndex * _itemSpacing - _viewSize/2 + _itemSize/2;
        }

    }
}
