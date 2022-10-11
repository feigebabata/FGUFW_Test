using FGUFW;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FGUFW.ECS
{
    public partial class World:IDisposable
    {
        public const int FRAME_COUNT = 20;
        private readonly float frameDelay = 1f/(float)FRAME_COUNT;

        public float TimeScale=1f;
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
        private float _nextUpdateTime = 0;

        /// <summary>
        /// 没逻辑帧跑多少次渲染帧
        /// </summary>
        private float _maxRanderLength;

        private void onCreateSystem()
        {
            UnityEngine.Random.InitState(314);
            _maxRanderLength = (float)ScreenHelper.SmoothFPS/FRAME_COUNT;
            initSystem();
            PlayerLoopHelper.AddToLoop<UnityEngine.PlayerLoop.Update,World>(update,true);
            DateTime.UtcNow.SetRecord();
        }

        private void onDestorySystem()
        {
            PlayerLoopHelper.RemoveToLoop<UnityEngine.PlayerLoop.Update>(update);
            
            disposeSys();
        }



        private void update()
        {
            RenderFrameIndex++;
            frameSyncUpdate();
            bool canUpdate = getCanUpdate();
            if (canUpdate)
            {
                // Debug.Log(DateTime.UtcNow.GetRecordTime());
                DateTime.UtcNow.SetRecord();
                //获取平滑的渲染帧/逻辑帧
                _maxRanderLength = Mathf.Lerp(_maxRanderLength,RenderFrameIndex,0.5f);
                Cmd2Comp?.Convert(this,_frameOperates[FrameIndex][0]);
                worldUpdate();
                RenderFrameIndex = 0;
                _nextUpdateTime = TimeHelper.Time+frameDelay;
            }
            setRenderFrameLerp();
        }

        private void setRenderFrameLerp()
        {
            float newLerp = Mathf.Clamp01((RenderFrameIndex+1f)/_maxRanderLength);
            RenderFrameLerp = newLerp;
        }

        private bool getCanUpdate()
        {
            bool delayEnd = getDelayEnd();
            bool frameOperateComplete = getFrameOperateComplete();
            return delayEnd && frameOperateComplete;
        }

        private bool getDelayEnd()
        {
            return TimeHelper.Time > _nextUpdateTime;
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

    }
}