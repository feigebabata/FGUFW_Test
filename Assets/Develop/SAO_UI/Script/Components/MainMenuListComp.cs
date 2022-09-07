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
            // if(_listState==LoopListState.MoveToItem)
            // {
            //     UnityEditor.EditorApplication.isPaused = true;
            // }
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

        }

    }
}