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

        /// <summary>
        /// 渲染帧于逻辑帧插值
        /// </summary>
        /// <value></value>
        public float RenderFrameLerp{get;private set;}

        private List<ISystem> _systems = new List<ISystem>();
        private float _worldCreateTime;
        private float _maxRanderLength;

        private void update()
        {
            RenderFrameIndex++;
            bool canUpdate = getCanUpdate();
            if (canUpdate)
            {
                _maxRanderLength = (float)ScreenHelper.SmoothFPS/FRAME_COUNT;
                worldUpdate();
                RenderFrameIndex = 0;
                // Debug.Log($"-------{ScreenHelper.SmoothFPS}---{_maxRanderLength}------");
            }
            setRenderFrameLerp();
            // Debug.Log($"{RenderFrameIndex:D2}  {RenderFrameLerp}");
        }

        private void setRenderFrameLerp()
        {
            if(ScreenHelper.SmoothFPS>0)
            {
                RenderFrameLerp = Mathf.Clamp01((RenderFrameIndex+1f)/_maxRanderLength);
            }
        }

        private bool getCanUpdate()
        {
            // Debug.LogWarning($"{TimeHelper.Time}  {getFrameTime(FrameIndex)}");
            // Debug.LogWarning(UnityEngine.Time.deltaTime);
            return TimeHelper.Time >= getFrameTime(FrameIndex);
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