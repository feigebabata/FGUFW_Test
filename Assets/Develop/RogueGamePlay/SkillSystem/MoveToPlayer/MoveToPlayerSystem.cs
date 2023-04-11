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
    partial struct MoveToPlayerSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            EntityQuery eq = new EntityQueryBuilder(Allocator.Temp).WithAll<MoveToPlayer,ForceMovement>().Build(ref state);
            state.RequireForUpdate(eq);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var entity = SystemAPI.GetSingletonEntity<Player>();
            var playerLT = SystemAPI.GetComponent<LocalTransform>(entity);
            
            state.Dependency = new Job
            {
                Position = playerLT.Position,
            }
            .ScheduleParallel(state.Dependency);
        }


        [BurstCompile]
        partial struct Job:IJobEntity
        {
            [ReadOnly]
            public float3 Position;

            void Execute(in MoveToPlayer moveToPlayer,ref ForceMovementTarget forceMovement)
            {
                forceMovement.TargetPoint = Position;
            }
        }
    }
}
