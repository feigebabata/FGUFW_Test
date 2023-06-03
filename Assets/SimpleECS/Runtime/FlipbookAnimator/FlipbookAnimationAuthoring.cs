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
    [AddComponentMenu("SimpleECS/FlipbookAnimation")]
    [DisallowMultipleComponent]
    public class FlipbookAnimationAuthoring : MonoBehaviour,IAuthoring<FlipbookAnimation>
    {
        public int MatAnimID;
        public int StartFrame;
        public int Length;
        public float Time;

        public FlipbookAnimation Convert()
        {
            var data = new FlipbookAnimation
            {
                Enabled = this.enabled,
                MatAnimID = this.MatAnimID,
                Start = this.StartFrame,
                Length = this.Length,
                Time = this.Time,
            };
            GameObject.Destroy(this);
            return data;
        }

        void Start(){}
    }

    public struct FlipbookAnimation : IComponent
    {
        public bool Enabled {get;set;}
        public int MatAnimID;
        public int Start;
        public int Length;
        public float Time;
    }

    public struct FlipbookAnimationUpdate : IComponent
    {
        public bool Enabled {get;set;}
        public int FlipbookIndex;
    }
    
}
