using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GUNNAC
{
    public class CameraViewRate : MonoBehaviour
    {
        public Vector2 Rate;
        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
            float x = Screen.height*Rate.x/Rate.y;
            float offset = 1 - x / Screen.width;
            var camera = GetComponent<Camera>();
            var rect = new Rect(offset/2,0,1-offset,1);
            camera.rect = rect;
        }
    }
}
