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
        public int FrameIndex{get;private set;}=-1;
        private int _frameIndex;
        public float DeltaTime=> frameDelay*TimeScale;

        private List<ISystem> _systems = new List<ISystem>();
        private float _worldCreateTime;

        /// <summary>
        /// 缩放造成的误差时间
        /// </summary>
        private float _scaleTimeDistance=0;

        private void update()
        {
            while (getFrameTime(FrameIndex+1)>=Time.unscaledTime)
            {
                worldUpdate();
            }
        }

        private void worldUpdate()
        {
            FrameIndex++;

            updateSys();
            
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

        public void AddSystem(ISystem system)
        {
            if(_systems.Contains(system))return;
            _systems.Add(system);
            _systems.Sort((l,r)=>r.Order-l.Order);
        }

        public void RemoveSystem(ISystem system)
        {
            _systems.Remove(system);
        }

        public void SetSystemEnable(int systemType,bool enable)
        {
            var sys = _systems.Find(sys=>sys.Type==systemType);
            if(sys != null)
            {
                sys.Enabled = enable;
            }
        }

        public void InitSystem()
        {
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