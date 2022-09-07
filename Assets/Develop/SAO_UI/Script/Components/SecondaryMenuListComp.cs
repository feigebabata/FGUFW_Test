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

        protected override void LateUpdate()
        {
            var deltaTime = Time.fixedDeltaTime;

            base.LateUpdate();

            if (_listState == LoopListState.Opening)
            {
                // bool moveEnd = true;
                for (int i = 0; i < _cacheIndex; i++)
                {
                    var item = _itemComps[i];
                    var item_RT = item.transform.AsRT();
                    var y = -_itemSpacing * item.ItemIndex;
                    if (Mathf.Approximately(item_RT.anchoredPosition.y, y)) continue;
                    // moveEnd = false;

                    var speed = Mathf.Pow(item.ItemIndex + 1, 0.25f) * _itemSpacing * _length * 0.35f;
                    y = Mathf.MoveTowards(item_RT.anchoredPosition.y, y, speed * deltaTime);
                    item_RT.anchoredPosition = new Vector2(0, y);
                }
            }
            else if (_listState == LoopListState.Closing)
            {
                for (int i = 0; i < _cacheIndex; i++)
                {
                    var item = _itemComps[i];
                    var item_RT = item.transform.AsRT();
                    var y = - _viewSize;
                    if (Mathf.Approximately(item_RT.anchoredPosition.y, y)) continue;
                    // moveEnd = false;

                    var speed = Mathf.Pow(item.ItemIndex + 1, 0.25f) * _itemSpacing * _length * 1f;
                    y = Mathf.MoveTowards(item_RT.anchoredPosition.y, y, speed * deltaTime);
                    item_RT.anchoredPosition = new Vector2(0, y);
                }
            }
        }

    }
}
