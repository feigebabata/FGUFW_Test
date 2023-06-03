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
    public abstract class World<T>:IDisposable where T:World<T>
    {
        protected ISystem<T>[] _systems;
        protected ISystemOnCreate<T>[] _sysemOnCreates;
        protected ISystemOnDestroy<T>[] _sysemOnDestroys;
        protected bool[] _systemEnableds;
        protected T _world;

        public int SystemCount{get;private set;}
        public float Time{get;private set;}
        public float DeltaTime{get;private set;}
        public JobHandle Dependency = default;
        
        public World()
        {
            _world = this as T;

            _systems = getSystem();
            SystemCount = _systems.Length;
            _sysemOnCreates = new ISystemOnCreate<T>[SystemCount];
            _sysemOnDestroys = new ISystemOnDestroy<T>[SystemCount];
            _systemEnableds = new bool[SystemCount];

            for (int i = 0; i < SystemCount; i++)
            {
                var system = _systems[i];
                if(system is ISystemOnCreate<T>)_sysemOnCreates[i] = system as ISystemOnCreate<T>;
                if(system is ISystemOnDestroy<T>)_sysemOnDestroys[i] = system as ISystemOnDestroy<T>;
                _systemEnableds[i] = true;
            }
            
            systemsCreate();

            PlayerLoopHelper.AddToLoop<UnityEngine.PlayerLoop.Update,T>(systemsUpdate);
        }

        protected abstract ISystem<T>[] getSystem(); 

        public void SetSystemEnabled(int systemIndex,bool enabled)
        {
            _systemEnableds[systemIndex] = enabled;
        }

        public void Dispose()
        {
            PlayerLoopHelper.RemoveToLoop<UnityEngine.PlayerLoop.Update>(systemsUpdate);
            
            Dependency.Complete();

            systesmDestroy();
            OnDispose();
        }

        protected abstract void OnDispose(); 

        private void systemsCreate()
        {
            for (int i = 0; i < SystemCount; i++)
            {
                var system = _sysemOnCreates[i];
                if(system!=null)
                {
                    system.OnCreate(_world,i);
                }
            }
        }

        private void systemsUpdate()
        {
            this.DeltaTime = UnityEngine.Time.deltaTime;
            this.Time += DeltaTime;

            for (int i = 0; i < SystemCount; i++)
            {
                if(_systemEnableds[i])
                {
                    var system = _systems[i];
                    system.OnUpdate(_world,i);
                }
            }
        }

        private void systesmDestroy()
        {
            for (int i = 0; i < SystemCount; i++)
            {
                var system = _sysemOnDestroys[i];
                if(system!=null)
                {
                    system.OnDestroy(_world,i);
                }
            }
        }


    }
}
