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
    partial struct ViewFollowSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            EntityQuery eq = new EntityQueryBuilder(Allocator.Temp).WithAll<ViewFollow>().Build(ref state);
            state.RequireForUpdate(eq);
        }

        public void OnUpdate(ref SystemState state)
        {
            var entity = SystemAPI.GetSingletonEntity<PlayerMovementControl>();
            var localTransform = SystemAPI.GetComponent<LocalTransform>(entity);
            state.Dependency = new Job
            {
                Position = localTransform.Position,
            }
            .ScheduleParallel(state.Dependency);
            
            foreach (var item in Camera.allCameras)
            {
                item.transform.position = localTransform.Position+new float3(0,0,-10);
            }
        }


        [BurstCompile]
        partial struct Job:IJobEntity
        {
            [ReadOnly]public float3 Position;

            void Execute(in ViewFollow viewFollow,ref LocalTransform localTransform)
            {
                localTransform.Position = Position+viewFollow.Offset;
            }
        }
    }
}
//1098 1952 548