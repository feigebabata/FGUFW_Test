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

        /// <summary>
        /// 逻辑帧下次更新事件
        /// </summary>
        /// <value></value>
        public float NextUpdateTime{get;private set;}

        /// <summary>
        /// 随机数
        /// </summary>
        /// <value></value>
        public System.Random Random{get;private set;}

        public Action<World> OnPreUpdate;

        /// <summary>
        /// 上一帧更新延迟 延迟=实际间隔事件-frameDelay
        /// </summary>
        /// <value></value>
        public float PrevFrameUpdateDelay{get;private set;}

        private float _prevFrameUpdateTime;

        private List<ISystem> _systems = new List<ISystem>();

        /// <summary>
        /// 每逻辑帧跑多少次渲染帧
        /// </summary>
        private float _maxRanderLength;
        
        /// <summary>
        /// 外接逻辑帧更新影响
        /// </summary>
        private List<Func<World, bool>> _canUpdates;


        

        private void onCreateSystem(int seed)
        {
            _canUpdates = new List<Func<World, bool>>();
            this.Random = new System.Random(seed);
            _maxRanderLength = (float)ScreenHelper.SmoothFPS/FRAME_COUNT;
            initSystem();
            PlayerLoopHelper.AddToLoop<UnityEngine.PlayerLoop.Update,World>(update,true);
            DateTime.UtcNow.SetRecord();
        }

        private void onDestorySystem()
        {
            _canUpdates.Clear();
            PlayerLoopHelper.RemoveToLoop<UnityEngine.PlayerLoop.Update>(update);
            
            disposeSys();
        }


        private void update()
        {
            RenderFrameIndex++;

            bool delayEnd = getDelayEnd();
            bool canUpdate = getCanUpdate();
            
            if (delayEnd && canUpdate)
            {
                //获取平滑的渲染帧/逻辑帧
                _maxRanderLength = Mathf.Lerp(_maxRanderLength,RenderFrameIndex,0.5f);

                if(OnPreUpdate!=null)OnPreUpdate(this);
                
                worldUpdate();

                RenderFrameIndex = 0;
                NextUpdateTime = TimeHelper.Time+frameDelay;
                
                PrevFrameUpdateDelay = TimeHelper.Time-_prevFrameUpdateTime-frameDelay;
                _prevFrameUpdateTime = TimeHelper.Time;
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
            for (int i = 0; i < _canUpdates.Count; i++)
            {
                if(!_canUpdates[i](this))return false;
            }
            return true;
        }

        private bool getDelayEnd()
        {
            return TimeHelper.Time >= NextUpdateTime;
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

        public void AddCanUpdate(Func<World,bool> canUpdate)
        {
            _canUpdates?.Add(canUpdate);
        }

        public void RemoveCanUpdate(Func<World,bool> canUpdate)
        {
            _canUpdates?.Remove(canUpdate);
        }


    }
}