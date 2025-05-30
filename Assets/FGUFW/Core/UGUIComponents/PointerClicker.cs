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
        [SerializeField]
        private GameObject m_Data;

        private object _data;

        public T Data<T>()
        {
            if (_data == default)
            {
                return default;
            }
            return (T)_data;
        }

        public void Data(object data)
        {
            _data = data;

            if (data == default)
            {
                m_Data = default;
                return;
            }

            if (data is Component)
            {
                m_Data = (data as Component).gameObject;
            }
            else if (data is GameObject)
            {
                m_Data = data as GameObject;
            }
            else
            {
                m_Data = default;
            }
        }

        public Action<PointerClicker> OnClick;

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick?.Invoke(this);
        }


    }
}
