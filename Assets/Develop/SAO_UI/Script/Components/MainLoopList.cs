using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SAO_UI
{
    //循环列表 
    public class MainLoopList : MonoBehaviour
    {
        public float DecelerationRate = 0.135f;
        private State _ListState;
        private float _velocity;//衰减: _velocity = Mathf.Pow(DecelerationRate, deltaTime); 


        public enum State
        {
            Opening,//依次出现
            Normal2Select,
            Select2Normal,
            Select,
            Scrolling,
        }
    }
}
