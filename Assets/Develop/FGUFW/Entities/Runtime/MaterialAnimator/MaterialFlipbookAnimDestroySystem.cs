using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Rendering;
using UnityEngine;
using Unity.Mathematics;
using System;
using Unity.Collections;
using UnityEngine.Rendering;
using Unity.Burst;
#pragma warning disable CS0282

namespace FGUFW.Entities
{
    [UpdateInGroup(typeof(MaterialFlipbookAnimatorSystemGroup))]
    [BurstCompile]
    public partial struct MaterialFlipbookAnimDestroySystem : ISystem
    {
        // private EntityQuery _switchEQ;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            // _switchEQ = new EntityQueryBuilder(Allocator.Temp).WithAll<MaterialFlipbookAnimationSwitch>().Build(ref state);
    
            // var ecb = new EntityCommandBuffer(Allocator.Temp);
            // ecb.AddComponent(ecb.CreateEntity(),new MaterialFlipbookAnimEventSingleton
            // {
            //     Events = new NativeList<MaterialFlipbookAnimEventCastData>(64,Allocator.Persistent)
            // });
            // ecb.Playback(state.EntityManager);

        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Dependency.Complete();
            // var singletonRW = SystemAPI.GetSingletonRW<MaterialFlipbookAnimEventSingleton>();
            // if(singletonRW.ValueRO.Events.Length>0)
            // {
            //     // Debug.Log(singletonRW.ValueRO.Events.Length);
            //     singletonRW.ValueRW.Events.Clear();
            // }

            var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

            // var entitys = _switchEQ.ToEntityArray(Allocator.Temp);
            // if(entitys.Length>0)
            // {
            //     ecb.RemoveComponent<MaterialFlipbookAnimationSwitch>(entitys);
            // }

            state.Dependency = new Job1
            {
                ECBP = ecb.AsParallelWriter(),
            }
            .ScheduleParallel(state.Dependency);

            // ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
            // state.Dependency = new Job2
            // {
            //     ECBP = ecb.AsParallelWriter(),
            // }
            // .ScheduleParallel(state.Dependency);
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
            // var singleton = SystemAPI.GetSingleton<MaterialFlipbookAnimEventSingleton>();
            // singleton.Events.Dispose();
            
        }

        

        [BurstCompile]
        partial struct Job1:IJobEntity
        {
            public EntityCommandBuffer.ParallelWriter ECBP;

            void Execute([ChunkIndexInQuery] int chunkInQueryIndex,Entity entity,in MaterialFlipbookAnimUpdate enable)
            {
                ECBP.SetComponentEnabled<MaterialFlipbookAnimUpdate>(chunkInQueryIndex,entity,false);
            }
        }

        

        // [BurstCompile]
        // partial struct Job2:IJobEntity
        // {
        //     public EntityCommandBuffer.ParallelWriter ECBP;

        //     void Execute([ChunkIndexInQuery] int chunkInQueryIndex,Entity entity,in MaterialFlipbookAnimationSwitch enable)
        //     {
        //         ECBP.SetComponentEnabled<MaterialFlipbookAnimationSwitch>(chunkInQueryIndex,entity,false);
        //     }
        // }


    }
}

