using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using UnityEngine.Jobs;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Collections;
using System;
using FGUFW;

namespace FGUFW.SimpleECS
{
    [AddComponentMenu("SimpleECS/FlipbookAnimator")]
    [DisallowMultipleComponent]
    public class FlipbookAnimatorAuthoring : MonoBehaviour,IAuthoring<FlipbookAnimator>
    {
        public float Speed=1;//播放速度
        public float Time;//当前时间 每帧叠加detalTime 切换动画时需要置为0
        public int FrameIndex;//当前帧索引
        public bool Loop;

        public FlipbookAnimator Convert()
        {
            var data = new FlipbookAnimator
            {
                Enabled = this.enabled,
                Speed = this.Speed,
                FrameIndex = this.FrameIndex,
                Loop = this.Loop,
            };
            GameObject.Destroy(this);
            return data;
        }

        void Start(){}
    }

    public struct FlipbookAnimator : IComponent
    {
        public bool Enabled {get;set;}
        public float Speed;//播放速度
        public float Time;//当前时间 每帧叠加detalTime 切换动画时需要置为0
        public int FrameIndex;//当前帧索引
        public bool Loop;
    }    
    
}
