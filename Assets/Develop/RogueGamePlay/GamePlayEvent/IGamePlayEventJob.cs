using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Jobs.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Burst;
using System;

namespace RogueGamePlay
{
    /// <summary>
    /// GamePlayEvent生成系统组
    /// </summary>
    public partial class GamePlayEventCreateSystemGroup : ComponentSystemGroup{}

    public struct GamePlayEventData
    {
        public Entity Self;
        public GamePlayEventID EventID;
    }

    public struct GamePlayEventSingleton:IComponentData
    {
        public NativeList<GamePlayEventData> Events;
    }

    [JobProducerType(typeof(IGamePlayEventJobExtensions.GamePlayEventJobProcess<>))]
    public interface IGamePlayEventJob
    {
        void Execute(Entity entity,GamePlayEventID eventID);
    }

    public unsafe static class IGamePlayEventJobExtensions
    {
        public static JobHandle Schedule<T>(this T jobData, GamePlayEventSingleton singleton, JobHandle inputDeps)
            where T : struct, IGamePlayEventJob
        {
            if(singleton.Events.Length==0)
            {
                return inputDeps;
            }
            var data = new GamePlayEventJobData<T>
            {
                UserJobData = jobData,
                Events = singleton.Events
            };
            var jobReflectionData = GamePlayEventJobProcess<T>.JobReflectionData.Data;
            var parameters = new JobsUtility.JobScheduleParameters(UnsafeUtility.AddressOf(ref data),jobReflectionData,inputDeps,ScheduleMode.Single);
            return JobsUtility.Schedule(ref parameters);
        }

        internal unsafe struct GamePlayEventJobData<T> where T : struct
        {
            public T UserJobData;
            public NativeList<GamePlayEventData> Events;
        }

        internal unsafe struct GamePlayEventJobProcess<T> where T : struct, IGamePlayEventJob
        {
            internal static readonly SharedStatic<IntPtr> JobReflectionData = SharedStatic<IntPtr>.GetOrCreate<GamePlayEventJobProcess<T>>();

            internal static void Initialize()
            {
                if (JobReflectionData.Data == IntPtr.Zero)
                {
                    JobReflectionData.Data = JobsUtility.CreateJobReflectionData(typeof(GamePlayEventJobData<T>), typeof(T), (ExecuteJobFunction)Execute);
                }
                    
            }

            public delegate void ExecuteJobFunction(ref GamePlayEventJobData<T> jobData);

            public static void Execute(ref GamePlayEventJobData<T> jobData)
            {
                foreach (var eventData in jobData.Events)
                {
                    jobData.UserJobData.Execute(eventData.Self,eventData.EventID);
                }
            }
        }        
        
        //自动执行
        public static void EarlyJobInit<T>() where T : struct, IGamePlayEventJob
        {
            GamePlayEventJobProcess<T>.Initialize();
        }

    }
}