using FGUFW;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FGUFW.ECS
{
    public partial class World:IDisposable
    {
        public const int FRAME_COUNT = 16;
        private readonly float frameDelay = 1f/(float)FRAME_COUNT;

        public float TimeScale=1;
        public float DeltaTime=> frameDelay*TimeScale;
        public float Time {get;private set;}
        
        public int FrameIndex{get;private set;}

        /// <summary>
        /// 渲染帧索引 于逻辑帧更新时重置
        /// </summary>
        /// <value></value>
        public int RenderFrameIndex{get;private set;}

        public float RenderFrameLerp
        {
            get
            {
                float lerp = 0;
                if(ScreenHelper.FPS>0)
                {
                    float maxRanderLength = (float)ScreenHelper.FPS/FRAME_COUNT;
                    lerp = Mathf.Clamp01((RenderFrameIndex+1f)/maxRanderLength);
                }
                return lerp;
            }
        }

        private List<ISystem> _systems = new List<ISystem>();
        private float _worldCreateTime;


        private void update()
        {
            Debug.Log($"{RenderFrameIndex}");
            if (UnityEngine.Time.unscaledTime >= getFrameTime(FrameIndex))
            // if (UnityEngine.Time.time >= getFrameTime(FrameIndex))
            {
                RenderFrameIndex = 0;
                worldUpdate();
            }
            RenderFrameIndex++;
        }

        private void worldUpdate()
        {
            updateSys();
            FrameIndex++;
            this.Time += DeltaTime;
        }

        private void updateSys()
        {
            foreach (var sys in _systems)
            {
                sys.OnUpdate();
            }
        }

        private void disposeSys()
        {
            foreach (var sys in _systems)
            {
                sys.Dispose();
            }
            _systems.Clear();
        }

        private void initSystem()
        {
            AssemblyHelper.FilterClassAndStruct<ISystem>(t=>
            {
                var sys = (ISystem)Activator.CreateInstance(t);
                _systems.Add(sys);
            });
            _systems.Sort((l,r)=>r.Order-l.Order);
            foreach (var sys in _systems)
            {
                sys.OnInit(this);
            }
        }

        private float getFrameTime(int FrameIndex)
        {
            float time = FrameIndex/FRAME_COUNT;
            time += frameDelay*(FrameIndex%FRAME_COUNT);
            time += _worldCreateTime;
            return time;
        }
    }
}