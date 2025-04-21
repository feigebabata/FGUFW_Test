using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace FGUFW
{
    /// <summary>
    /// 在Editor自动绑定组件字段
    /// </summary>
    public abstract class AutoRefComponent : MonoBehaviour
    {
        /// <summary>
        /// Reset is called when the user hits the Reset button in the Inspector's
        /// context menu or when adding the component the first time.
        /// </summary>
        void Reset()
        {
            this.AutoRefField();
        }

        public void TryAddAllBtnListener(MonoBehaviour mb)
        {
            var type = this.GetType();
            var fields = type.GetFields(BindingFlags.Public|BindingFlags.Instance);
            var btnType = typeof(Button);

            foreach (var fieldInfo in fields)
            {
                if(fieldInfo.FieldType!=btnType)continue;
                var fieldName = fieldInfo.Name;
                var btnComp = fieldInfo.GetValue(this) as Button;
                
                var method = mb.GetType().GetMethod($"OnClick{fieldName}",BindingFlags.Public|BindingFlags.Instance|BindingFlags.NonPublic);
                if(method==default || method.GetParameters().Length>0)continue;

                var callback = Delegate.CreateDelegate(typeof(UnityAction),mb,method) as UnityAction;
                btnComp.AddListener(callback);
            }

        }

        public void TryRemoveAllBtnListener()
        {
            var type = this.GetType();
            var fields = type.GetFields(BindingFlags.Public|BindingFlags.Instance);
            var btnType = typeof(Button);

            foreach (var fieldInfo in fields)
            {
                if(fieldInfo.FieldType!=btnType)continue;
                var fieldName = fieldInfo.Name;
                var btnComp = fieldInfo.GetValue(this) as Button;
                btnComp.onClick.RemoveAllListeners();
            }

        }
        

    }

}