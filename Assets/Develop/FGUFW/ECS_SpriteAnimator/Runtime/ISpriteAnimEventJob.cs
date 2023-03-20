using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Burst;
using static Unity.Entities.SystemAPI;
using System;
using Unity.Jobs;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs.LowLevel.Unsafe;

namespace FGUFW.ECS_SpriteAnimator
{
    [JobProducerType(typeof(ISpriteAnimEventJobExtensions.SpriteAnimEventJobProcess<>))]
    public interface ISpriteAnimEventJob
    {
        void Execute(Entity entity,SpriteEvent spriteEvent);
    }
    
    public struct SpriteAnimEventData
    {
        public SpriteEvent Event;
        public Entity Self;
    }

    public struct SpriteAnimEventsSingleton:IComponentData
    {
        public NativeList<SpriteAnimEventData> Events;
    }

    public unsafe static class ISpriteAnimEventJobExtensions
    {
        public static JobHandle Schedule<T>(this T jobData, SpriteAnimEventsSingleton singleton, JobHandle inputDeps)
            where T : struct, ISpriteAnimEventJob
        {
            if(singleton.Events.Length==0)
            {
                return inputDeps;
            }
            var data = new SpriteAnimEventJobData<T>
            {
                UserJobData = jobData,
                Events = singleton.Events
            };
            // Debug.LogWarning(singleton.Events.Length);
            var jobReflectionData = SpriteAnimEventJobProcess<T>.JobReflectionData.Data;
            var parameters = new JobsUtility.JobScheduleParameters(UnsafeUtility.AddressOf(ref data),jobReflectionData,inputDeps,ScheduleMode.Single);
            return JobsUtility.Schedule(ref parameters);
        }


        internal unsafe struct SpriteAnimEventJobData<T> where T : struct
        {
            public T UserJobData;
            public NativeList<SpriteAnimEventData> Events;
        }

        internal struct SpriteAnimEventJobProcess<T> where T : struct, ISpriteAnimEventJob
        {
            internal static readonly SharedStatic<IntPtr> JobReflectionData = SharedStatic<IntPtr>.GetOrCreate<SpriteAnimEventJobProcess<T>>();

            internal static void Initialize()
            {
                if (JobReflectionData.Data == IntPtr.Zero)
                {
                    JobReflectionData.Data = JobsUtility.CreateJobReflectionData(typeof(SpriteAnimEventJobData<T>), typeof(T), (ExecuteJobFunction)Execute);
                }
                    
            }

            public delegate void ExecuteJobFunction(ref SpriteAnimEventJobData<T> jobData);

            public unsafe static void Execute(ref SpriteAnimEventJobData<T> jobData)
            {
                foreach (var eventData in jobData.Events)
                {
                    jobData.UserJobData.Execute(eventData.Self,eventData.Event);
                }
            }
        }        
        
        //自动执行
        public static void EarlyJobInit<T>() where T : struct, ISpriteAnimEventJob
        {
            SpriteAnimEventJobProcess<T>.Initialize();
        }

    }

}