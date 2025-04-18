using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        

    }

}