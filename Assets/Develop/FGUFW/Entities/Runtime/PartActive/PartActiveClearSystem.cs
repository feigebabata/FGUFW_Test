using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Burst;
using Unity.Transforms;
using FGUFW.Entities;
using FGUFW;
#pragma warning disable CS0282

namespace FGUFW.Entities
{
    [UpdateBefore(typeof(PartActiveCheckSystemGroup))]
    [BurstCompile]
    partial struct PartActiveClearSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            EntityQuery eq = new EntityQueryBuilder(Allocator.Temp).WithAll<PartActive>().Build(ref state);
            state.RequireForUpdate(eq);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
            state.Dependency = new Job
            {
                ECBP = ecb.AsParallelWriter(),
            }
            .ScheduleParallel(state.Dependency);
        }

        [BurstCompile]
        partial struct Job:IJobEntity
        {
            public EntityCommandBuffer.ParallelWriter ECBP;

            void Execute([ChunkIndexInQuery] int chunkInQueryIndex,Entity entity,ref PartActive partActive)
            {
                ECBP.SetComponent(chunkInQueryIndex,entity,new PartActive
                {
                    ActivesID = new Int2s8(),
                });
            }
        }
    }
}
