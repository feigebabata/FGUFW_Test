using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Burst;
using Unity.Transforms;

namespace RogueGamePlay
{
    [BurstCompile]
    partial struct AttackerDestroySystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            EntityQuery eq = new EntityQueryBuilder(Allocator.Temp).WithAll<Attacker>().Build(ref state);
            state.RequireForUpdate(eq);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
            state.Dependency = new Job
            {
                ECB = ecb.AsParallelWriter(),
                Time = (float)SystemAPI.Time.ElapsedTime,
            }
            .ScheduleParallel(state.Dependency);
        }


        [BurstCompile]
        partial struct Job:IJobEntity
        {
            public EntityCommandBuffer.ParallelWriter ECB;

            [ReadOnly]
            public float Time;

            void Execute([ChunkIndexInQuery] int chunkInQueryIndex,Entity entity,in Attacker attacker)
            {
                if(attacker.AttackCount<=0 || attacker.StartTime+attacker.Time>Time)
                {
                    ECB.DestroyEntity(chunkInQueryIndex,entity);
                }
            }
        }
    }
}
