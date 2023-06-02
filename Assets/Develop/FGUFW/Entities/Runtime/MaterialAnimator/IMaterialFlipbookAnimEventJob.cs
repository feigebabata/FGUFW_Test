// using System.Collections;
// using System.Collections.Generic;
// using Unity.Entities;
// using Unity.Rendering;
// using UnityEngine;
// using Unity.Mathematics;
// using System;
// using Unity.Collections;
// using UnityEngine.Rendering;
// using Unity.Burst;
// using Unity.Jobs.LowLevel.Unsafe;
// using Unity.Jobs;
// using Unity.Collections.LowLevel.Unsafe;

// namespace FGUFW.Entities
// {

//     /// <summary>
//     /// 在 MaterialFlipbookAnimatorSystemGroup 之后
//     /// </summary>
//     [JobProducerType(typeof(IMaterialFlipbookAnimEventJobExtensions.MaterialFlipbookAnimEventJobProcess<>))]
//     public interface IMaterialFlipbookAnimEventJob
//     {
//         void Execute(Entity entity,int frameEvent);
//     }

//     public struct MaterialFlipbookAnimEventCastData
//     {
//         public Entity Self;
//         public int Event;
//     }

//     public struct MaterialFlipbookAnimEventSingleton:IComponentData
//     {
//         public NativeList<MaterialFlipbookAnimEventCastData> Events;
//     }

//     public unsafe static class IMaterialFlipbookAnimEventJobExtensions
//     {
//         public static JobHandle Schedule<T>(this T jobData, MaterialFlipbookAnimEventSingleton singleton, JobHandle inputDeps)
//             where T : struct, IMaterialFlipbookAnimEventJob
//         {
//             if(singleton.Events.Length==0)
//             {
//                 return inputDeps;
//             }
//             var data = new MaterialFlipbookAnimEventJobData<T>
//             {
//                 UserJobData = jobData,
//                 Events = singleton.Events
//             };
//             var jobReflectionData = MaterialFlipbookAnimEventJobProcess<T>.JobReflectionData.Data;
//             var parameters = new JobsUtility.JobScheduleParameters(UnsafeUtility.AddressOf(ref data),jobReflectionData,inputDeps,ScheduleMode.Single);
//             return JobsUtility.Schedule(ref parameters);
//         }


//         internal unsafe struct MaterialFlipbookAnimEventJobData<T> where T : struct
//         {
//             public T UserJobData;
//             public NativeList<MaterialFlipbookAnimEventCastData> Events;
//         }

//         internal struct MaterialFlipbookAnimEventJobProcess<T> where T : struct, IMaterialFlipbookAnimEventJob
//         {
//             internal static readonly SharedStatic<IntPtr> JobReflectionData = SharedStatic<IntPtr>.GetOrCreate<MaterialFlipbookAnimEventJobProcess<T>>();

//             internal static void Initialize()
//             {
//                 if (JobReflectionData.Data == IntPtr.Zero)
//                 {
//                     JobReflectionData.Data = JobsUtility.CreateJobReflectionData(typeof(MaterialFlipbookAnimEventJobData<T>), typeof(T), (ExecuteJobFunction)Execute);
//                 }
                    
//             }

//             public delegate void ExecuteJobFunction(ref MaterialFlipbookAnimEventJobData<T> jobData);

//             public unsafe static void Execute(ref MaterialFlipbookAnimEventJobData<T> jobData)
//             {
//                 foreach (var eventData in jobData.Events)
//                 {
//                     jobData.UserJobData.Execute(eventData.Self,eventData.Event);
//                 }
//             }
//         }        
        
//         //自动执行
//         public static void EarlyJobInit<T>() where T : struct, IMaterialFlipbookAnimEventJob
//         {
//             MaterialFlipbookAnimEventJobProcess<T>.Initialize();
//         }

//     }
// }
