using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FGUFW
{
    [AddComponentMenu("UI/PointerClicker", 10)]
    [DisallowMultipleComponent]
    public class PointerClicker : AutoRefComponent, IPointerClickHandler
    {
        public object Data;
        public Action<PointerClicker> OnClick;

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick?.Invoke(this);
        }
    }
}
