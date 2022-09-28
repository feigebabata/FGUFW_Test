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
        

        private List<ISystem> _systems = new List<ISystem>();
        private float _worldCreateTime;


        private void update()
        {
            // if (UnityEngine.Time.unscaledTime >= getFrameTime(FrameIndex))
            if (UnityEngine.Time.time >= getFrameTime(FrameIndex))
            {
                worldUpdate();
            }
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