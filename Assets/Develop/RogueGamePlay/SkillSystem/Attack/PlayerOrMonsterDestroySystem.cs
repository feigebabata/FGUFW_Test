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

namespace RogueGamePlay
{
    [BurstCompile]
    partial struct PlayerOrMonsterDestroySystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            EntityQuery eq = new EntityQueryBuilder(Allocator.Temp).WithAll<Player>().Build(ref state);
            state.RequireForUpdate(eq);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
            
            var player = SystemAPI.GetSingleton<Player>();
            if(player.HP<=0)
            {
                ecb.DestroyEntity(SystemAPI.GetSingletonEntity<Player>());
            }
            
            state.Dependency = new DestroyMonsterJob
            {
                ECBP = ecb.AsParallelWriter(),
            }
            .ScheduleParallel(state.Dependency);
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }

        [BurstCompile]
        partial struct DestroyMonsterJob:IJobEntity
        {
            internal EntityCommandBuffer.ParallelWriter ECBP;

            void Execute([ChunkIndexInQuery] int chunkInQueryIndex,Entity entity,in Monster monster)
            {
                if(monster.HP<=0)
                {
                    ECBP.DestroyEntity(chunkInQueryIndex,entity);
                }
            }
        }
    }
}
