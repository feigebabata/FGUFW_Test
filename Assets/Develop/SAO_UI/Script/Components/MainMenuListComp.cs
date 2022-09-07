using FGUFW;
using System.Collections;
using UnityEngine;

namespace SAO_UI
{
    public class MainMenuListComp : LoopListBaseComp
    {

        public const float Select_Alpha = 0.9f, Unselect_Alpha = 0.5f;

        protected override void setListState(LoopListState state)
        {
            base.setListState(state);
            if(_listState==LoopListState.Opening)
            {
                for (int i = 0; i < _cacheIndex; i++)
                {
                    var itemComp = _itemComps[i];
                    itemComp.transform.AsRT().anchoredPosition = new Vector2(0, (_cacheIndex - i) * _itemSpacing);
                }
            }
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();

            var deltaTime = Time.fixedDeltaTime;
            if (_listState == LoopListState.Opening)
            {
                bool moveEnd = true;
                for (int i = 0; i < _cacheIndex; i++)
                {
                    var item = _itemComps[i];
                    var item_RT = item.transform.AsRT();
                    var y = -_itemSpacing * item.ItemIndex;
                    if (Mathf.Approximately(item_RT.anchoredPosition.y , y)) continue;
                    moveEnd = false;

                    var speed = Mathf.Pow(item.ItemIndex + 1,0.25f) * _itemSpacing * _length * 0.35f;
                    y = Mathf.MoveTowards(item_RT.anchoredPosition.y, y, speed * deltaTime);
                    item_RT.anchoredPosition = new Vector2(0, y);
                }
                if (moveEnd)
                {
                    OnClickItem(0);
                }
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
                            var item = (MainMenuListItemComp)_itemComps[i];
                            var color = item.Img.color;
                            if (item.Img.sprite != item.Unselect) item.Img.sprite = item.Unselect;
                            if (color.a == 1) continue;
                            float speed = (1 - Select_Alpha) / 0.25f;
                            color.a = Mathf.MoveTowards(color.a, Select_Alpha, speed * deltaTime);
                            item.Img.color = color;
                        }
                    }
                    break;
                case LoopListState.Select:
                    {
                        for (int i = 0; i < _cacheIndex; i++)
                        {
                            var item = (MainMenuListItemComp)_itemComps[i];

                            if (item.ItemIndex == _currentIndex)
                            {
                                if (item.Img.sprite == item.Select)
                                {
                                    var color = item.Img.color;
                                    if (color.a == Select_Alpha) continue;
                                    float speed = (1 - Select_Alpha) / 0.25f;
                                    color.a = Mathf.MoveTowards(color.a, Select_Alpha, speed * deltaTime);
                                    item.Img.color = color;
                                }
                                else
                                {
                                    var color = item.Img.color;
                                    float speed = (1 - Unselect_Alpha) / 0.25f;
                                    color.a = Mathf.MoveTowards(color.a, 1, speed * deltaTime);
                                    item.Img.color = color;
                                    if (color.a == 1) item.Img.sprite = item.Select;
                                }
                            }
                            else
                            {
                                if (item.Img.sprite == item.Unselect)
                                {
                                    var color = item.Img.color;
                                    if (color.a == Unselect_Alpha) continue;
                                    float speed = (1 - Unselect_Alpha) / 0.25f;
                                    color.a = Mathf.MoveTowards(color.a, Unselect_Alpha, speed * deltaTime);
                                    item.Img.color = color;
                                }
                                else
                                {
                                    var color = item.Img.color;
                                    float speed = (1 - Select_Alpha) / 0.25f;
                                    color.a = Mathf.MoveTowards(color.a, 1, speed * deltaTime);
                                    item.Img.color = color;
                                    if (color.a == 1) item.Img.sprite = item.Unselect;
                                }
                            }
                        }
                    }
                    break;
            }

        }

    }
}