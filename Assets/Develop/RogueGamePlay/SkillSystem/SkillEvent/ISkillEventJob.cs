
using Unity.Entities;
using Unity.Collections;
using Unity.Jobs.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Burst;
using System;
using Unity.Mathematics;

namespace RogueGamePlay
{

    public struct SkillEventData
    {
        public SkillEvent Event;
        public Entity Origin;
        public Entity Target;
        public float3 Position;
    }

    public struct SkillEventSingleton:IComponentData
    {
        public NativeList<SkillEventData> Events;
    }

    /// <summary>
    /// 在事件添加之后执行
    /// </summary>
    [JobProducerType(typeof(ISkillEventJobExtensions.SkillEventJobProcess<>))]
    public interface ISkillEventJob
    {
        void Execute(SkillEventData eventData);
    }

    public unsafe static class ISkillEventJobExtensions
    {
        public static JobHandle Schedule<T>(this T jobData, SkillEventSingleton singleton, JobHandle inputDeps)
            where T : struct, ISkillEventJob
        {
            if(singleton.Events.Length==0)
            {
                return inputDeps;
            }
            var data = new SkillEventJobData<T>
            {
                UserJobData = jobData,
                Events = singleton.Events
            };
            var jobReflectionData = SkillEventJobProcess<T>.JobReflectionData.Data;
            var parameters = new JobsUtility.JobScheduleParameters(UnsafeUtility.AddressOf(ref data),jobReflectionData,inputDeps,ScheduleMode.Single);
            return JobsUtility.Schedule(ref parameters);
        }

        internal unsafe struct SkillEventJobData<T> where T : struct
        {
            public T UserJobData;
            public NativeList<SkillEventData> Events;
        }

        internal unsafe struct SkillEventJobProcess<T> where T : struct, ISkillEventJob
        {
            internal static readonly SharedStatic<IntPtr> JobReflectionData = SharedStatic<IntPtr>.GetOrCreate<SkillEventJobProcess<T>>();

            internal static void Initialize()
            {
                if (JobReflectionData.Data == IntPtr.Zero)
                {
                    JobReflectionData.Data = JobsUtility.CreateJobReflectionData(typeof(SkillEventJobData<T>), typeof(T), (ExecuteJobFunction)Execute);
                }
                    
            }

            public delegate void ExecuteJobFunction(ref SkillEventJobData<T> jobData);

            public static void Execute(ref SkillEventJobData<T> jobData)
            {
                foreach (var eventData in jobData.Events)
                {
                    jobData.UserJobData.Execute(eventData);
                }
            }
        }        
        
        //自动执行
        public static void EarlyJobInit<T>() where T : struct, ISkillEventJob
        {
            SkillEventJobProcess<T>.Initialize();
        }

    }
}
